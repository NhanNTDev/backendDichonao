namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IPaymentService:IBaseService<Payment>
    {
    }
    public partial class PaymentService:BaseService<Payment>,IPaymentService
    {
        public PaymentService(IUnitOfWork unitOfWork,IPaymentRepository repository):base(unitOfWork,repository){}
    }
}
