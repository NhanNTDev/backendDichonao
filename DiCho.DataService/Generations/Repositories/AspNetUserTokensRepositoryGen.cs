
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IAspNetUserTokensRepository :IBaseRepository<AspNetUserTokens>
    {
    }
    public partial class AspNetUserTokensRepository :BaseRepository<AspNetUserTokens>, IAspNetUserTokensRepository
    {
         public AspNetUserTokensRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

