namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IAspNetUserClaimsService:IBaseService<AspNetUserClaims>
    {
    }
    public partial class AspNetUserClaimsService:BaseService<AspNetUserClaims>,IAspNetUserClaimsService
    {
        public AspNetUserClaimsService(IUnitOfWork unitOfWork,IAspNetUserClaimsRepository repository):base(unitOfWork,repository){}
    }
}
