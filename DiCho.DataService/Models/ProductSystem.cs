using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class ProductSystem
    {
        public ProductSystem()
        {
            ProductHarvests = new HashSet<ProductHarvest>();
            ProductSalesCampaigns = new HashSet<ProductSalesCampaign>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string Unit { get; set; }
        public string Province { get; set; }
        public bool Active { get; set; }
        public int? ProductCategoryId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<ProductHarvest> ProductHarvests { get; set; }
        public virtual ICollection<ProductSalesCampaign> ProductSalesCampaigns { get; set; }
    }
}
