using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            ProductSystems = new HashSet<ProductSystem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<ProductSystem> ProductSystems { get; set; }
    }
}
