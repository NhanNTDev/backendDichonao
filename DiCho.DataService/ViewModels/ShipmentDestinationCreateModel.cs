using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class ShipmentDestinationCreateModel
    {
        public string Address { get; set; }
        public int ShipmentId { get; set; }
    }

    public class ShipmentDestinationViewModel
    {
        public int Id { get; set; }
        public string WarehouseTo { get; set; }
        public string Address { get; set; }
        public List<OrderModelForDriver> Orders { get; set; }
    }
}
