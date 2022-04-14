using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class AspNetUserLogins : IdentityUserLogin<string>
    {
        public virtual AspNetUsers User { get; set; }
    }
}
