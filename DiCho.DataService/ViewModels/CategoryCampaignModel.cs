using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class CategoryCampaignModel
    {
        public static string[] Fields = {
            "Id", "ProductCategoryId", "CampaignId", "ProductCategory", "Campaign"
        };
        public int? Id { get; set; }
        public int? ProductCategoryId { get; set; }
        public virtual ProductCategoryMappingModel ProductCategory { get; set; }
        public int? CampaignId { get; set; }
        public virtual CampaignMappingModel Campaign { get; set; }
    }
    public class CategoryCampaignCreateModel
    {
        public int? ProductCategoryId { get; set; }
        public int? CampaignId { get; set; }
    }
    public class CategoryCampaignUpdateModel
    {
        public int? Id { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? CampaignId { get; set; }
    }
}
