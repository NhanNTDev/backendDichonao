using AutoMapper;
using DiCho.DataService.Models;
using DiCho.DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.AutoMapperModule
{
    public static class ProductHarvestModule
    {
        public static void ConfigProductHarvestModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ProductHarvest, ProductHarvestModel>();
            mc.CreateMap<ProductHarvestModel, ProductHarvest>();

            mc.CreateMap<ProductHarvest, ProductHarvestCreateModel>();
            mc.CreateMap<ProductHarvestCreateModel, ProductHarvest>()
               .ForMember(des => des.Active, opt => opt.MapFrom(src => 1));

            mc.CreateMap<ProductHarvest, ProductHarvestUpdateModel>();
            mc.CreateMap<ProductHarvestUpdateModel, ProductHarvest>();

            mc.CreateMap<ProductHarvest, ProductHarvestDetailViewModel>();
            mc.CreateMap<ProductHarvestDetailViewModel, ProductHarvest>();

            mc.CreateMap<ProductHarvest, ProductHarvestOriginModel>();
            mc.CreateMap<ProductHarvestOriginModel, ProductHarvest>();

            mc.CreateMap<ProductHarvest, HarvestMapNotiModel>();
            mc.CreateMap<HarvestMapNotiModel, ProductHarvest>();

            mc.CreateMap<ProductHarvest, HarvestMappingHarvestCampaignModel>();
            mc.CreateMap<HarvestMappingHarvestCampaignModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestMappingModel>();
            mc.CreateMap<HarvestMappingModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, ProductHarvestMappingCampaignModel>();
            mc.CreateMap<ProductHarvestMappingCampaignModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestMappingCartModel>();
            mc.CreateMap<HarvestMappingCartModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestApplyRequestModel>();
            mc.CreateMap<HarvestApplyRequestModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestApplyModel>();
            mc.CreateMap<HarvestApplyModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestDetailModel>();
            mc.CreateMap<HarvestDetailModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestMapItemCartModel>();
            mc.CreateMap<HarvestMapItemCartModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestDataMapModel>();
            mc.CreateMap<HarvestDataMapModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestSuggestFarmerApplyData>();
            mc.CreateMap<HarvestSuggestFarmerApplyData, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, ProductHarvestSearchNameModel>();
            mc.CreateMap<ProductHarvestSearchNameModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestDataMapProductSystemModel>();
            mc.CreateMap<HarvestDataMapProductSystemModel, ProductHarvest>();
            
            mc.CreateMap<ProductHarvest, HarvestSuggestFarmerApplyModel>();
            mc.CreateMap<HarvestSuggestFarmerApplyModel, ProductHarvest>();

        }
    }
}
