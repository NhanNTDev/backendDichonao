namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IPaymentTypeService:IBaseService<PaymentType>
    {
    }
    public partial class PaymentTypeService:BaseService<PaymentType>,IPaymentTypeService
    {
        public PaymentTypeService(IUnitOfWork unitOfWork,IPaymentTypeRepository repository):base(unitOfWork,repository){}
    }
}
