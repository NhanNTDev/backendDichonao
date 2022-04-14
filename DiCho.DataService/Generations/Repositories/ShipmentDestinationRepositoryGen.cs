
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IShipmentDestinationRepository :IBaseRepository<ShipmentDestination>
    {
    }
    public partial class ShipmentDestinationRepository :BaseRepository<ShipmentDestination>, IShipmentDestinationRepository
    {
         public ShipmentDestinationRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

