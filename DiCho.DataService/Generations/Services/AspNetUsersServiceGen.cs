namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IAspNetUsersService:IBaseService<AspNetUsers>
    {
    }
    public partial class AspNetUsersService:BaseService<AspNetUsers>,IAspNetUsersService
    {
        public AspNetUsersService(IUnitOfWork unitOfWork,IAspNetUsersRepository repository):base(unitOfWork,repository){}
    }
}
