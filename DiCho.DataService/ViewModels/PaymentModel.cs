using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? Amount { get; set; }
        public string Status { get; set; }
        public int? OrderId { get; set; }
        public int? PaymentTypeId { get; set; }

        public virtual PaymentTypeModel PaymentType { get; set; }
    }

    public class PaymentCreateInputModel
    {
        public int? PaymentTypeId { get; set; }
    }
    public class PaymentCreateModel
    {
        public string Code { get; set; }
        public int? Amount { get; set; }
        public int Status { get; set; }
        public int? OrderId { get; set; }
        public int? PaymentTypeId { get; set; }
    }

    public class PaymentDataMapModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? Amount { get; set; }
        public string Status { get; set; }
        public int? OrderId { get; set; }
        public int? PaymentTypeId { get; set; }

        public virtual PaymentTypeModel PaymentType { get; set; }
    }

    public class PaymentOfCustomerModel
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public int PaymentTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
