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
    public static class CampaignDeliveryZoneModule
    {
        public static void ConfigCampaignDeliveryZoneModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<CampaignDeliveryZone, CampaignDeliveryZoneModel>();
            mc.CreateMap<CampaignDeliveryZoneModel, CampaignDeliveryZone>();
            
            mc.CreateMap<CampaignDeliveryZone, CampaignDeliveryZoneCreateModel>();
            mc.CreateMap<CampaignDeliveryZoneCreateModel, CampaignDeliveryZone>();


        }
    }
}
