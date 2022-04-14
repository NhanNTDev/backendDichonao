
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IProductCategoryRepository :IBaseRepository<ProductCategory>
    {
    }
    public partial class ProductCategoryRepository :BaseRepository<ProductCategory>, IProductCategoryRepository
    {
         public ProductCategoryRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

