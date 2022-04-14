using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class ProductHarvest
    {
        public ProductHarvest()
        {
            ProductHarvestInCampaigns = new HashSet<ProductHarvestInCampaign>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public DateTime? EstimatedTime { get; set; }
        public string EstimatedProduction { get; set; }
        public int? ActualProduction { get; set; }
        public int InventoryTotal { get; set; }
        public DateTime? StartAt { get; set; }
        public bool Active { get; set; }
        public int? FarmId { get; set; }
        public int? ProductSystemId { get; set; }

        public virtual Farm Farm { get; set; }
        public virtual ProductSystem ProductSystem { get; set; }
        public virtual ICollection<ProductHarvestInCampaign> ProductHarvestInCampaigns { get; set; }
    }
}
