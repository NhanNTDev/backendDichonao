
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IAspNetUserLoginsRepository :IBaseRepository<AspNetUserLogins>
    {
    }
    public partial class AspNetUserLoginsRepository :BaseRepository<AspNetUserLogins>, IAspNetUserLoginsRepository
    {
         public AspNetUserLoginsRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

