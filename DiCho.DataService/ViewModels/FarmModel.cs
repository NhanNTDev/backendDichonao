using DiCho.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class FarmViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? Active { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }
        public decimal? TotalStar { get; set; }
        public int? Feedbacks { get; set; }
    }
    
    public class FarmModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? Active { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }
        public decimal? TotalStar { get; set; }
        public int? Feedbacks { get; set; }
        public List<FarmOrderFeedbackModel> FarmOrders { get; set; }
    }

    public class FarmInCampaignModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? Active { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }
        public int? CountHarvestInCampaign { get; set; }
        public int? CountFarmOrder { get; set; }
    }
    
    public class FarmCreateInputModel
    {
        public string Name { get; set; }
        public IFormFile Avatar { get; set; }
        public IFormFileCollection Images { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string FarmerId { get; set; }
    }

    public class FarmCreateModel
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }
    }

    public class FarmUpdateInputModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public IFormFile Avatar { get; set; }
        public IFormFileCollection Images { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }

    public class FarmUpdateModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }
    }

    public class FarmMappingHarvestModel
    {
        [BindNever]
        public int? Id { get; set; }
        [BindNever]
        public string Name { get; set; }
        [BindNever]
        public string Address { get; set; }
        [BindNever]
        public int FarmZoneId { get; set; }
    }
    public class FarmNameModel
    {
        [BindNever]
        public int? Id { get; set; }
        [StringAttribute]
        public string Name { get; set; }
        [BindNever]
        public string Avatar { get; set; }
        [BindNever]
        public string Address { get; set; }
    }

    public class FarmApplyModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class FarmMappingCampaignModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? Active { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }
        public decimal TotalStar { get; set; }
        public virtual UserDataMapModel Farmer { get; set; }
        //public virtual FarmZone FarmZone { get; set; }
        //public virtual AspNetUsers Farmer { get; set; }
        public virtual ICollection<FarmOrderFeedbackModel> FarmOrders { get; set; }
        //public virtual ICollection<HarvestMappingCampaignModel> Harvests { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
    }

    public class FarmMapToFarmOrderDriverModel
    {
        public virtual UserDataMapModel Farmer { get; set; }
    }

    public class FarmOrderOfGroupModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }

    public class FarmMappingHarvestInCampaign
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string FarmerId { get; set; }
        public int? HarvestApplyRequest { get; set; }

        //public virtual ICollection<HarvestCampaignApplyRequest> HarvestCampaign { get; set; }
    }

    public class FarmItemCartViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<HarvestCampaignItemCartViewModel> HarvestInCampaigns { get; set; }
    }

    public class FarmDataMapModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }
        public virtual UserDataMapModel Farmer { get; set; }
        public virtual ICollection<HarvestDataMapModel> ProductHarvests { get; set; }
        public virtual ICollection<FarmOrderForManagerModel> FarmOrders { get; set; }
    }

    public class FarmMapFarmOrderModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }
        public virtual ICollection<FarmOrderMappingHarvestOrder> FarmOrders { get; set; }
    }

    public class FarmMapNotiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual UserDataMapModel Farmer { get; set; }
    }

    public class FarmSuggestFarmerApply
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public virtual ICollection<HarvestSuggestFarmerApplyData> ProductHarvests { get; set; }
    }
    
    public class FarmStatisticalModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int? CountFarmOrder { get; set; }
        public double Total { get; set; }
    }

    public class FarmMapToCampaginAndFarmOrderModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProductHarvestMappingCampaignModel> ProductHarvests { get; set; }
        public virtual ICollection<GetFarmOrderModel> FarmOrders { get; set; }
    }

    public class FarmMapDataToViewGroupFarmOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime? CreateAt { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }
        public virtual ICollection<FarmOrderMapToGroupModel> FarmOrders { get; set; }
    }
}
