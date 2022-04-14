
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface ICampaignDeliveryZoneRepository :IBaseRepository<CampaignDeliveryZone>
    {
    }
    public partial class CampaignDeliveryZoneRepository :BaseRepository<CampaignDeliveryZone>, ICampaignDeliveryZoneRepository
    {
         public CampaignDeliveryZoneRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

