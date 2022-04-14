namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IFarmOrderService:IBaseService<FarmOrder>
    {
    }
    public partial class FarmOrderService:BaseService<FarmOrder>,IFarmOrderService
    {
        public FarmOrderService(IUnitOfWork unitOfWork,IFarmOrderRepository repository):base(unitOfWork,repository){}
    }
}
