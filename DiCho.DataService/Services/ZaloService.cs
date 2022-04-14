using DiCho.DataService.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ZaloDotNetSDK;

namespace DiCho.DataService.Services
{
    public interface IZaloService
    {
        Task SendMessage();
        Task<TokenModel> GetInfo(string code);
    }

    public class ZaloService : IZaloService
    {
        private readonly IConfiguration _configuration;
        private readonly ITradeZoneMapService _tradeZoneMapService;
        private readonly IJWTService _jWTService;
        private readonly IRedisCacheClient _redisCacheClient;
        
        public ZaloService(ITradeZoneMapService tradeZoneMapService, IJWTService jWTService, IConfiguration configuration,
            IRedisCacheClient redisCacheClient)
        {
            _tradeZoneMapService = tradeZoneMapService;
            _redisCacheClient = redisCacheClient;
            _jWTService = jWTService;
            _configuration = configuration;
        }
        public async Task SendMessage()
        {
            ZaloClient clientZalo = new(_configuration["Zalo:Oa"]);
            var users = clientZalo.getListRecentChat(0, 1);
            var convert = JsonConvert.SerializeObject(users);

            var data = JsonConvert.DeserializeObject<ZaloModel>(convert);

            var user_data = data.Data.Where(x => x.Src == 1).FirstOrDefault();
            var convertLocation = JsonConvert.DeserializeObject<ZaloLocation>(user_data.Location);
            var longitude = convertLocation.Longitude;
            var latitude = convertLocation.Latitude;
            var address = await _tradeZoneMapService.GetAddressFromLatLong(longitude, latitude);
            var encodeAddress = HttpUtility.UrlEncode(address);
            var uri = "https://dichonaocustomer.azurewebsites.net/home";

            var codeVerifier = GenerateNonce();
            string code = GenerateCodeChallenge(codeVerifier);
            var guid = Guid.NewGuid().ToString();
            var url = $"https://oauth.zaloapp.com/v4/permission?app_id=513303371438730637&redirect_uri={uri}&code_challenge={code}&state={guid}";

            var userModel = new UserZaloModel { Code = codeVerifier, Address = address };
            await _redisCacheClient.Db1.AddAsync<UserZaloModel>("code", userModel);

            Object sendMessage;
            if (user_data != null)
                sendMessage = clientZalo.sendTextMessageToMessageId(user_data.Message_id, "Mua hàng ngay tại đây: " + url);
        }

        private static string GenerateNonce()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
            var random = new Random();
            var nonce = new char[128];
            for (int i = 0; i < nonce.Length; i++)
            {
                nonce[i] = chars[random.Next(chars.Length)];
            }

            return new string(nonce);
        }

        private static string GenerateCodeChallenge(string codeVerifier)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            var b64Hash = Convert.ToBase64String(hash);
            var code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");
            return code;
        }

        public async Task<TokenModel> GetInfo(string code)
        {
            var userZaloModel = await _redisCacheClient.Db1.GetAsync<UserZaloModel>("code");
            string access_token;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://oauth.zaloapp.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Add("secret_key", "bQXS5ijeyvNLxxIS3LSH");


                HttpContent content = new FormUrlEncodedContent(
                new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("app_id", "513303371438730637"),
                    new KeyValuePair<string,string>("code", $"{code}"),
                    new KeyValuePair<string,string>("grant_type", "authorization_code"),
                    new KeyValuePair<string,string>("code_verifier", $"{userZaloModel.Code}")
                });

                using (HttpResponseMessage response = await client.PostAsync("v4/access_token", content))
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;

                    response.EnsureSuccessStatusCode();

                    access_token = JsonConvert.DeserializeObject<AccessToken>(responseContent).Access_token;

                }
            }
            using var clientUser = new HttpClient();
            clientUser.BaseAddress = new Uri("https://graph.zalo.me/");
            clientUser.DefaultRequestHeaders.Add("access_token", $"{access_token}");
            using HttpResponseMessage responseUser = await clientUser.GetAsync("v2.0/me?fields=id,name,picture");

            var responseContentUser = responseUser.Content.ReadAsStringAsync().Result;

            var user = JsonConvert.DeserializeObject<UserZalo>(responseContentUser);

            var dataUser = new UserDataZalo { Id = user.Id, Name = user.Name, Url = user.Picture.Data.Url, Address = userZaloModel.Address };

            var userResult = await _jWTService.LoginUserZalo(dataUser);
            return userResult;
        }
    }
}
