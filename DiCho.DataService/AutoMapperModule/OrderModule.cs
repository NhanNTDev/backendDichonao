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
    public static class OrderModule
    {
        public static void ConfigOrderModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Order, OrderModel>();
            mc.CreateMap<OrderModel, Order>();
            
            mc.CreateMap<Order, OrderMappingFarmOrderModel>();
            mc.CreateMap<OrderMappingFarmOrderModel, Order>();
            
            mc.CreateMap<Order, OrderUpdateModel>();
            mc.CreateMap<OrderUpdateModel, Order>();
            
            mc.CreateMap<Order, OrderOfCustomerModel>();
            mc.CreateMap<OrderOfCustomerModel, Order>();
            
            mc.CreateMap<Order, OrderPaymentModel>();
            mc.CreateMap<OrderPaymentModel, Order>();
            
            mc.CreateMap<Order, OrderMapDataPaymentType>();
            mc.CreateMap<OrderMapDataPaymentType, Order>();
            
            mc.CreateMap<Order, GetOrderDetailDataModel>();
            mc.CreateMap<GetOrderDetailDataModel, Order>();
            
            mc.CreateMap<Order, OrderMapCampaignModel>();
            mc.CreateMap<OrderMapCampaignModel, Order>();
            
            mc.CreateMap<Order, OrderFarmOrderModel>();
            mc.CreateMap<OrderFarmOrderModel, Order>();
            
            mc.CreateMap<Order, OrderDetailModel>();
            mc.CreateMap<OrderDetailModel, Order>();
            
            mc.CreateMap<Order, OrderDataMapModel>();
            mc.CreateMap<OrderDataMapModel, Order>();
            
            mc.CreateMap<Order, OrderGroupModel>();
            mc.CreateMap<OrderGroupModel, Order>();
            
            mc.CreateMap<Order, OrderModelForDriver>();
            mc.CreateMap<OrderModelForDriver, Order>();
            
            mc.CreateMap<Order, OrderForManagerModel>();
            mc.CreateMap<OrderForManagerModel, Order>();
            
            mc.CreateMap<Order, OrderMapCustomerDataModel>();
            mc.CreateMap<OrderMapCustomerDataModel, Order>();
            
            mc.CreateMap<Order, OrderForDriverModel>();
            mc.CreateMap<OrderForDriverModel, Order>();
            
            mc.CreateMap<Order, UpdateDriverForOrderByWarehouse>();
            mc.CreateMap<UpdateDriverForOrderByWarehouse, Order>();
            
            mc.CreateMap<Order, OrderForWarehouseManagerModel>();
            mc.CreateMap<OrderForWarehouseManagerModel, Order>();
            
            mc.CreateMap<Order, OrderDataMapHarvestCampaignModel>();
            mc.CreateMap<OrderDataMapHarvestCampaignModel, Order>();

            mc.CreateMap<Order, OrderCreateModel>();
            mc.CreateMap<OrderCreateModel, Order>()
                .ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)OrderEnum.Chờxácnhận));
        }
    }
}
