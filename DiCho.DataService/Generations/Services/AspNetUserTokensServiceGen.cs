namespace DiCho.DataService.Services
{
    using DiCho.Core.BaseConnect;
    using DiCho.DataService.Models;
    using DiCho.DataService.Repositories;
    public partial interface IAspNetUserTokensService:IBaseService<AspNetUserTokens>
    {
    }
    public partial class AspNetUserTokensService:BaseService<AspNetUserTokens>,IAspNetUserTokensService
    {
        public AspNetUserTokensService(IUnitOfWork unitOfWork,IAspNetUserTokensRepository repository):base(unitOfWork,repository){}
    }
}
