using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class WareHouseZoneModel
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string WareHouseZoneName { get; set; }
    }

    public class WareHouseZoneCreateModel
    {
        public int ZoneId { get; set; }
    }

    public class WareHouseZoneDataMapModel
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string WareHouseZoneName { get; set; }
        public int? WareHouseId { get; set; }
    }
}
