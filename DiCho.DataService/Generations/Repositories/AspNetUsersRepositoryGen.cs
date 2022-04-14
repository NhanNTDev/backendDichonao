
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IAspNetUsersRepository :IBaseRepository<AspNetUsers>
    {
    }
    public partial class AspNetUsersRepository :BaseRepository<AspNetUsers>, IAspNetUsersRepository
    {
         public AspNetUsersRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

