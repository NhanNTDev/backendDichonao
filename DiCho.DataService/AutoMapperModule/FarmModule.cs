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
    public static class FarmModule
    {
        public static void ConfigFarmModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Farm, FarmModel>();
            mc.CreateMap<FarmModel, Farm>();

            mc.CreateMap<Farm, FarmCreateModel>();
            mc.CreateMap<FarmCreateModel, Farm>()
                .ForMember(des => des.Active, opt => opt.MapFrom(src => true))
                .ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now));

            mc.CreateMap<Farm, FarmUpdateModel>();
            mc.CreateMap<FarmUpdateModel, Farm>();

            mc.CreateMap<Farm, FarmViewModel>();
            mc.CreateMap<FarmViewModel, Farm>();

            mc.CreateMap<Farm, FarmMapDataToViewGroupFarmOrder>();
            mc.CreateMap<FarmMapDataToViewGroupFarmOrder, Farm>();

            mc.CreateMap<Farm, FarmMapNotiModel>();
            mc.CreateMap<FarmMapNotiModel, Farm>();

            mc.CreateMap<Farm, FarmMappingHarvestModel>();
            mc.CreateMap<FarmMappingHarvestModel, Farm>();

            mc.CreateMap<Farm, FarmMapToCampaginAndFarmOrderModel>();
            mc.CreateMap<FarmMapToCampaginAndFarmOrderModel, Farm>();

            mc.CreateMap<Farm, FarmMapFarmOrderModel>();
            mc.CreateMap<FarmMapFarmOrderModel, Farm>();

            mc.CreateMap<Farm, FarmStatisticalModel>();
            mc.CreateMap<FarmStatisticalModel, Farm>();

            mc.CreateMap<Farm, FarmMappingCampaignModel>();
            mc.CreateMap<FarmMappingCampaignModel, Farm>();

            mc.CreateMap<Farm, FarmNameModel>();
            mc.CreateMap<FarmNameModel, Farm>();

            mc.CreateMap<Farm, FarmMappingHarvestInCampaign>();
            mc.CreateMap<FarmMappingHarvestInCampaign, Farm>();

            mc.CreateMap<Farm, FarmMapToFarmOrderDriverModel>();
            mc.CreateMap<FarmMapToFarmOrderDriverModel, Farm>();

            mc.CreateMap<Farm, FarmApplyModel>();
            mc.CreateMap<FarmApplyModel, Farm>();

            mc.CreateMap<Farm, FarmItemCartViewModel>();
            mc.CreateMap<FarmItemCartViewModel, Farm>();

            mc.CreateMap<Farm, FarmInCampaignModel>();
            mc.CreateMap<FarmInCampaignModel, Farm>();

            mc.CreateMap<Farm, FarmDataMapModel>();
            mc.CreateMap<FarmDataMapModel, Farm>();

            mc.CreateMap<Farm, FarmSuggestFarmerApply>();
            mc.CreateMap<FarmSuggestFarmerApply, Farm>();

        }
    }
}
