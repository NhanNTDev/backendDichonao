namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IProductCategoryService:IBaseService<ProductCategory>
    {
    }
    public partial class ProductCategoryService:BaseService<ProductCategory>,IProductCategoryService
    {
        public ProductCategoryService(IUnitOfWork unitOfWork,IProductCategoryRepository repository):base(unitOfWork,repository){}
    }
}
