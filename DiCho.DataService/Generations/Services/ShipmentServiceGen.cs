namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IShipmentService:IBaseService<Shipment>
    {
    }
    public partial class ShipmentService:BaseService<Shipment>,IShipmentService
    {
        public ShipmentService(IUnitOfWork unitOfWork,IShipmentRepository repository):base(unitOfWork,repository){}
    }
}
