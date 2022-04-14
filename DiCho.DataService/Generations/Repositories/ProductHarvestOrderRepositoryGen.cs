
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IProductHarvestOrderRepository :IBaseRepository<ProductHarvestOrder>
    {
    }
    public partial class ProductHarvestOrderRepository :BaseRepository<ProductHarvestOrder>, IProductHarvestOrderRepository
    {
         public ProductHarvestOrderRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

