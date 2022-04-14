
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IAspNetUserRolesRepository :IBaseRepository<AspNetUserRoles>
    {
    }
    public partial class AspNetUserRolesRepository :BaseRepository<AspNetUserRoles>, IAspNetUserRolesRepository
    {
         public AspNetUserRolesRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

