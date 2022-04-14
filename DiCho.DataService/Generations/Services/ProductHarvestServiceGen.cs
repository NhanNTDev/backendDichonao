namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IProductHarvestService:IBaseService<ProductHarvest>
    {
    }
    public partial class ProductHarvestService:BaseService<ProductHarvest>,IProductHarvestService
    {
        public ProductHarvestService(IUnitOfWork unitOfWork,IProductHarvestRepository repository):base(unitOfWork,repository){}
    }
}
