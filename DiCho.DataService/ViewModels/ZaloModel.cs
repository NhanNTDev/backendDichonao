using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class ZaloModel
    {
        public List<ZaloDataModel> Data { get; set; }
    }

    public class ZaloDataModel
    {
        public int Src { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string Message { get; set; }
        public string Message_id { get; set; }
        public string From_id { get; set; }
        public string To_id { get; set; }
        public string From_display_name { get; set; }
        public string From_avatar { get; set; }
        public string To_display_name { get; set; }
        public string To_avatar { get; set; }
    }

    public class ZaloLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string AppId { get; set; }
        public string SrcId { get; set; }
        public int IsUserLocation { get; set; }
    }

    public class AccessToken
    {
        public string Access_token { get; set; }
        public string Refresh_token { get; set; }
    }

    public class UserZalo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Picture Picture { get; set; }
    }

    public class Picture
    {
        public DataImage Data { get; set; }
    }
    
    public class DataImage
    {
        public string Url { get; set; }
    }

    public class UserDataZalo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
    }

    public class UserZaloModel
    {
        public string Code { get; set; }
        public string Address { get; set; }
    }
}
