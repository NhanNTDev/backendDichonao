using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class ShipmentDestination
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int? ShipmentId { get; set; }

        public virtual Shipment Shipment { get; set; }
    }
}
