using DiCho.Core.Attributes;
using DiCho.DataService.Models;
using DiCho.DataService.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class CampaignModel
    {
        public static string[] Fields = {
            "Id", "Name", "Image1", "Image2", "Image3", "Image4", "Image5", "Description", "StartAt", "EndAt",
            "Type", "Status", "CampaignZoneId", "CampaignZoneName", "Note"
        };
        public int? Id { get; set; }
        [StringAttribute]
        public string Name { get; set; }
        [BindNever]
        public string Image1 { get; set; }
        [BindNever]
        public string Image2 { get; set; }
        [BindNever]
        public string Image3 { get; set; }
        [BindNever]
        public string Image4 { get; set; }
        [BindNever]
        public string Image5 { get; set; }
        [StringAttribute]
        public string Type { get; set; }
        [BindNever]
        public string Description { get; set; }
        [BindNever]
        public DateTime? StartAt { get; set; }
        [BindNever]
        public DateTime? EndAt { get; set; }
        [BindNever]
        public DateTime? ExpectedDeliveryTime { get; set; }
        [StringAttribute]
        public string Status { get; set; }
        [BindNever]
        public string Note { get; set; }
        [BindNever]
        public int? CampaignZoneId { get; set; }
        [BindNever]
        public string CampaignZoneName { get; set; }
        [BindNever]
        public int? FarmInCampaign { get; set; }
    }

    public class CampaignFarmZoneModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? ExpectedDeliveryTime { get; set; }
        public string Status { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }
        public int? FarmInCampaign { get; set; }
        public virtual ICollection<CampaignDeliveryZoneModel> CampaignDeliveryZones { get; set; }
        public virtual ICollection<ProductSalesCampaignViewModel> ProductSalesCampaigns { get; set; }
    }

    public class CampaignCreateInputModel
    {
        public string Name { get; set; }
        public List<string> Images { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int CampaignZoneId { get; set; }
        public List<int> DeliveryZoneId { get; set; }
        public virtual ICollection<ProductSalesCampaignCreateModel> ProductSalesCampaigns { get; set; }
    }
    
    public class CampaignCreateModel
    {
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? StartRecruitmentAt { get; set; }
        public DateTime? EndRecruitmentAt { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }
        public ICollection<CampaignDeliveryZoneCreateModel> CampaignDeliveryZones { get; set; }
        public virtual ICollection<ProductSalesCampaignCreateModel> ProductSalesCampaigns { get; set; }

    }

    public class CampaignUpdateInputModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<string> Images { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int? CampaignZoneId { get; set; }
        public List<int> DeliveryZoneId { get; set; }
        public virtual ICollection<ProductSalesCampaignCreateModel> ProductSalesCampaigns { get; set; }
    }

    public class CampaignUpdateModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }
        public ICollection<CampaignDeliveryZoneCreateModel> CampaignDeliveryZones { get; set; }
        public virtual ICollection<ProductSalesCampaignCreateModel> ProductSalesCampaigns { get; set; }
    }

    public class CampaignMappingModel
    {
        [BindNever]
        public int? Id { get; set; }
        [BindNever]
        public string Name { get; set; }
        [BindNever]
        public int? CampaignZoneId { get; set; }
        [BindNever]
        public string CampaignZoneName { get; set; }
    }

    public class CampaignOfFarmerAppliedModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string Status { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }
        public int? FarmInCampaign { get; set; }
        public DateTime? TimeApply { get; set; }
        public List<string> Farms { get; set; }
    }

    public class CampaignSearchModel
    {
        public static string[] Fields = {
            "Id", "Name", "Image1", "Status", "CampaignZoneId", "CampaignZoneName", "Type"
        };
        [BindNever]
        public int? Id { get; set; }
        [StringAttribute]
        public string Name { get; set; }
        [BindNever]
        public string Image1 { get; set; }
        [BindNever]
        public string Status { get; set; }
        [BindNever]
        public string Type { get; set; }
        [BindNever]
        public int? CampaignZoneId { get; set; }
        [BindNever]
        public string CampaignZoneName { get; set; }
        [BindNever]
        public List<string> Farms { get; set; }
    }
    
    public class CampaignOfFarmerApplyModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string Status { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }
        public int? FarmInCampaign { get; set; }
        public List<string> Farms { get; set; }
    }

    public class CampaignApplyRequestModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public int? FarmApplyRequest { get; set; }
    }

    public class CampaignDetailApplyModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? StartRecruitmentAt { get; set; }
        public DateTime? EndRecruitmentAt { get; set; }
        public string Status { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }
        public int? FarmInCampaign { get; set; }
        public ICollection<string> DeliveryZoneName { get; set; }
        public virtual ICollection<ProductSalesCampaignDetailModel> ProductSalesCampaigns { get; set; }
    }

    public class CampaignDetailApplieModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? StartRecruitmentAt { get; set; }
        public DateTime? EndRecruitmentAt { get; set; }
        public string Status { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }
        public int? FarmInCampaign { get; set; }
        public DateTime? TimeApply { get; set; }
        public ICollection<FarmApplyModel> Farms { get; set; }
        public ICollection<string> DeliveryZoneName { get; set; }
        public ICollection<ProductSalesCampaignDetailModel> ProductSalesCampaigns { get; set; }
    }

    public class CampaignDataMapmodel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? StartRecruitmentAt { get; set; }
        public DateTime? EndRecruitmentAt { get; set; }
        public int? Status { get; set; }
        public int? FarmZoneId { get; set; }

        //public virtual ICollection<CampaignDeliveryZone> CampaignDeliveryZones { get; set; }
        //public virtual ICollection<Order> Orders { get; set; }
    }

    public class CampaignSuggestFarmerApply
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? StartRecruitmentAt { get; set; }
        public DateTime? EndRecruitmentAt { get; set; }
        public string Status { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }

        public virtual ICollection<HarvestSuggestFarmerApplyModel> ProductHarvests { get; set; }
    }

    public class CampaignStatisticalOfAdmin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public int? Orders { get; set; }
        public double? TotalRevenues { get; set; }
        public int? Farms { get; set; }
    }

    public class CampaignMapFarmOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public virtual ICollection<OrderMapCampaignModel> Orders { get; set; }
    }

    public class CampaignMapCampaignDeliveryZoneModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CampaignDeliveryZone> CampaignDeliveryZones { get; set; }
    }

    public class CampaignGetByIdModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string Status { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }
        public int? FarmInCampaign { get; set; }
        public List<FarmModel> Farms { get; set; }
        public virtual ICollection<CampaignDeliveryZoneModel> CampaignDeliveryZones { get; set; }
        public virtual ICollection<ProductSalesCampaignModel> ProductSalesCampaigns { get; set; }
    }
}
