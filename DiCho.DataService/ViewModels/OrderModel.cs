using DiCho.Core.Attributes;
using DiCho.DataService.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class OrderModel
    {
        public static string[] Fields = {
            "Id", "Code", "Total", "Phone", "Address", "Status", "CreateAt", "Campaign", "Customer", "DeliveryZoneId", "ShipCost"
        };
        [BindNever]
        public int? Id { get; set; }
        public string Code { get; set; }
        [BindNever]
        public double? Total { get; set; }
        [BindNever]
        public double? ShipCost { get; set; }
        [BindNever]
        public string Phone { get; set; }
        [BindNever]
        public string Address { get; set; }
        public string Status { get; set; }
        [BindNever]
        public DateTime? CreateAt { get; set; }
        [BindNever]
        public string DateTimeParse { get; set; }
        [BindNever]
        public int? DeliveryZoneId { get; set; }
        [BindNever]
        public double? Star { get; set; }
        [BindNever]
        public string Content { get; set; }
        [BindNever]
        public DateTime? FeedbackCreateAt { get; set; }
        public virtual CampaignMappingModel Campaign { get; set; }
        public virtual UserMappingModel Customer { get; set; }
    }
    public class OrderCreateModelInput
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CustomerId { get; set; }
        public int PaymentTypeId { get; set; }
        public int? CampaignId { get; set; }
        public virtual ICollection<FarmOrderCreateModelInput> FarmOrders { get; set; }
    }
    public class OrderCreateModel
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CustomerId { get; set; }
        public int? CampaignId { get; set; }
        public int? DeliveryZoneId { get; set; }
        public PaymentCreateModel Payment { get; set; }
        public virtual ICollection<FarmOrderCreateModel> FarmOrders { get; set; }
    }

    public class CampaignCreateOrderModel
    {
        public int? CampaignId { get; set; }
        public virtual ICollection<FarmOrderCreateModelInput> FarmOrders { get; set; }
    }

    public class OrderUpdateModel
    {
        public int? Id { get; set; }
        public int? Status { get; set; }
    }

    public class OrderMappingFarmOrderModel
    {
        [BindNever]
        public int? Id { get; set; }
        [BindNever]
        public virtual ICollection<FarmOrderMappingHarvestOrder> FarmOrders { get; set; }
    }

    public class WarehouseGroupModel
    {
        public int WarehouseId { get; set; }
        public string WarehouseAddress { get; set; }
        public double TotalWeight { get; set; }
        public List<OrderGroupZoneModel> OrderGroupZones { get; set; }
    }

    public class OrderGroupZoneModel
    {
        public int ZoneId { get; set; }
        public double WeightOfZone { get; set; }
        public List<OrderGroupModel> Orders { get; set; }
    }

    public class OrderGroupModel
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        //public string Code { get; set; }
        //public string CampaignName { get; set; }
        //public string Address { get; set; }
        //public string Status { get; set; }
        //public DateTime? CreateAt { get; set; }
        //public string Note { get; set; }
        //public int? DeliveryZoneId { get; set; }
        //public List<FarmOrderOfGroupModel> FarmOrders { get; set; }
    }

    public class OrderModelForDriver
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
    }

    public class OrderForManagerModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public double Total { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Note { get; set; }
        public int? CampaignId { get; set; }
        public string CustomerId { get; set; }
        public int? DeliveryZoneId { get; set; }
        public int? ShipmentId { get; set; }
        public string DriverId { get; set; }
        public virtual ICollection<FarmOrderForManagerModel> FarmOrders { get; set; }
    }

    public class OrderDataMapModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public double? Total { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? CampaignId { get; set; }
        public string CustomerId { get; set; }
        public int? DeliveryZoneId { get; set; }
        public int? ShipmentId { get; set; }
        public string DriverId { get; set; }

        public virtual CampaignDataMapmodel Campaign { get; set; }
        //public virtual UserDataMapModel Customer { get; set; }
        public virtual ICollection<FarmOrderDataMapToHarvestCampaignModel> FarmOrders { get; set; }
        public virtual ICollection<PaymentDataMapModel> Payments { get; set; }
    }

    public class OrderMapCustomerDataModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public double? Total { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? CampaignId { get; set; }
        public string CustomerId { get; set; }
        public int? DeliveryZoneId { get; set; }
        public int? ShipmentId { get; set; }
        public string DriverId { get; set; }
        public virtual UserMappingModel Customer { get; set; }
    }

    public class OrderForDriverModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public double ShipCost { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string DateTimeParse { get; set; }
        public string Note { get; set; }
        public string DriverId { get; set; }

        public virtual ICollection<FarmOrderForDeliveryDriverModel> FarmOrders { get; set; }
    }

    public class UpdateDriverForOrderByWarehouse
    {
        public int Id { get; set; }
        public string DriverId { get; set; }
    }

    public class OrderForWarehouseManagerModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public double ShipCost { get; set; }
        public double Weight { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Note { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }

        //public virtual ICollection<FarmOrderForDeliveryDriverModel> FarmOrders { get; set; }
    }

    public class OrderListModel
    {
        public virtual ICollection<OrderForWarehouseManagerModel> Orders { get; set; }
    }

    public class OrderDataMapHarvestCampaignModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Note { get; set; }
        public string DriverId { get; set; }
        public int ShipmentId { get; set; }
        public virtual ICollection<FarmOrderDataMapToHarvestCampaignModel> FarmOrders { get; set; }
    }

    public class OrderDetailModel
    {
        public int? CampaignId { get; set; }
        public string CampaignName { get; set; }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public double ShipCost { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string DateTimeParse { get; set; }
        public string Note { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public double Star { get; set; }
        public string Content { get; set; }
        public DateTime? FeedbackCreateAt { get; set; }
        public virtual ICollection<ProductHarvestOrderOfDetailModel> ProductHarvestOrders { get; set; }
    }

    public class GetOrderDetailDataModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public double? Total { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? CampaignId { get; set; }
        public string CustomerId { get; set; }
        public int? DeliveryZoneId { get; set; }
        public int? ShipmentId { get; set; }
        public string DriverId { get; set; }

        public virtual CampaignDataMapmodel Campaign { get; set; }
        public virtual ICollection<FarmOrderDetailDataModel> FarmOrders { get; set; }
    }

    public class OrderFarmOrderModel
    {
        public virtual ICollection<FarmOrder> FarmOrders { get; set; }
    }

    public class OrderMapCampaignModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public double Total { get; set; }
        public int? Status { get; set; }
    }

    public class OrderOfCustomerModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public double Total { get; set; }
        public double ShipCost { get; set; }
        public string Status { get; set; }
        public DateTime CreateAt { get; set; }
        public string DateTimeParse { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public int? ShipmentId { get; set; }
        public double Star { get; set; }
        public string Content { get; set; }
        public DateTime? FeedbackCreateAt { get; set; }
        public virtual ICollection<PaymentOfCustomerModel> Payments { get; set; }
    }

    public class FeedbackOrderModel
    {
        public int Id { get; set; }
        public double Star { get; set; }
        public string Content { get; set; }
    }

    public class OrderMapDataPaymentType
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public virtual CampaignDataMapmodel Campaign { get; set; }
        public virtual ICollection<PaymentDataMapModel> Payments { get; set; }
    }

    public class TaskOfDriverModel
    {
        public int TaskOfCollections { get; set; }
        public int TaskOfDeliveries { get; set; }
        public int TaskOfShipments { get; set; }
    }

    public class OrderPaymentModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CustomerId { get; set; }
        public virtual ICollection<FarmOrderForManagerModel> FarmOrders { get; set; }
        public virtual ICollection<PaymentModel> Payments { get; set; }
    }

    public class UrlReturn
    {
        public string Url { get; set; }
    }
}
