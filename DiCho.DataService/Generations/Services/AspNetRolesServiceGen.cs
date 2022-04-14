namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IAspNetRolesService:IBaseService<AspNetRoles>
    {
    }
    public partial class AspNetRolesService:BaseService<AspNetRoles>,IAspNetRolesService
    {
        public AspNetRolesService(IUnitOfWork unitOfWork,IAspNetRolesRepository repository):base(unitOfWork,repository){}
    }
}
