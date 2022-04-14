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
    public static class AspNetUsersModule
    {
        public static void ConfigAspNetUsersModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<AspNetUsers, AspNetUsersModel>();
            mc.CreateMap<AspNetUsersModel, AspNetUsers>();
            
            mc.CreateMap<AspNetUsers, UserDataMapModel>();
            mc.CreateMap<UserDataMapModel, AspNetUsers>();
            
            mc.CreateMap<AspNetUsers, AspNetUsersCreateModel>();
            mc.CreateMap<AspNetUsersCreateModel, AspNetUsers>();
            
            mc.CreateMap<AspNetUsers, AspNetUsersUpdateModel>();
            mc.CreateMap<AspNetUsersUpdateModel, AspNetUsers>();
            
            mc.CreateMap<AspNetUsers, AspNetUsersUpdateImageModel>();
            mc.CreateMap<AspNetUsersUpdateImageModel, AspNetUsers>();
            
            mc.CreateMap<AspNetUsers, UserModel>();
            mc.CreateMap<UserModel, AspNetUsers>();
            
            mc.CreateMap<AspNetUsers, UserMappingModel>();
            mc.CreateMap<UserMappingModel, AspNetUsers>();
            
            mc.CreateMap<AspNetUsers, UserDataMapModel>();
            mc.CreateMap<UserDataMapModel, AspNetUsers>();
            
            mc.CreateMap<AspNetUsers, UserDriverModel>();
            mc.CreateMap<UserDriverModel, AspNetUsers>();
            
            mc.CreateMap<AspNetUsers, CustomerOrder>();
            mc.CreateMap<CustomerOrder, AspNetUsers>();
            
        }
    }
}
