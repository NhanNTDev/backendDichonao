
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IPaymentTypeRepository :IBaseRepository<PaymentType>
    {
    }
    public partial class PaymentTypeRepository :BaseRepository<PaymentType>, IPaymentTypeRepository
    {
         public PaymentTypeRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

