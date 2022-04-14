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
    public static class ProductSystemModule
    {
        public static void ConfigProductSystemModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<ProductSystem, ProductSystemModel>();
            mc.CreateMap<ProductSystemModel, ProductSystem>();
            
            mc.CreateMap<ProductSystem, ProductSystemCreateModel>();
            mc.CreateMap<ProductSystemCreateModel, ProductSystem>()
                .ForMember(des => des.Active, opt => opt.MapFrom(src => 1));

            mc.CreateMap<ProductSystem, ProductSystemUpdateModel>();
            mc.CreateMap<ProductSystemUpdateModel, ProductSystem>();

            mc.CreateMap<ProductSystem, ProductSystemCampagin>();
            mc.CreateMap<ProductSystemCampagin, ProductSystem>();

            mc.CreateMap<ProductSystem, ProductSystemMappingHarvestModel>();
            mc.CreateMap<ProductSystemMappingHarvestModel, ProductSystem>();

            mc.CreateMap<ProductSystem, ProductSystemDataMapModel>();
            mc.CreateMap<ProductSystemDataMapModel, ProductSystem>();

            mc.CreateMap<ProductSystem, ProductSystemHarvestDataModel>();
            mc.CreateMap<ProductSystemHarvestDataModel, ProductSystem>();

        }
    }
}
