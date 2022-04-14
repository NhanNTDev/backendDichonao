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
using Microsoft.AspNetCore.Identity;
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
    public partial interface IWareHouseService
    {
        Task<List<WareHouseModel>> GetAllWarehouse(string name);
        Task<WareHouseModel> GetWarehouse(int id);
        Task CreateWarehouse(WareHouseCreateModel model);
        Task UpdateWarehouse(int id, WareHouseUpdateModel model);
        Task DeleteWarehouse(int id);
        Task<List<CustomerOrder>> GetWareHouseManger();
        Task<WareHouseModel> GetWarehouseByWarehouseManager(string warehouseManagerId);

    }

    public partial class WareHouseService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IJWTService _jWTService;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IWareHouseZoneService _wareHouseZoneService;
        private readonly ITradeZoneMapService _tradeZoneMapService;
        public WareHouseService(IWareHouseRepository repository, IJWTService jWTService, UserManager<AspNetUsers> userManager, ITradeZoneMapService tradeZoneMapService,
            IWareHouseZoneService wareHouseZoneService, IUnitOfWork unitOfWork, IMapper mapper = null) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _jWTService = jWTService;
            _userManager = userManager;
            _wareHouseZoneService = wareHouseZoneService;
            _tradeZoneMapService = tradeZoneMapService;
        }

        public async Task<List<WareHouseModel>> GetAllWarehouse(string name)
        {
            var warehouses = new List<WareHouseModel>();
            if (name == null)
                warehouses = await Get(x => x.Active).ProjectTo<WareHouseModel>(_mapper).ToListAsync();
            else
                warehouses = await Get(x => x.Active && name.Contains(x.Name)).ProjectTo<WareHouseModel>(_mapper).ToListAsync();
            foreach (var warehouse in warehouses)
            {
                if (warehouse.WarehouseManagerId != null)
                    warehouse.WarehouseManagerName = await _jWTService.GetNameOfUser(warehouse.WarehouseManagerId);
            }
            return warehouses;
        }

        public async Task<WareHouseModel> GetWarehouse(int id)
        {
            var warehouse = await Get(x => x.Id == id).ProjectTo<WareHouseModel>(_mapper).FirstOrDefaultAsync();
            if (warehouse == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            if (warehouse.WarehouseManagerId != null)
                warehouse.WarehouseManagerName = await _jWTService.GetNameOfUser(warehouse.WarehouseManagerId);
            return warehouse;
        }
        
        public async Task<WareHouseModel> GetWarehouseByWarehouseManager(string warehouseManagerId)
        {
            var warehouse = await Get(x => x.WarehouseManagerId == warehouseManagerId).ProjectTo<WareHouseModel>(_mapper).FirstOrDefaultAsync();
            if (warehouse == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            if (warehouse.WarehouseManagerId != null)
                warehouse.WarehouseManagerName = await _jWTService.GetNameOfUser(warehouse.WarehouseManagerId);
            return warehouse;
        }

        public async Task CreateWarehouse(WareHouseCreateModel model)
        {
            if (Get(x => x.Address == model.Address).Any())
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Kho này đã tồn tại rồi!");
            var entity = _mapper.CreateMapper().Map<WareHouse>(model);

            var zones = await _tradeZoneMapService.GetListZone();

            foreach (var wareHouseZone in entity.WareHouseZones)
            {
                wareHouseZone.WareHouseId = entity.Id;
                foreach (var zone in zones)
                {
                    if (zone.Id == wareHouseZone.ZoneId)
                        wareHouseZone.WareHouseZoneName = zone.Name;
                }
                
            }
            await CreateAsyn(entity);
        }

        public async Task UpdateWarehouse(int id, WareHouseUpdateModel model)
        {
            var entity = await Get(x => x.Id == id).FirstOrDefaultAsync();
            if (model.Id != id)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Vui lòng nhập đúng!");
            if (model.Id != id || entity == null || entity.Active == false)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            var warehouseZones = _wareHouseZoneService.Get(x => x.WareHouseId == entity.Id).ToList();
            _wareHouseZoneService.RemoveRange(warehouseZones);

            var updateEntity = _mapper.CreateMapper().Map(model, entity);

            var zones = await _tradeZoneMapService.GetListZone();

            foreach (var wareHouseZone in updateEntity.WareHouseZones)
            {
                wareHouseZone.WareHouseId = updateEntity.Id;
                foreach (var zone in zones)
                {
                    if (zone.Id == wareHouseZone.ZoneId)
                        wareHouseZone.WareHouseZoneName = zone.Name;
                }

            }
            await UpdateAsyn(updateEntity);
        }

        public async Task DeleteWarehouse(int id)
        {
            var entity = Get(id);
            if (entity == null || entity.Active == false)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            entity.Active = false;
            await UpdateAsyn(entity);
        }

        public async Task<List<CustomerOrder>> GetWareHouseManger()
        {
            var users = await _userManager.Users.Where(x => x.AspNetUserRoles.Any(y => y.Role.Name == "warehouseManager")).ToListAsync();
            var listUser = new List<CustomerOrder>();
            foreach (var user in users)
            {
                var warehouse = Get(x => x.WarehouseManagerId == user.Id).FirstOrDefault();
                if (warehouse == null)
                {
                    listUser.Add(new CustomerOrder
                    {
                        Id = user.Id,
                        Name = user.Name
                    });
                }
            }
            return listUser;
        }
    }
}
