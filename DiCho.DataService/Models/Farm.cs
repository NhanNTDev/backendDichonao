using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class Farm
    {
        public Farm()
        {
            FarmOrders = new HashSet<FarmOrder>();
            ProductHarvests = new HashSet<ProductHarvest>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public string FarmerId { get; set; }
        public int? FarmZoneId { get; set; }
        public string FarmZoneName { get; set; }

        public virtual AspNetUsers Farmer { get; set; }
        public virtual ICollection<FarmOrder> FarmOrders { get; set; }
        public virtual ICollection<ProductHarvest> ProductHarvests { get; set; }
    }
}
