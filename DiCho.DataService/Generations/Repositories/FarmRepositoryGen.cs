
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IFarmRepository :IBaseRepository<Farm>
    {
    }
    public partial class FarmRepository :BaseRepository<Farm>, IFarmRepository
    {
         public FarmRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

