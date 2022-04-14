
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IWareHouseRepository :IBaseRepository<WareHouse>
    {
    }
    public partial class WareHouseRepository :BaseRepository<WareHouse>, IWareHouseRepository
    {
         public WareHouseRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

