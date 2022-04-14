namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IAspNetUserRolesService:IBaseService<AspNetUserRoles>
    {
    }
    public partial class AspNetUserRolesService:BaseService<AspNetUserRoles>,IAspNetUserRolesService
    {
        public AspNetUserRolesService(IUnitOfWork unitOfWork,IAspNetUserRolesRepository repository):base(unitOfWork,repository){}
    }
}
