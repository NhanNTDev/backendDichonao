using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class AspNetUserTokens : IdentityUserToken<string>
    {
        public virtual AspNetUsers User { get; set; }
    }
}
