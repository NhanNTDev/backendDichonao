using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class FarmOrder
    {
        public FarmOrder()
        {
            ProductHarvestOrders = new HashSet<ProductHarvestOrder>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public double Star { get; set; }
        public string Content { get; set; }
        public DateTime? FeedBackCreateAt { get; set; }
        public string Note { get; set; }
        public int? FarmId { get; set; }
        public int? OrderId { get; set; }
        public string DriverId { get; set; }
        public string CollectionCode { get; set; }

        public virtual Farm Farm { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<ProductHarvestOrder> ProductHarvestOrders { get; set; }
    }
}
