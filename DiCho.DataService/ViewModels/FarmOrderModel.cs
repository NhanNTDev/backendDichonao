using DiCho.Core.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class FarmOrderModel
    {
        public static string[] Fields = {
            "Id", "Code", "Total", "Status", "CreateAt", "OrderId", "Farm", "Star", "Content", "FeedBackCreateAt", "Note"
        };
        [BindNever]
        public int? Id { get; set; }
        [StringAttribute]
        public string Code { get; set; }
        [BindNever]
        public double? Total { get; set; }
        [StringAttribute]
        public string Status { get; set; }
        [BindNever]
        public DateTime? CreateAt { get; set; }
        [BindNever]
        public string DateTimeParse { get; set; }
        [BindNever]
        public double? Star { get; set; }
        [BindNever]
        public string Content { get; set; }
        [BindNever]
        public DateTime? FeedBackCreateAt { get; set; }
        [BindNever]
        public string Note { get; set; }
        [BindNever]
        public int? OrderId { get; set; }
        [BindNever]
        public string CustomerName { get; set; }
        [BindNever]
        public string CampaignName { get; set; }
        [BindNever]
        public string PaymentStatus { get; set; }
        [BindNever]
        public string PaymentTypeName { get; set; }
        public virtual FarmMappingHarvestModel Farm { get; set; }
    }

    public class FarmOrderDetailModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public double? Total { get; set; }
        public string Status { get; set; }
        public int? OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CampaignName { get; set; }
        public DateTime? CreateAt { get; set; }
        public string DateTimeParse { get; set; }
        public double? Star { get; set; }
        public string Content { get; set; }
        public DateTime? FeedBackCreateAt { get; set; }
        public string Note { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentTypeName { get; set; }
        public virtual FarmMappingHarvestModel Farm { get; set; }
        public virtual ICollection<ProductHarvestOrderDetailModel> ProductHarvestOrders { get; set; }
    }

    public class FarmOrderCreateModelInput
    {
        public int? FarmId { get; set; }
        public virtual ICollection<HarvestOrderCreateModelInput> ProductHarvestOrders { get; set; }
    }

    public class FarmOrderCreateModel
    {
        public int? FarmId { get; set; }
        public virtual ICollection<ProductHarvestOrderCreateModel> ProductHarvestOrders { get; set; }
    }

    public class FarmOrderUpdateModel
    {
        public int? Id { get; set; }
        public int? Status { get; set; }
    }
    public class FarmOrderUpdateDriverInputModel
    {
        public int FarmId { get; set; }
        public string DriverId { get; set; }
    }
    
    public class FarmOrderUpdateDriverModel
    {
        public int? Id { get; set; }
        public string DriverId { get; set; }
    }

    public class FarmOrderUpdateCancelModel
    {
        public int? Id { get; set; }
        public int? Status { get; set; }
        public string Note { get; set; }
    }
    public class FarmOrderMappingHarvestOrder
    {
        [BindNever]
        public int? Id { get; set; }
        [BindNever]
        public int? Status { get; set; }
        [BindNever]
        public double Total { get; set; }
        [BindNever]
        public virtual ICollection<ProductHarvestOrderMappingFarmOrderModel> ProductHarvestOrders { get; set; }
    }

    public class FarmOrderMapOrderDataModel
    {
        public double Total { get; set; }
        public virtual OrderMapCustomerDataModel Order { get; set; }
    }

    public class FarmOrderDataMapToHarvestCampaignModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double? Total { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public double? Star { get; set; }
        public string Content { get; set; }
        public DateTime? FeedBackCreateAt { get; set; }
        public int? FarmId { get; set; }
        public int? OrderId { get; set; }
        public string DriverId { get; set; }
        public double Weight { get; set; }
        public virtual FarmMappingCampaignModel Farm { get; set; }
        public virtual ICollection<ProductHarvestOrderGroupFarmOrderModel> ProductHarvestOrders { get; set; }
    }

    public class FarmOrderMapToFarmForDriverModel
    {
        public virtual FarmMapToFarmOrderDriverModel Farm { get; set; }
    }

    public class FarmOrderGroup
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Note { get; set; }
        public int? FarmId { get; set; }
        public string Phone { get; set; }
        public int? OrderId { get; set; }
        public string DriverId { get; set; }

        public virtual FarmMappingHarvestModel Farm { get; set; }
        public virtual ICollection<ProductHarvestOrderGroup> ProductHarvestOrders { get; set; }
    }

    public class FarmOrderDetail
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Note { get; set; }
        public int? FarmId { get; set; }
        public string FarmName { get; set; }
        public string FarmAddress { get; set; }
        public string Phone { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public int? OrderId { get; set; }

        public virtual ICollection<ProductHarvestOrderGroup> ProductHarvestOrders { get; set; }
    }

    public class FarmOrderGroupFarm
    {
        public int? FarmId { get; set; }
        public List<FarmOrderGroup> FarmOrders { get; set; }
    }

    //public class FarmOrderGroupFarmDeliveryModel
    //{
    //    public int? FarmId { get; set; }
    //    public string FarmName { get; set; }
    //    public string FarmAddress { get; set; }
    //    public int? CountFarmOrder { get; set; }
    //    public string DriverName { get; set; }
    //    public double TotalWeight { get; set; }
    //}
    public class FarmOrderGroupFarmDeliveryModel
    {
        public int? FarmId { get; set; }
        public string FarmAddress { get; set; }
        public double TotalWeight { get; set; }
        public List<FarmOrderGroupModel> FarmOrderGroups { get; set; }
    }

    public class FarmOrderGroupModel
    {
        public int Id { get; set; }
        public double Weight { get; set; }
    }

    
    public class FarmOrderForDeliveryDriverModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public virtual ICollection<ProductHarvestOrderDetailModel> ProductHarvestOrders { get; set; }
    }

    public class FarmOrderForManagerModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Note { get; set; }
        public int? FarmId { get; set; }
        public int? OrderId { get; set; }
        public string DriverId { get; set; }

        public virtual ICollection<ProductHarvestOrderDetailModel> ProductHarvestOrders { get; set; }
    }

    public class GetFarmOrderModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public double Star { get; set; }
        public string Content { get; set; }
        public DateTime? FeedBackCreateAt { get; set; }
        public string Note { get; set; }
        public int? FarmId { get; set; }
        public int? OrderId { get; set; }
        public string DriverId { get; set; }
    }

    public class FarmOrderDetailDataModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Note { get; set; }
        public int? FarmId { get; set; }
        public int? OrderId { get; set; }
        public string DriverId { get; set; }

        public virtual ICollection<ProductHarvestOrderDetailModel> ProductHarvestOrders { get; set; }
    }

    public class FarmOrderFeedbackModel
    {
        public int? Id { get; set; }
        public DateTime? CreateAt { get; set; }
        public double? Star { get; set; }
        public string Content { get; set; }
        public DateTime? FeedBackCreateAt { get; set; }
    }

    public class FarmOrderRevenuseModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public double Star { get; set; }
        public string Content { get; set; }
        public DateTime? FeedBackCreateAt { get; set; }
        public string Note { get; set; }
        public int? FarmId { get; set; }
        public int? OrderId { get; set; }
        public string DriverId { get; set; }
    }

    public class FarmOrderFeedbackViewModel
    {
        public int? Id { get; set; }
        public string CustomerName { get; set; }
        public string Image { get; set; }
        public double? Star { get; set; }
        public string Content { get; set; }
        public DateTime? FeedBackCreateAt { get; set; }
        public int? OrderId { get; set; }

    }

    public class CollectionOfFarmOrderModel
    {
        public string CollectionCode { get; set; }
        public double TotalWeight { get; set; }
        public List<FarmMapDataToViewGroupFarmOrder> Farms { get; set; }
    }

    public class FarmOrderMapToGroupModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Note { get; set; }
        public int? FarmId { get; set; }
        public string FarmName { get; set; }
        public string FarmAddress { get; set; }
        public string Phone { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public int? OrderId { get; set; }
        public string CollectionCode { get; set; }
    }
}
