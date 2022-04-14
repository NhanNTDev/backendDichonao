using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.ViewModels
{
    public class PaymentTypeModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
    }
}
