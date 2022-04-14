using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class ProductHarvestInCampaign
    {
        public ProductHarvestInCampaign()
        {
            ProductHarvestOrders = new HashSet<ProductHarvestOrder>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int ValueChangeOfUnit { get; set; }
        public int Inventory { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int? Status { get; set; }
        public string Note { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? HarvestId { get; set; }
        public int? CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual ProductHarvest Harvest { get; set; }
        public virtual ICollection<ProductHarvestOrder> ProductHarvestOrders { get; set; }
    }
}
