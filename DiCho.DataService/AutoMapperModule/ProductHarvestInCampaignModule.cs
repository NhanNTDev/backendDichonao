using AutoMapper;
using DiCho.DataService.Enums;
using DiCho.DataService.Models;
using DiCho.DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.AutoMapperModule
{
    public static class ProductHarvestInCampaignModule
    {
        public static void ConfigProductHarvestInCampaignModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ProductHarvestInCampaign, ProductHarvestInCampaignModel>();
            mc.CreateMap<ProductHarvestInCampaignModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, ProductHarvestCampaignCreateModel>();
            mc.CreateMap<ProductHarvestCampaignCreateModel, ProductHarvestInCampaign>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)HarvestCampaignEnum.Chờxácnhận))
                .ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now));
            
            mc.CreateMap<ProductHarvestInCampaign, ProductHarvestCampaignUpdateModel>();
            mc.CreateMap<ProductHarvestCampaignUpdateModel, ProductHarvestInCampaign>();

            mc.CreateMap<ProductHarvestInCampaign, HarvestCampaignMapNotiModel>();
            mc.CreateMap<HarvestCampaignMapNotiModel, ProductHarvestInCampaign>();

            mc.CreateMap<ProductHarvestInCampaign, ProductHarvestInCampaginOriginModel>();
            mc.CreateMap<ProductHarvestInCampaginOriginModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, HarvestCampaignMappingModel>();
            mc.CreateMap<HarvestCampaignMappingModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, HarvestCampaignApplyRequest>();
            mc.CreateMap<HarvestCampaignApplyRequest, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, HarvestCampaignUpdateStatusModel>();
            mc.CreateMap<HarvestCampaignUpdateStatusModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, ProductHarvestCampaignDetailModel>();
            mc.CreateMap<ProductHarvestCampaignDetailModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, ProductHarvestCampaignOfFarmModel>();
            mc.CreateMap<ProductHarvestCampaignOfFarmModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, HarvestCampaignItemCartViewModel>();
            mc.CreateMap<HarvestCampaignItemCartViewModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, HarvestCampaignMapItemCartModel>();
            mc.CreateMap<HarvestCampaignMapItemCartModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, ProductHarvestInCampaignDetailByIdModel>();
            mc.CreateMap<ProductHarvestInCampaignDetailByIdModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, ProductHarvestCampaignSearchModel>();
            mc.CreateMap<ProductHarvestCampaignSearchModel, ProductHarvestInCampaign>();
            
            mc.CreateMap<ProductHarvestInCampaign, HarvestCampaignDataMapModel>();
            mc.CreateMap<HarvestCampaignDataMapModel, ProductHarvestInCampaign>();

        }
    }
}
