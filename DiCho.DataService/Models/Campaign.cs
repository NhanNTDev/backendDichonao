using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class Campaign
    {
        public Campaign()
        {
            CampaignDeliveryZones = new HashSet<CampaignDeliveryZone>();
            Orders = new HashSet<Order>();
            ProductHarvestInCampaigns = new HashSet<ProductHarvestInCampaign>();
            ProductSalesCampaigns = new HashSet<ProductSalesCampaign>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? StartRecruitmentAt { get; set; }
        public DateTime? EndRecruitmentAt { get; set; }
        public int? Status { get; set; }
        public int? CampaignZoneId { get; set; }
        public string CampaignZoneName { get; set; }
        public string Note { get; set; }

        public virtual ICollection<CampaignDeliveryZone> CampaignDeliveryZones { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductHarvestInCampaign> ProductHarvestInCampaigns { get; set; }
        public virtual ICollection<ProductSalesCampaign> ProductSalesCampaigns { get; set; }
    }
}
