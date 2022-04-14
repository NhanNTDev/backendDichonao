using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class CampaignDeliveryZoneModel
    {
        public int? Id { get; set; }
        public int? CampaignId { get; set; }
        public int DeliveryZoneId { get; set; }
        public string DeliveryZoneName { get; set; }
    }
    
    public class CampaignDeliveryZoneCreateModel
    {
        public int? DeliveryZoneId { get; set; }
    }
}
