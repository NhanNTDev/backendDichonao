using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiCho.Core.BaseConnect;
using DiCho.Core.Custom;
using DiCho.Core.Utilities;
using DiCho.DataService.Commons;
using DiCho.DataService.Models;
using DiCho.DataService.Repositories;
using DiCho.DataService.Response;
using DiCho.DataService.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.Services
{
    public partial interface IProductSystemService
    {
        Task<DynamicModelsResponse<ProductSystemModel>> Gets(ProductSystemModel model, int page, int size);
        Task<List<ProductSystemModel>> GetAllProductSystem();
        Task<ProductSystemModel> GetById(int id);
        Task<ProductSystemCreateModel> Create(ProductSystemCreateModel model);
        Task<ProductSystem> Update(int id, ProductSystemUpdateModel model);
        Task<ProductSystem> Delete(int id);
    }
    public partial class ProductSystemService
    {
        private readonly IConfigurationProvider _mapper;
        public ProductSystemService(IProductSystemRepository repository,
            IUnitOfWork unitOfWork, IMapper mapper = null) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
        }

        public async Task<DynamicModelsResponse<ProductSystemModel>> Gets(ProductSystemModel model, int page, int size)
        {
            var resultFilter = Get(x => x.Active).ProjectTo<ProductSystemModel>(_mapper)
                .DynamicFilter(model)
                .Select<ProductSystemModel>(ProductSystemModel.Fields.ToArray().ToDynamicSelector<ProductSystemModel>())
                .PagingIQueryable(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);
            var result = new DynamicModelsResponse<ProductSystemModel>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = resultFilter.Item1 },
                Data = await resultFilter.Item2.ToListAsync()
            };
            return result;
        }

        public async Task<List<ProductSystemModel>> GetAllProductSystem()
        {
            var products = await Get(x => x.Active).ProjectTo<ProductSystemModel>(_mapper).ToListAsync();
            return products;
        }

        public async Task<ProductSystemModel> GetById(int id)
        {
            var result = await Get(x => x.Id == id && x.Active).ProjectTo<ProductSystemModel>(_mapper).FirstOrDefaultAsync();
            if (result == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            return result;
        }
        public async Task<ProductSystemCreateModel> Create(ProductSystemCreateModel model)
        {
            if (Get(x => x.Name == model.Name).Any())
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Sản Phẩm này đã tồn tại!");
            var entity = _mapper.CreateMapper().Map<ProductSystem>(model);
            await CreateAsyn(entity);
            return model;
        }
        public async Task<ProductSystem> Update(int id, ProductSystemUpdateModel model)
        {
            var entity = await GetAsyn(id);
            if (model.Id != id)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Vui lòng nhập đúng!");
            if (model.Id != id || entity == null || entity.Active == false)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            var updateEntity = _mapper.CreateMapper().Map(model, entity);
            await UpdateAsyn(updateEntity);
            return updateEntity;
        }
        public async Task<ProductSystem> Delete(int id)
        {
            var entity = Get(id);
            var data = Get(x => x.Id == id).ProjectTo<ProductSystemHarvestDataModel>(_mapper).FirstOrDefault();
            if (entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            if (entity.Active == false)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Đã xóa rồi!");
            if (data.ProductHarvests.Count > 0)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Sản phẩm đang được các nông trại bày bán!");
            entity.Active = false;
            await UpdateAsyn(entity);
            return entity;
        }

    }
}
