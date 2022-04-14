using DiCho.DataService.Models;
using DiCho.DataService.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiCho.DataService.DI
{
    public static class ServiceDI
    {
        public static void ConfigServiceDI(this IServiceCollection services)
        {
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<AspNetUserRoles>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<ITradeZoneMapService, TradeZoneMapService>();
            services.AddScoped<IItemCartService, ItemCartService>();
            services.AddScoped<IZaloService, ZaloService>();
            services.AddScoped<IVehicleRoutingService, VehicleRoutingService>();
            //services.AddHostedService<WorkerService>();
        }
    }
}
