
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IOrderRepository :IBaseRepository<Order>
    {
    }
    public partial class OrderRepository :BaseRepository<Order>, IOrderRepository
    {
         public OrderRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

