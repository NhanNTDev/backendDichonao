namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface ICampaignDeliveryZoneService:IBaseService<CampaignDeliveryZone>
    {
    }
    public partial class CampaignDeliveryZoneService:BaseService<CampaignDeliveryZone>,ICampaignDeliveryZoneService
    {
        public CampaignDeliveryZoneService(IUnitOfWork unitOfWork,ICampaignDeliveryZoneRepository repository):base(unitOfWork,repository){}
    }
}
