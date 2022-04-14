using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class WareHouse
    {
        public WareHouse()
        {
            WareHouseZones = new HashSet<WareHouseZone>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }
        public string WarehouseManagerId { get; set; }

        public virtual ICollection<WareHouseZone> WareHouseZones { get; set; }
    }
}
