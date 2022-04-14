using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class ShipmentModel
    {
        public int WarehouseId { get; set; }
        public List<OrderGroupZoneModel> OrderGroupZones { get; set; }
    }

    public class ShipmentCreateModel
    {
        public string Code { get; set; }
        public string From { get; set; }
        public int? Stauts { get; set; }
        public string DriverId { get; set; }
        public double TotalWeight { get; set; }
    }

    public class ShipmentUpdateModel
    {
        public int Id { get; set; }
        public string DriverId { get; set; }
    }

    public class ShipmentForDriverModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string WarehouseFrom { get; set; }
        public string From { get; set; }
        public string Status { get; set; }
        public string DriverId { get; set; }
        public double TotalWeight { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual ICollection<ShipmentDestinationViewModel> ShipmentDestinations { get; set; }
    }

    public class ShipmentForWareHouseManagerModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string From { get; set; }
        public string FromWarehouse { get; set; }
        public string Status { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public double TotalWeight { get; set; }
        public DateTime CreateAt { get; set; }
        public string DateTimeParse { get; set; }

        public virtual ICollection<ShipmentDestinationViewModel> ShipmentDestinations { get; set; }
    }

    public class ShipmentDetailForWareHouseManagerModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string From { get; set; }
        public string FromWarehouse { get; set; }
        public string Status { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public double TotalWeight { get; set; }

        public List<OrderForManagerModel> Orders { get; set; }
    }

    public class DashBoardOfWarehouse
    {
        public int FarmOrdersCollect { get; set; }
        public int ShipmentsTransport { get; set; }
        public int OrdersDelivery { get; set; }
        public int Drivers { get; set; }
    }
}
