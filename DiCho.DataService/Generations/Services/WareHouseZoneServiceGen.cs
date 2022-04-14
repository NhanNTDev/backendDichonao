namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IWareHouseZoneService:IBaseService<WareHouseZone>
    {
    }
    public partial class WareHouseZoneService:BaseService<WareHouseZone>,IWareHouseZoneService
    {
        public WareHouseZoneService(IUnitOfWork unitOfWork,IWareHouseZoneRepository repository):base(unitOfWork,repository){}
    }
}
