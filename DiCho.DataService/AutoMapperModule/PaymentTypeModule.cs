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
    public static class PaymentTypeModule
    {
        public static void ConfigPaymentTypeModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<PaymentType, PaymentTypeModel>();
            mc.CreateMap<PaymentTypeModel, PaymentType>();


        }
    }
}
