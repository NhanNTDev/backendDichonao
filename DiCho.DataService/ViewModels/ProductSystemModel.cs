using DiCho.Core.Attributes;
using DiCho.DataService.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class ProductSystemModel
    {
        public static string[] Fields = {
            "Id", "Name", "MinPrice", "MaxPrice", "Unit", "Province", "Active", "ProductCategoryId"
        };
        public int? Id { get; set; }
        [StringAttribute]
        public string Name { get; set; }
        [BindNever]
        public double? MinPrice { get; set; }
        [BindNever]
        public double? MaxPrice { get; set; }
        [BindNever]
        public string Unit { get; set; }
        [StringAttribute]
        public string Province { get; set; }
        public bool? Active { get; set; }
        public int? ProductCategoryId { get; set; }
    }

    public class ProductSystemCreateModel
    {
        public string Name { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string Unit { get; set; }
        public string Province { get; set; }
        public int? ProductCategoryId { get; set; }
    }

    public class ProductSystemUpdateModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string Unit { get; set; }
        public string Province { get; set; }
        public int? ProductCategoryId { get; set; }
    }

    public class ProductSystemMappingHarvestModel
    {
        [BindNever]
        public int? Id { get; set; }
        [BindNever]
        public string Name { get; set; }
        [BindNever]
        public string Unit { get; set; }
        [BindNever]
        public double? MinPrice { get; set; }
        [BindNever]
        public double MaxPrice { get; set; }
        [BindNever]
        public string Province { get; set; }
        [BindNever]
        public int? ProductCategoryId { get; set; }
        [BindNever]

        public virtual ProductCategoryMappingModel ProductCategory { get; set; }
    }

    public class ProductSystemDataMapModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string Unit { get; set; }
        public string Province { get; set; }
        public bool Active { get; set; }
        public int? ProductCategoryId { get; set; }

        public virtual ProductCategoryMappingModel ProductCategory { get; set; }
        public virtual ICollection<ProductSalesCampaignModel> ProductSalesCampaigns { get; set; }
    }

    public class ProductSystemHarvestDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string Unit { get; set; }
        public string Province { get; set; }
        public bool Active { get; set; }
        public int? ProductCategoryId { get; set; }
        public virtual ICollection<HarvestDataMapModel> ProductHarvests { get; set; }
    }

    public class ProductSystemCampagin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string Unit { get; set; }
    }
}
