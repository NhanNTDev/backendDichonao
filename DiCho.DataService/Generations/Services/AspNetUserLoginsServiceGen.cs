namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IAspNetUserLoginsService:IBaseService<AspNetUserLogins>
    {
    }
    public partial class AspNetUserLoginsService:BaseService<AspNetUserLogins>,IAspNetUserLoginsService
    {
        public AspNetUserLoginsService(IUnitOfWork unitOfWork,IAspNetUserLoginsRepository repository):base(unitOfWork,repository){}
    }
}
