namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IProductSalesCampaignService:IBaseService<ProductSalesCampaign>
    {
    }
    public partial class ProductSalesCampaignService:BaseService<ProductSalesCampaign>,IProductSalesCampaignService
    {
        public ProductSalesCampaignService(IUnitOfWork unitOfWork,IProductSalesCampaignRepository repository):base(unitOfWork,repository){}
    }
}
