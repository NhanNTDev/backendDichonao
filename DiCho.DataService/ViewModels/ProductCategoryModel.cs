using DiCho.Core.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class ProductCategoryModel
    {
        public static string[] Fields = {
            "Id", "Name", "Image", "Description", "ProductInventory", "Active"
        };
        public int? Id { get; set; }
        [StringAttribute]
        public string Name { get; set; }
        [BindNever]
        public string Description { get; set; }
        [BindNever]
        public string Image { get; set; }
        [BindNever]
        public int? ProductInventory { get; set; }
        public bool? Active { get; set; }

    }
    public class ProductCategoryCreateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
    public class ProductCategoryUpdateModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
    public class ProductCategoryMappingModel
    {
        [BindNever]
        public int? Id { get; set; }
        [BindNever]
        public string Name { get; set; }
        [BindNever]
        public string Image { get; set; }
    }
}
