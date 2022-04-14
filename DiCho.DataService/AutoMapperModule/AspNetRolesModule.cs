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
    public static class AspNetRolesModule
    {
        public static void ConfigAspNetRolesModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<AspNetRoles, AspNetRolesModel>();
            mc.CreateMap<AspNetRolesModel, AspNetRoles>();

        }
    }
}
