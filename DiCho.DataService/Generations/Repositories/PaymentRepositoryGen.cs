
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IPaymentRepository :IBaseRepository<Payment>
    {
    }
    public partial class PaymentRepository :BaseRepository<Payment>, IPaymentRepository
    {
         public PaymentRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

