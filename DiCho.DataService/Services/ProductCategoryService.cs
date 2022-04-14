using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiCho.Core.BaseConnect;
using DiCho.Core.Custom;
using DiCho.Core.Utilities;
using DiCho.DataService.Commons;
using DiCho.DataService.Enums;
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
    public partial interface IProductCategoryService
    {
        Task<DynamicModelsResponse<ProductCategoryModel>> Gets(ProductCategoryModel model, int page, int size);
        Task<ProductCategoryModel> GetById(int id);
        Task<ProductCategoryCreateModel> Create(ProductCategoryCreateModel model);
        Task<ProductCategory> Update(int id, ProductCategoryUpdateModel model);
        Task<ProductCategory> Delete(int id);
    }
    public partial class ProductCategoryService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IProductHarvestService _harvestService;


        public ProductCategoryService(IProductCategoryRepository repository,
            IProductHarvestService harvestService,
            IUnitOfWork unitOfWork, IMapper mapper = null) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _harvestService = harvestService;
        }
        public async Task<DynamicModelsResponse<ProductCategoryModel>> Gets(ProductCategoryModel model, int page, int size)
        {
            var resultFilter = Get().ProjectTo<ProductCategoryModel>(_mapper)
                .DynamicFilter(model)
                .Select<ProductCategoryModel>(ProductCategoryModel.Fields.ToArray().ToDynamicSelector<ProductCategoryModel>())
                .PagingIQueryable(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);
            var listCategory = await resultFilter.Item2.ToListAsync();

            foreach (var category in listCategory)
                category.ProductInventory = _harvestService.Get(x => x.Active && x.ProductSystem.ProductCategoryId == category.Id).Count();

            var result = new DynamicModelsResponse<ProductCategoryModel>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = resultFilter.Item1 },
                Data = listCategory
            };
            return result;
        }
        public async Task<ProductCategoryModel> GetById(int id)
        {
            var result = await Get(x => x.Id == id && x.Active).ProjectTo<ProductCategoryModel>(_mapper).FirstOrDefaultAsync();
            result.ProductInventory = _harvestService.Get(x => x.Active && x.ProductSystem.ProductCategoryId == result.Id).Count();
            if (result == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            return result;
        }
        public async Task<ProductCategoryCreateModel> Create(ProductCategoryCreateModel model)
        {
            if (Get(x => x.Name == model.Name).Any())
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Loại này đã tồn tại!");
            var entity = _mapper.CreateMapper().Map<ProductCategory>(model);
            await CreateAsyn(entity);
            return model;
        }
        public async Task<ProductCategory> Update(int id, ProductCategoryUpdateModel model)
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
        public async Task<ProductCategory> Delete(int id)
        {
            var entity = Get(id);
            if (entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            if (!entity.Active)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Đã xóa rồi!");
            entity.Active = false;
            await UpdateAsyn(entity);
            return entity;
        }
    }
}
