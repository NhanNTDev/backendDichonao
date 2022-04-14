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
    public static class FarmOrderModule
    {
        public static void ConfigFarmOrderModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<FarmOrder, FarmOrderModel>();
            mc.CreateMap<FarmOrderModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderDetailDataModel>();
            mc.CreateMap<FarmOrderDetailDataModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderMappingHarvestOrder>();
            mc.CreateMap<FarmOrderMappingHarvestOrder, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderRevenuseModel>();
            mc.CreateMap<FarmOrderRevenuseModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderFeedbackViewModel>();
            mc.CreateMap<FarmOrderFeedbackViewModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderFeedbackModel>();
            mc.CreateMap<FarmOrderFeedbackModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderMapToGroupModel>();
            mc.CreateMap<FarmOrderMapToGroupModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderDetailModel>();
            mc.CreateMap<FarmOrderDetailModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderUpdateModel>();
            mc.CreateMap<FarmOrderUpdateModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderForManagerModel>();
            mc.CreateMap<FarmOrderForManagerModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderGroupModel>();
            mc.CreateMap<FarmOrderGroupModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderGroup>();
            mc.CreateMap<FarmOrderGroup, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderOfGroupModel>();
            mc.CreateMap<FarmOrderOfGroupModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderGroupFarm>();
            mc.CreateMap<FarmOrderGroupFarm, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderUpdateCancelModel>();
            mc.CreateMap<FarmOrderUpdateCancelModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderUpdateDriverModel>();
            mc.CreateMap<FarmOrderUpdateDriverModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderDataMapToHarvestCampaignModel>();
            mc.CreateMap<FarmOrderDataMapToHarvestCampaignModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderDetail>();
            mc.CreateMap<FarmOrderDetail, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderMapToFarmForDriverModel>();
            mc.CreateMap<FarmOrderMapToFarmForDriverModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderMapOrderDataModel>();
            mc.CreateMap<FarmOrderMapOrderDataModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderForDeliveryDriverModel>();
            mc.CreateMap<FarmOrderForDeliveryDriverModel, FarmOrder>();

            mc.CreateMap<FarmOrder, GetFarmOrderModel>();
            mc.CreateMap<GetFarmOrderModel, FarmOrder>();

            mc.CreateMap<FarmOrder, FarmOrderCreateModel>();
            mc.CreateMap<FarmOrderCreateModel, FarmOrder>()
                .ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)FarmOrderEnum.Chờxácnhận));

        }
    }
}
