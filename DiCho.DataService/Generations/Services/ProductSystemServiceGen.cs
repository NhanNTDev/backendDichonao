namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IProductSystemService:IBaseService<ProductSystem>
    {
    }
    public partial class ProductSystemService:BaseService<ProductSystem>,IProductSystemService
    {
        public ProductSystemService(IUnitOfWork unitOfWork,IProductSystemRepository repository):base(unitOfWork,repository){}
    }
}
