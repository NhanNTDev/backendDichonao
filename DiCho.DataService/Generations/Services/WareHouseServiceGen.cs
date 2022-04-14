namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IWareHouseService:IBaseService<WareHouse>
    {
    }
    public partial class WareHouseService:BaseService<WareHouse>,IWareHouseService
    {
        public WareHouseService(IUnitOfWork unitOfWork,IWareHouseRepository repository):base(unitOfWork,repository){}
    }
}
