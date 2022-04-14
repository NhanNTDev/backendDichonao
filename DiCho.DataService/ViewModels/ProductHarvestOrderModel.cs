using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class ProductHarvestOrderModel
    {
        public static string[] Fields = {
            "Id", "Unit", "Quantity", "Price", "FarmOrderId", "HarvestCampaignId"
        };
        public int? Id { get; set; }
        public int? Unit { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public int? FarmOrderId { get; set; }
        public int? HarvestCampaignId { get; set; }
    }
    public class HarvestOrderCreateModelInput
    {
        public int? HarvestCampaignId { get; set; }
    }
    public class ProductHarvestOrderCreateModel
    {
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int? HarvestCampaignId { get; set; }
    }

    public class ProductHarvestOrderMappingFarmOrderModel
    {
        [BindNever]
        public int? Id { get; set; }
        [BindNever]
        public string ProductName { get; set; }
        [BindNever]
        public int Quantity { get; set; }
        [BindNever]
        public double? Price { get; set; }
        [BindNever]
        public int? FarmOrderId { get; set; }
        [BindNever]
        public int HarvestCampaignId { get; set; }

    }

    public class ProductHarvestOrderDetailModel
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
        public int HarvestCampaignId { get; set; }
    }

    public class ProductHarvestOrderGroup
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int? Quantity { get; set; }
    }

    public class ProductHarvestOrderDataMapModel
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int? FarmOrderId { get; set; }
        public int? HarvestCampaignId { get; set; }

        public virtual HarvestCampaignDataMapModel HarvestCampaign { get; set; }
    }
    
    public class ProductHarvestOrderGroupFarmOrderModel
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int? FarmOrderId { get; set; }
        public int? HarvestCampaignId { get; set; }

        public virtual HarvestCampaignItemCartViewModel HarvestCampaign { get; set; }
    }

    public class ProductHarvestOrderOfDetailModel
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
        public int? HarvestCampaignId { get; set; }
        public double? Total { get; set; }
    }
}
