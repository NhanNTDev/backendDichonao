using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class ProductHarvestOrder
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int? FarmOrderId { get; set; }
        public int? HarvestCampaignId { get; set; }

        public virtual FarmOrder FarmOrder { get; set; }
        public virtual ProductHarvestInCampaign HarvestCampaign { get; set; }
    }
}
