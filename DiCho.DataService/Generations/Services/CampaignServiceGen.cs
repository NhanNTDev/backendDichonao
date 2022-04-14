namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface ICampaignService:IBaseService<Campaign>
    {
    }
    public partial class CampaignService:BaseService<Campaign>,ICampaignService
    {
        public CampaignService(IUnitOfWork unitOfWork,ICampaignRepository repository):base(unitOfWork,repository){}
    }
}
