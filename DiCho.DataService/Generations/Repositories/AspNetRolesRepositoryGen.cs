
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IAspNetRolesRepository :IBaseRepository<AspNetRoles>
    {
    }
    public partial class AspNetRolesRepository :BaseRepository<AspNetRoles>, IAspNetRolesRepository
    {
         public AspNetRolesRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

