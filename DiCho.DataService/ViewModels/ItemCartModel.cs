using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class ItemCartModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string CustomerId { get; set; }
        public int HarvestCampaignId { get; set; }
    }
    public class ItemCartCreateInputModel
    {
        public int Quantity { get; set; }
        public int HarvestCampaignId { get; set; }
        public string CustomerId { get; set; }
    }
    
    public class ItemCartCreateModel
    {
        public int? Id { get; set; }
        public int Quantity { get; set; }
        public int? HarvestCampaignId { get; set; }
        public string CustomerId { get; set; }
    }
    public class ItemCartUpdateModel
    {
        public int? Id { get; set; }
        public int? Quantity { get; set; }
    }

    public class ItemCartMappingOrderModel
    {
        public int? Id { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public string CustomerId { get; set; }
        public int? HarvestCampaignId { get; set; }
        public virtual HarvestCampaignMappingModel HarvestCampaign { get; set; }
    }

    public class ItemCartViewModel
    {
        public bool? Checked { get; set; }
        public int? CampaignId { get; set; }
        public string CampaignName { get; set; }
        public DateTime? ExpectedDeliveryTime { get; set; }
        public List<FarmItemCartViewModel> Farms { get; set; }
    }
}
