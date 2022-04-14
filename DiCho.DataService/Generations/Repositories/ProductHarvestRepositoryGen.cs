
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IProductHarvestRepository :IBaseRepository<ProductHarvest>
    {
    }
    public partial class ProductHarvestRepository :BaseRepository<ProductHarvest>, IProductHarvestRepository
    {
         public ProductHarvestRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

