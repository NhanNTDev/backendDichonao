namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IProductHarvestInCampaignService:IBaseService<ProductHarvestInCampaign>
    {
    }
    public partial class ProductHarvestInCampaignService:BaseService<ProductHarvestInCampaign>,IProductHarvestInCampaignService
    {
        public ProductHarvestInCampaignService(IUnitOfWork unitOfWork,IProductHarvestInCampaignRepository repository):base(unitOfWork,repository){}
    }
}
