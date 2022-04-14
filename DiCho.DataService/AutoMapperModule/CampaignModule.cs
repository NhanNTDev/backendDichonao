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
    public static class CampaignModule
    {
        public static void ConfigCampaignModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Campaign, CampaignModel>();
            mc.CreateMap<CampaignModel, Campaign>();

            mc.CreateMap<Campaign, CampaignCreateModel>();
            mc.CreateMap<CampaignCreateModel, Campaign>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CampaignEnum.Chờthamgia));

            mc.CreateMap<Campaign, CampaignUpdateModel>();
            mc.CreateMap<CampaignUpdateModel, Campaign>();

            mc.CreateMap<Campaign, CampaignFarmZoneModel>();
            mc.CreateMap<CampaignFarmZoneModel, Campaign>();

            mc.CreateMap<Campaign, CampaignGetByIdModel>();
            mc.CreateMap<CampaignGetByIdModel, Campaign>();

            mc.CreateMap<Campaign, CampaignMapFarmOrder>();
            mc.CreateMap<CampaignMapFarmOrder, Campaign>();

            mc.CreateMap<Campaign, CampaignMapCampaignDeliveryZoneModel>();
            mc.CreateMap<CampaignMapCampaignDeliveryZoneModel, Campaign>();

            mc.CreateMap<Campaign, CampaignStatisticalOfAdmin>();
            mc.CreateMap<CampaignStatisticalOfAdmin, Campaign>();

            mc.CreateMap<Campaign, CampaignMappingModel>();
            mc.CreateMap<CampaignMappingModel, Campaign>();

            mc.CreateMap<Campaign, CampaignOfFarmerAppliedModel>();
            mc.CreateMap<CampaignOfFarmerAppliedModel, Campaign>();

            mc.CreateMap<Campaign, CampaignOfFarmerApplyModel>();
            mc.CreateMap<CampaignOfFarmerApplyModel, Campaign>();

            mc.CreateMap<Campaign, CampaignApplyRequestModel>();
            mc.CreateMap<CampaignApplyRequestModel, Campaign>();

            mc.CreateMap<Campaign, CampaignDetailApplyModel>();
            mc.CreateMap<CampaignDetailApplyModel, Campaign>();

            mc.CreateMap<Campaign, CampaignDetailApplieModel>();
            mc.CreateMap<CampaignDetailApplieModel, Campaign>();

            mc.CreateMap<Campaign, CampaignDataMapmodel>();
            mc.CreateMap<CampaignDataMapmodel, Campaign>();

            mc.CreateMap<Campaign, CampaignSearchModel>();
            mc.CreateMap<CampaignSearchModel, Campaign>();

            mc.CreateMap<Campaign, CampaignSuggestFarmerApply>();
            mc.CreateMap<CampaignSuggestFarmerApply, Campaign>();

        }
    }
}
