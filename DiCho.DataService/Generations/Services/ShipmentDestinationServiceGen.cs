namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IShipmentDestinationService:IBaseService<ShipmentDestination>
    {
    }
    public partial class ShipmentDestinationService:BaseService<ShipmentDestination>,IShipmentDestinationService
    {
        public ShipmentDestinationService(IUnitOfWork unitOfWork,IShipmentDestinationRepository repository):base(unitOfWork,repository){}
    }
}
