using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiCho.Core.BaseConnect;
using DiCho.Core.Custom;
using DiCho.DataService.Models;
using DiCho.DataService.Repositories;
using DiCho.DataService.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.DataService.Services
{
    public partial interface IAddressService
    {
        Task<List<AddressModel>> Gets(string customerId);
        Task<AddressCreateModel> Create(AddressCreateModel model);
        Task<Address> Update(int id, AddressUpdateModel model);
        Task<Address> Delete(int id);
    }

    public partial class AddressService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IJWTService _jWTService;
        public AddressService(IAddressRepository repository, IJWTService jWTService,
            IUnitOfWork unitOfWork, IMapper mapper = null) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _jWTService = jWTService;
        }

        public async Task<List<AddressModel>> Gets(string customerId)
        {
            var result = await Get(x => x.CustomerId == customerId).ProjectTo<AddressModel>(_mapper).ToListAsync();
            return result;
        }

        public async Task<AddressCreateModel> Create (AddressCreateModel model)
        {
            if (Get(x => x.Address1 == model.Address1 && x.CustomerId == model.CustomerId).Any())
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Địa chỉ đã tồn tại!");

            var customer = await _jWTService.GetUserId(model.CustomerId);
            var entity = _mapper.CreateMapper().Map<Address>(model);
            if (model.Phone == null)
                entity.Phone = customer.Phone;
            if (model.Email == null)
                entity.Email = customer.Email;
            await CreateAsyn(entity);
            return model;
        }

        public async Task<Address> Update(int id, AddressUpdateModel model)
        {
            var entity = await GetAsyn(id);
            if (model.Id != id)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Vui lòng nhập đúng!");
            if (model.Id != id || entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            if (Get(x => x.Address1 == model.Address1 && x.Address1 != entity.Address1 && x.CustomerId == model.CustomerId).Any())
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Địa chỉ đã tồn tại!");
            var updateEntity = _mapper.CreateMapper().Map(model, entity);
            await UpdateAsyn(updateEntity);
            return updateEntity;
        }

        public async Task<Address> Delete(int id)
        {
            var entity = Get(id);
            if (entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            await DeleteAsyn(entity);
            return entity;
        }
    }
}
