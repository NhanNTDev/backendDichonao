using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class Order
    {
        public Order()
        {
            FarmOrders = new HashSet<FarmOrder>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public double ShipCost { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string Note { get; set; }
        public int? CampaignId { get; set; }
        public string CustomerId { get; set; }
        public int? DeliveryZoneId { get; set; }
        public int? ShipmentId { get; set; }
        public string DriverId { get; set; }
        public string DeliveryCode { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual AspNetUsers Customer { get; set; }
        public virtual ICollection<FarmOrder> FarmOrders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
