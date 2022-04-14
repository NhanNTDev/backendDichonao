
using Microsoft.EntityFrameworkCore;
using DiCho.Core.BaseConnect;
using DiCho.DataService.Models;
namespace DiCho.DataService.Repositories
{
    public partial interface IAddressRepository :IBaseRepository<Address>
    {
    }
    public partial class AddressRepository :BaseRepository<Address>, IAddressRepository
    {
         public AddressRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}

