using DiCho.Core.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class WareHouseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string WarehouseManagerId { get; set; }
        public string WarehouseManagerName { get; set; }
        public virtual ICollection<WareHouseZoneModel> WareHouseZones { get; set; }
    }

    public class WareHouseDataMapModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? Active { get; set; }
        public string WarehouseManagerId { get; set; }

        public virtual ICollection<WareHouseZoneDataMapModel> WareHouseZones { get; set; }
    }
    
    public class WareHouseCreateModel
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<WareHouseZoneCreateModel> WareHouseZones { get; set; }
    }

    public class WareHouseUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string WarehouseManagerId { get; set; }
        public virtual ICollection<WareHouseZoneCreateModel> WareHouseZones { get; set; }
    }
}
