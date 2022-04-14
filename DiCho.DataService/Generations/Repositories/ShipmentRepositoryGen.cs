
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IShipmentRepository :IBaseRepository<Shipment>
    {
    }
    public partial class ShipmentRepository :BaseRepository<Shipment>, IShipmentRepository
    {
         public ShipmentRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

