namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IAspNetRoleClaimsService:IBaseService<AspNetRoleClaims>
    {
    }
    public partial class AspNetRoleClaimsService:BaseService<AspNetRoleClaims>,IAspNetRoleClaimsService
    {
        public AspNetRoleClaimsService(IUnitOfWork unitOfWork,IAspNetRoleClaimsRepository repository):base(unitOfWork,repository){}
    }
}
