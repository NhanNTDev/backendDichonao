
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IFarmOrderRepository :IBaseRepository<FarmOrder>
    {
    }
    public partial class FarmOrderRepository :BaseRepository<FarmOrder>, IFarmOrderRepository
    {
         public FarmOrderRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

