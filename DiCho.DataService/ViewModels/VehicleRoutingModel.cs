using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class VehicleRoutingModel
    {
        public long[,] DistanceMatrix { get; set; }
        public int VehicleNumber { get; set; }
        public int Depot { get; set; }
    }
    public class VehicleRoutingViewModel
    {
        public string TotalDistance { get; set; }
        public double TotalWeight { get;set; }
        public List<RouteOfVihicle> RouteOfVihicles { get; set; }
    }

    public class RouteOfVihicle
    {
        public int Vihicle { get; set; }
        public double TotalWeightOfVihicle { get; set; }
        public Routes Routes { get; set; }
    }

    public class Routes
    {
        public Dictionary<string, double> Point { get; set; }
        public string Distance { get; set; }
    }

    public class WarehouseDataRouting
    {
        public List<string> WarehouseAddresses { get; set; }
    }

    public class DistanceMatrixLong
    {
        public List<long> DistanceMatrix { get; set; }
    }

    public class VehicleRoutingAddressModel
    {
        public string Api_key = "AIzaSyDpCHIvLJ7SFW8wXTjShluXxRWfEkmGECA";
        public string[] Address =
        {
            "Unnamed Road, Gia Canh, Định Quán, Đồng Nai, Việt Nam",
            "451 Đ. Lê Văn Việt, Tăng Nhơn Phú A, thành phố Thủ Đức, Thành phố Hồ Chí Minh, Việt Nam",
            "786 Nguyễn Kiệm, Street, Gò Vấp, Thành phố Hồ Chí Minh, Việt Nam",
            "Bùi Công Trừng, Thạnh Xuân, Quận 12, Thành phố Hồ Chí Minh, Việt Nam",
            "270 Bình Thới, Phường 10, Quận 11, Thành phố Hồ Chí Minh, Việt Nam",
            "106 Đường Số 23, Phường 11, Quận 6, Thành phố Hồ Chí Minh, Việt Nam",
            "600 Nguyễn Lương Bằng, Phú Mỹ, Quận 7, Thành phố Hồ Chí Minh, Việt Nam"

            //"314 Đ. Lê Thị Riêng, Tân Thới An, Quận 12, Thành phố Hồ Chí Minh, Việt Nam",
            //"186 Đội Cung, Phường 8, Quận 11, Thành phố Hồ Chí Minh, Việt Nam",
            //"2/2 Đường 106, Phường Tăng Nhơn Phú A, Quận 9, Thành Phố Hồ Chí Minh"
        };
    }

    public class VehicleRoutingViewModel1
    {
        public string TotalDistance { get; set; }
        public List<RouteOfVihicle1> RouteOfVihicles { get; set; }
    }

    public class RouteOfVihicle1
    {
        public int Vihicle { get; set; }
        public Routes1 Routes { get; set; }
    }

    public class Routes1
    {
        public List<string> Point { get; set; }
        public string Distance { get; set; }
    }


    public class DistanceMatrixModel
    {
        public string[] Destination_addresses { get; set; }
        public string[] Origin_addresses { get; set; }
        public Row[] Rows { get; set; }
        public string Status { get; set; }
    }

    public class Row
    {
        public Element[] Elements { get; set; }
    }

    public class Element
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public string Status { get; set; }
    }

    public class Distance
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class Duration
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

}
