
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IAspNetUserClaimsRepository :IBaseRepository<AspNetUserClaims>
    {
    }
    public partial class AspNetUserClaimsRepository :BaseRepository<AspNetUserClaims>, IAspNetUserClaimsRepository
    {
         public AspNetUserClaimsRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

