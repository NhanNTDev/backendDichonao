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
    public static class ShipmentModule
    {
        public static void ConfigShipmentModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Shipment, ShipmentModel>();
            mc.CreateMap<ShipmentModel, Shipment>();

            mc.CreateMap<Shipment, ShipmentForDriverModel>();
            mc.CreateMap<ShipmentForDriverModel, Shipment>();

            mc.CreateMap<Shipment, ShipmentForWareHouseManagerModel>();
            mc.CreateMap<ShipmentForWareHouseManagerModel, Shipment>();

            mc.CreateMap<Shipment, ShipmentUpdateModel>();
            mc.CreateMap<ShipmentUpdateModel, Shipment>();

            mc.CreateMap<Shipment, ShipmentDetailForWareHouseManagerModel>();
            mc.CreateMap<ShipmentDetailForWareHouseManagerModel, Shipment>();
            
            mc.CreateMap<ShipmentDestination, ShipmentDestinationCreateModel>();
            mc.CreateMap<ShipmentDestinationCreateModel, ShipmentDestination>();
            
            mc.CreateMap<ShipmentDestination, ShipmentDestinationViewModel>();
            mc.CreateMap<ShipmentDestinationViewModel, ShipmentDestination>();

            mc.CreateMap<Shipment, ShipmentCreateModel>();
            mc.CreateMap<ShipmentCreateModel, Shipment>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => ShipmentEnum.Đangvậnchuyển))
                .ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
