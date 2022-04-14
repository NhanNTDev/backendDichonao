
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IProductSalesCampaignRepository :IBaseRepository<ProductSalesCampaign>
    {
    }
    public partial class ProductSalesCampaignRepository :BaseRepository<ProductSalesCampaign>, IProductSalesCampaignRepository
    {
         public ProductSalesCampaignRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

