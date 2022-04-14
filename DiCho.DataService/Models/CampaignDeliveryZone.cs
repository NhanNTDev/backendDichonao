using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class CampaignDeliveryZone
    {
        public int Id { get; set; }
        public int? CampaignId { get; set; }
        public int? DeliveryZoneId { get; set; }
        public string DeliveryZoneName { get; set; }

        public virtual Campaign Campaign { get; set; }
    }
}
