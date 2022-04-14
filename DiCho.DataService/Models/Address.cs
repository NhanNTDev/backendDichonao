using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Description { get; set; }
        public string CustomerId { get; set; }

        public virtual AspNetUsers Customer { get; set; }
    }
}
