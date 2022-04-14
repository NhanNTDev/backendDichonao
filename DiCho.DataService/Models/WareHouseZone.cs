using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class WareHouseZone
    {
        public int Id { get; set; }
        public int? ZoneId { get; set; }
        public string WareHouseZoneName { get; set; }
        public int? WareHouseId { get; set; }

        public virtual WareHouse WareHouse { get; set; }
    }
}
