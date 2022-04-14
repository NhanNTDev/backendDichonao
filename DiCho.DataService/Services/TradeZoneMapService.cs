using DiCho.Core.Custom;
using DiCho.DataService.ViewModels;
using GoogleMaps.LocationServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DiCho.DataService.Services
{
    public partial interface ITradeZoneMapService
    {
        //Task<List<Properties>> GetListZone();
        Task<List<Zones>> GetListZone();
        Task<Properties> GetZone(int? zoneId);
        Task<Properties> CheckZone(string address);
        Task<int> CheckZoneForCampagin(string address);
        Task<Properties> CheckZonebyLongLat(decimal longitude, decimal latitude);
        Task<string> GetAddress(string address);
        Task<string> GetAddressFromLatLong(double longitude, double latitude);
    }

    public partial class TradeZoneMapService : ITradeZoneMapService
    {
        private readonly IConfiguration _configuration;

        public TradeZoneMapService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static async Task<string> TradeZoneToken()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.tradezonemap.com/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            var userTradeZone = new { userName = "test", password = "123456" };
            var content = new StringContent(JsonConvert.SerializeObject(userTradeZone), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await client.PostAsync("api/v1.0/accounts/authenticate-username-pass", content);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();
            return responseContent;
        }

        private decimal[] GetLatLong(string address)
        {
            var locationService = new GoogleLocationService(_configuration["Geocoding:ApiKey"]);

            var point = locationService.GetLatLongFromAddress(address);
            var latitude = point.Latitude;
            var longitude = point.Longitude;

            decimal[] result = { (decimal)longitude, (decimal)latitude  /*106.76635f, 10.740447f*/ };
            return result;
        }

        public async Task<string> GetAddress(string address)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://maps.googleapis.com/");
            using HttpResponseMessage response = await client.GetAsync($"maps/api/geocode/json?key={_configuration["Geocoding:ApiKey"]}&address={address}&sensor=false");
            var responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();
            string formattedAddress = "";
            Rootobject location = JsonConvert.DeserializeObject<Rootobject>(responseContent);
            var resultLocation = new List<Result>();
            foreach (var result in location.results)
            {
                if (resultLocation.Count == 0)
                {
                    formattedAddress = result.formatted_address;
                    resultLocation.Add(result);
                }
            }
            return formattedAddress;
        }
        
        public async Task<string> GetAddressFromLatLong(double longitude, double latitude)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://maps.google.com/");
            using HttpResponseMessage response = await client.GetAsync($"maps/api/geocode/json?latlng={latitude},{longitude}&key={_configuration["Geocoding:ApiKey"]}");
            var responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();
            string formattedAddress = "";
            Rootobject location = JsonConvert.DeserializeObject<Rootobject>(responseContent);
            var resultLocation = new List<Result>();
            foreach (var result in location.results)
            {
                if (resultLocation.Count == 0)
                {
                    formattedAddress = result.formatted_address;
                    resultLocation.Add(result);
                }
            }
            return formattedAddress;
        }
        
        public async Task<List<Zones>> GetListZone()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.tradezonemap.com/");

            var tradeZoneToken = await TradeZoneToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tradeZoneToken);

            using HttpResponseMessage response = await client.GetAsync("api/v1.0/group-zones");
            var responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();

            Rootobject zone = JsonConvert.DeserializeObject<Rootobject>(responseContent);

            var properties = new List<Properties>();
            foreach (var feature in zone.features)
            {
                properties.Add(feature.properties);
            }

            var listZone = new List<Zones>();
            foreach (var propertie in properties)
            {
                listZone.Add(new Zones
                {
                    Id = propertie.f4,
                    Name = propertie.f1
                });
            }

            return listZone;
        }

        public async Task<Properties> GetZone(int? zoneId)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.tradezonemap.com/");

            var tradeZoneToken = await TradeZoneToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tradeZoneToken);

            using HttpResponseMessage response = await client.GetAsync("api/v1.0/group-zones");
            var responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();

            Rootobject zone = JsonConvert.DeserializeObject<Rootobject>(responseContent);

            var result = new Properties();
            foreach (var feature in zone.features)
            {
                if (feature.properties.f4 == zoneId)
                    result = feature.properties;
            }
            return result;
        }

        public async Task<Properties> CheckZone(string address)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.tradezonemap.com/");

            var tradeZoneToken = await TradeZoneToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tradeZoneToken);

            using HttpResponseMessage response = await client.GetAsync("api/v1.0/group-zones");
            var responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();

            Rootobject zone = JsonConvert.DeserializeObject<Rootobject>(responseContent);

            var longlat = GetLatLong(address);
            var Properties = new Properties();
            foreach (var feature in zone.features)
            {
                if (Properties != null)
                {
                    decimal[][] coordinate = feature.geometry.coordinates[0];
                    if (coordinate[0] != coordinate[^1])
                        coordinate[^1] = coordinate[0];
                    bool check = false;
                    int j = 0;
                    var x = longlat[1]; // x is latitute
                    var y = longlat[0]; // y is longitute
                    int n = coordinate.Length;
                    for (int i = 0; i < n; i++)
                    {
                        j++;
                        if (j == n)
                            j = 0;
                        if ((coordinate[i][0] < y) && (coordinate[j][0] >= y) || (coordinate[j][0] < y) && (coordinate[i][0] >= y))
                        {
                            if ((coordinate[i][1] + (y - coordinate[i][0]) / (coordinate[j][0] - coordinate[i][0]) * (coordinate[j][1] - coordinate[i][1]) < x))
                            {
                                check = !check;
                                if (check)
                                    Properties = feature.properties;
                            }
                        }
                    }
                }
                else
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Địa chỉ hiện tại chưa được hỗ trợ!");
            }

            return Properties;
        }

        public async Task<Properties> CheckZonebyLongLat(decimal longitude, decimal latitude)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.tradezonemap.com/");

            var tradeZoneToken = await TradeZoneToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tradeZoneToken);

            using HttpResponseMessage response = await client.GetAsync("api/v1.0/group-zones");
            var responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();

            Rootobject zone = JsonConvert.DeserializeObject<Rootobject>(responseContent);

            var Properties = new Properties();
            foreach (var feature in zone.features)
            {
                if (Properties != null)
                {
                    decimal[][] coordinate = feature.geometry.coordinates[0];
                    if (coordinate[0] != coordinate[^1])
                        coordinate[^1] = coordinate[0];
                    bool check = false;
                    int j = 0;
                    var x = latitude; // x is latitute
                    var y = longitude; // y is longitute
                    int n = coordinate.Length;
                    for (int i = 0; i < n; i++)
                    {
                        j++;
                        if (j == n)
                            j = 0;
                        if ((coordinate[i][0] < y) && (coordinate[j][0] >= y) || (coordinate[j][0] < y) && (coordinate[i][0] >= y))
                        {
                            if ((coordinate[i][1] + (y - coordinate[i][0]) / (coordinate[j][0] - coordinate[i][0]) * (coordinate[j][1] - coordinate[i][1]) < x))
                            {
                                check = !check;
                                if (check)
                                    Properties = feature.properties;
                            }
                        }
                    }
                }
                else
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Địa chỉ hiện tại chưa được hỗ trợ!");
            }

            return Properties;
        }

        public async Task<int> CheckZoneForCampagin(string address)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.tradezonemap.com/");

            var tradeZoneToken = await TradeZoneToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tradeZoneToken);

            using HttpResponseMessage response = await client.GetAsync("api/v1.0/group-zones");
            var responseContent = response.Content.ReadAsStringAsync().Result;
            response.EnsureSuccessStatusCode();

            Rootobject zone = JsonConvert.DeserializeObject<Rootobject>(responseContent);

            var longlat = GetLatLong(address);
            var Properties = new Properties();
            foreach (var feature in zone.features)
            {
                if (Properties != null)
                {
                    decimal[][] coordinate = feature.geometry.coordinates[0];
                    if (coordinate[0] != coordinate[^1])
                        coordinate[^1] = coordinate[0];
                    bool check = false;
                    int j = 0;
                    var x = longlat[1]; // x is latitute
                    var y = longlat[0]; // y is longitute
                    int n = coordinate.Length;
                    for (int i = 0; i < n; i++)
                    {
                        j++;
                        if (j == n)
                            j = 0;
                        if ((coordinate[i][0] < y) && (coordinate[j][0] >= y) || (coordinate[j][0] < y) && (coordinate[i][0] >= y))
                        {
                            if ((coordinate[i][1] + (y - coordinate[i][0]) / (coordinate[j][0] - coordinate[i][0]) * (coordinate[j][1] - coordinate[i][1]) < x))
                            {
                                check = !check;
                                if (check)
                                    Properties = feature.properties;
                            }
                        }
                    }
                }
                else
                    return 0;
            }

            return Properties.f4;
        }
    }
}
