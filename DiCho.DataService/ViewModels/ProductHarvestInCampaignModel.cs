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
    public class ProductHarvestInCampaignModel
    {
        public static string[] Fields = {
            "Id", "ProductName", "Inventory", "Price", "Status", "HarvestId", "CampaignId", "ValueChangeOfUnit", "Unit", "Quantity",
            "Campaign", "Harvest"
        };
        public int? Id { get; set; }
        [StringAttribute]
        public string ProductName { get; set; }
        [BindNever]
        public int? Inventory { get; set; }
        [BindNever]
        public int? Quantity { get; set; }
        [BindNever]
        public double? Price { get; set; }
        [BindNever]
        public string UnitOfSystem { get; set; }
        [BindNever]
        public string Unit { get; set; }
        [BindNever]
        public int? ValueChangeOfUnit { get; set; }
        [BindNever]
        public string Status { get; set; }
        [BindNever]
        public int? HarvestId { get; set; }
        public int? CampaignId { get; set; }

        [BindNever]
        public virtual CampaignMappingModel Campaign { get; set; }
        [BindNever]
        public virtual HarvestMappingModel Harvest { get; set; }
    }
    public class ProductHarvestCampaignCreateModel
    {
        public int Inventory { get; set; }
        public double? Price { get; set; }
        public string Unit { get; set; }
        public int ValueChangeOfUnit { get; set; }
        public int? HarvestId { get; set; }
        public int? CampaignId { get; set; }
    }
    public class ProductHarvestCampaignUpdateModel
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int? Inventory { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public int? ValueChangeOfUnit { get; set; }
        public int? HarvestId { get; set; }
        public int? CampaignId { get; set; }
    }
    
    public class HarvestCampaignUpdateStatusModel
    {
        public int? Id { get; set; }
    }

    public class HarvestCampaignMappingModel
    {
        [BindNever]
        public int? Id { get; set; }
        [BindNever]
        public string ProductName { get; set; }
        [BindNever]
        public int? Inventory { get; set; }
        [BindNever]
        public int? Quantity { get; set; }
        [BindNever]
        public double Price { get; set; }
        [BindNever]
        public string Unit { get; set; }
        [BindNever]
        public int? ValueChangeOfUnit { get; set; }
        [BindNever]
        public int? HarvestId { get; set; }

        public virtual HarvestMappingModel Harvest { get; set; }
        [BindNever]
        public int? CampaignId { get; set; }

        public virtual CampaignMappingModel Campaign { get; set; }
    }

    public class HarvestCampaignApplyRequest
    {
        public int? Key { get; set; }
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string UnitOfSystem { get; set; }
        public int? ValueChangeOfUnit { get; set; }
        public int? Inventory { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public string Status { get; set; }
        public int? HarvestId { get; set; }

        public virtual HarvestMappingCartModel Harvest { get; set; }
    }

    public class ProductHarvestCampaignDetailModel
    {
        public int? Id { get; set; }
        public int? FarmId { get; set; }
        public string FarmName { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string HarvestName { get; set; }
        public string ProductName { get; set; }
        public string ProductNameSystem { get; set; }
        public string ProductCategoryName { get; set; }
        public double? Price { get; set; }
        public string Unit { get; set; }
        public int? ValueChangeOfUnit { get; set; }
        public string SystemUnit { get; set; }
        public int? Inventory { get; set; }
        public int? Quantity { get; set; }
        public string Status { get; set; }
        public int? HarvestId { get; set; }
        public string HarvestDescription { get; set; }
        public CampaignMappingModel Campaign { get; set; }
    }

    public class ProductHarvestInCampaignDetailByIdModel
    {
        public int? Id { get; set; }
        public string FarmName { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string HarvestName { get; set; }
        public string ProductName { get; set; }
        public string ProductNameSystem { get; set; }
        public string ProductCategoryName { get; set; }
        public double? MinPrice { get; set; }
        public double MaxPrice { get;set; }
        public double? Price { get; set; }
        public string Unit { get; set; }
        public int? ValueChangeOfUnit { get; set; }
        public string SystemUnit { get; set; }
        public int? Inventory { get; set; }
        public int? Quantity { get; set; }
        public string Status { get; set; }
        public int? HarvestId { get; set; }
        public string HarvestDescription { get; set; }
        public CampaignMappingModel Campaign { get; set; }
    }

    public class ProductHarvestCampaignOfFarmModel
    {
        public int? Id { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string HarvestName { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public double? Price { get; set; }
        public string Unit { get; set; }
        public int? ValueChangeOfUnit { get; set; }
        public int? Inventory { get; set; }
        public int? Quantity { get; set; }
        public string Status { get; set; }
        public int? HarvestId { get; set; }
    }

    public class ProductHarvestCampaignSearchModel
    {
        public static string[] Fields = {
            "Id", "ProductName", "Image1"
        };
        [BindNever]
        public int? Id { get; set; }
        [BindNever]
        public string Image1 { get; set; }
        [StringAttribute]
        public string ProductName { get; set; }
        [BindNever]
        public string HarvestName { get; set; }
        [BindNever]
        public string FarmName { get; set; }
    }

    public class HarvestCampaignItemCartViewModel
    {
        public bool? Checked { get; set; }
        public int? ItemCartId { get; set; }
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int? MaxQuantity { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int ValueChangeOfUnit { get; set; }
        public string Unit { get; set; }
        public double? Total { get; set; }
    }

    public class HarvestCampaignMapItemCartModel
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int ValueChangeOfUnit { get; set; }
        public int? Inventory { get; set; }
        public int? Quantity { get; set; }
        public double Price { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? HarvestId { get; set; }
        public int? CampaignId { get; set; }
        public double? Total { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual HarvestMapItemCartModel Harvest { get; set; }
    }

    public class HarvestCampaignDataMapModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int ValueChangeOfUnit { get; set; }
        public int Inventory { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? HarvestId { get; set; }
        public int? CampaignId { get; set; }
        public virtual HarvestDataMapModel Harvest { get; set; }
    }

    public class HarvestCampaignMapNotiModel
    {
        public string ProductName { get; set; }
        public virtual CampaignDataMapmodel Campaign { get; set; }
        public virtual HarvestMapNotiModel Harvest { get; set; }
    }

    public class ProductHarvestInCampaginOriginModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int ValueChangeOfUnit { get; set; }
        public int Inventory { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? HarvestId { get; set; }

        public virtual CampaignMappingModel Campaign { get; set; }
        public virtual ProductHarvestOriginModel Harvest { get; set; }
    }
}
