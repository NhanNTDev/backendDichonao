
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface ICampaignRepository :IBaseRepository<Campaign>
    {
    }
    public partial class CampaignRepository :BaseRepository<Campaign>, ICampaignRepository
    {
         public CampaignRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

