
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IAspNetRoleClaimsRepository :IBaseRepository<AspNetRoleClaims>
    {
    }
    public partial class AspNetRoleClaimsRepository :BaseRepository<AspNetRoleClaims>, IAspNetRoleClaimsRepository
    {
         public AspNetRoleClaimsRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

