using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? Amount { get; set; }
        public int? Status { get; set; }
        public int? OrderId { get; set; }
        public int? PaymentTypeId { get; set; }

        public virtual Order Order { get; set; }
        public virtual PaymentType PaymentType { get; set; }
    }
}
