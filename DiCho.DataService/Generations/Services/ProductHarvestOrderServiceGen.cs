namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IProductHarvestOrderService:IBaseService<ProductHarvestOrder>
    {
    }
    public partial class ProductHarvestOrderService:BaseService<ProductHarvestOrder>,IProductHarvestOrderService
    {
        public ProductHarvestOrderService(IUnitOfWork unitOfWork,IProductHarvestOrderRepository repository):base(unitOfWork,repository){}
    }
}
