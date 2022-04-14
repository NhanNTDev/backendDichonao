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
    public static class WareHouseModule
    {
        public static void ConfigWareHouseModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<WareHouse, WareHouseModel>();
            mc.CreateMap<WareHouseModel, WareHouse>();

            mc.CreateMap<WareHouse, WareHouseDataMapModel>();
            mc.CreateMap<WareHouseDataMapModel, WareHouse>();

            mc.CreateMap<WareHouse, WareHouseUpdateModel>();
            mc.CreateMap<WareHouseUpdateModel, WareHouse>();

            mc.CreateMap<WareHouse, WareHouseCreateModel>();
            mc.CreateMap<WareHouseCreateModel, WareHouse>()
                .ForMember(des => des.Active, opt => opt.MapFrom(src => true));

        }
    }
}
