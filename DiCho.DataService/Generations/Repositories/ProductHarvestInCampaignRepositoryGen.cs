
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IProductHarvestInCampaignRepository :IBaseRepository<ProductHarvestInCampaign>
    {
    }
    public partial class ProductHarvestInCampaignRepository :BaseRepository<ProductHarvestInCampaign>, IProductHarvestInCampaignRepository
    {
         public ProductHarvestInCampaignRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

