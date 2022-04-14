using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class AspNetRoleClaims : IdentityRoleClaim<string>
    {
        public virtual AspNetRoles Role { get; set; }
    }
}
