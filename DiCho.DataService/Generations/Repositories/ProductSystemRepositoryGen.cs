
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IProductSystemRepository :IBaseRepository<ProductSystem>
    {
    }
    public partial class ProductSystemRepository :BaseRepository<ProductSystem>, IProductSystemRepository
    {
         public ProductSystemRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

