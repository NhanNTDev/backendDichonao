namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IOrderService:IBaseService<Order>
    {
    }
    public partial class OrderService:BaseService<Order>,IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork,IOrderRepository repository):base(unitOfWork,repository){}
    }
}
