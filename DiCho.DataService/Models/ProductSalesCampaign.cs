using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class ProductSalesCampaign
    {
        public int Id { get; set; }
        public int? Capacity { get; set; }
        public int? ProductSystemId { get; set; }
        public int? CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual ProductSystem ProductSystem { get; set; }
    }
}
