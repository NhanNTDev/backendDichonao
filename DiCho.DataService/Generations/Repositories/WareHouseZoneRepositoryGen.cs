
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IWareHouseZoneRepository :IBaseRepository<WareHouseZone>
    {
    }
    public partial class WareHouseZoneRepository :BaseRepository<WareHouseZone>, IWareHouseZoneRepository
    {
         public WareHouseZoneRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

