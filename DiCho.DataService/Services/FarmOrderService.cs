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
    public partial interface IFarmOrderService
    {
        Task UpdateStatus(int id, string status);
        Task UpdateCancelStatus(int id, string note);
        Task UpdateFarmOrderDriver(List<FarmOrderUpdateDriverInputModel> modelInput);
        Task<List<GetFarmOrderModel>> GetFarmOrder(int campaignId, int farmId);
        Task<List<GetFarmOrderModel>> GetFarmOrderByCampaign(int campaignId);
        Task<List<RevenuseOfFarmer>> GetRevenues(string from, string to);
    }

    public partial class FarmOrderService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IFirebaseService _firebaseService;
        private readonly IFarmService _farmService;

        public FarmOrderService(IFarmOrderRepository repository, IFirebaseService firebaseService, IFarmService farmService,
            UserManager<AspNetUsers> userManager,
        IUnitOfWork unitOfWork, IMapper mapper = null) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _userManager = userManager;
            _firebaseService = firebaseService;
            _farmService = farmService;
        }

        public async Task<List<GetFarmOrderModel>> GetFarmOrder(int campaignId, int farmId)
        {
            var farmOrders = await Get(x => (x.Status == (int)FarmOrderEnum.Đãhoànthành || x.Status == (int)FarmOrderEnum.Đãhủy) && x.FarmId == farmId && x.Order.CampaignId == campaignId).ProjectTo<GetFarmOrderModel>(_mapper).ToListAsync();
            foreach (var farmOrder in farmOrders)
            {
                switch (farmOrder.Status)
                {
                    case "0":
                        farmOrder.Status = "Chờ xác nhận";
                        break;
                    case "1":
                        farmOrder.Status = "Đã xác nhận";
                        break;
                    case "2":
                        farmOrder.Status = "Đang chờ xử lý";
                        break;
                    case "3":
                        farmOrder.Status = "Đang bàn giao cho bên vận chuyển";
                        break;
                    case "4":
                        farmOrder.Status = "Đã bàn giao cho bên vận chuyển";
                        break;
                    case "5":
                        farmOrder.Status = "Đang vận chuyển";
                        break;
                    case "6":
                        farmOrder.Status = "Đã hoàn thành";
                        break;
                    case "7":
                        farmOrder.Status = "Đã hủy";
                        break;
                }
            }
            return farmOrders;
        } 
        
        public async Task<List<GetFarmOrderModel>> GetFarmOrderByCampaign(int campaignId)
        {
            var farmOrders = await Get(x => x.Status == (int)FarmOrderEnum.Đãhoànthành && x.Order.CampaignId == campaignId).ProjectTo<GetFarmOrderModel>(_mapper).ToListAsync();
            foreach (var farmOrder in farmOrders)
            {
                switch (farmOrder.Status)
                {
                    case "0":
                        farmOrder.Status = "Chờ xác nhận";
                        break;
                    case "1":
                        farmOrder.Status = "Đã xác nhận";
                        break;
                    case "2":
                        farmOrder.Status = "Đang chờ xử lý";
                        break;
                    case "3":
                        farmOrder.Status = "Đang bàn giao cho bên vận chuyển";
                        break;
                    case "4":
                        farmOrder.Status = "Đã bàn giao cho bên vận chuyển";
                        break;
                    case "5":
                        farmOrder.Status = "Đang vận chuyển";
                        break;
                    case "6":
                        farmOrder.Status = "Đã hoàn thành";
                        break;
                    case "7":
                        farmOrder.Status = "Đã hủy";
                        break;
                }
            }
            return farmOrders;
        } 

        public async Task UpdateFarmOrderDriver(List<FarmOrderUpdateDriverInputModel> modelInput)
        {
            foreach (var item in modelInput)
            {
                var farmOrders = Get(x => x.FarmId == item.FarmId && x.DriverId == null && x.Status == (int)FarmOrderEnum.Đangbàngiaochobênvậnchuyển).ToList();
                foreach (var farmOrder in farmOrders)
                {
                    var entity = await GetAsyn(farmOrder.Id);
                    if (entity == null)
                        throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
                    var model = new FarmOrderUpdateDriverModel { Id = farmOrder.Id, DriverId = item.DriverId };
                    var updateEntity = _mapper.CreateMapper().Map(model, entity);
                    await UpdateAsyn(updateEntity);
                }
                var farm = _farmService.Get(x => x.Id == item.FarmId).FirstOrDefault();
                string title = "Đơn hàng mới cần thu gom";
                string body = $"Đơn hàng từ nông trại {farm.Name} đang cần được thu gom.";
                await _firebaseService.SendNotification(title, body, item.FarmId.ToString());
                await _firebaseService.AddNotiToRedis(new NotificationModel { UserId = item.DriverId, Title = title, Body = body });
            }
        }

        public async Task UpdateStatus(int id, string status)
        {
            var entity = await GetAsyn(id);
            if (entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");

            switch (status)
            {
                case "Chờ xác nhận":
                    status = "0";
                    break;
                case "Đã xác nhận":
                    status = "1";
                    break;
                case "Đang chờ xử lý":
                    status = "2";
                    break;
                case "Đang bàn giao cho bên vận chuyển":
                    status = "3";
                    break;
                case "Đã bàn giao cho bên vận chuyển":
                    status = "4";
                    break;
                case "Đang vận chuyển":
                    status = "5";
                    break;
                case "Đã hoàn thành":
                    status = "6";
                    break;
                case "Đã hủy":
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Không thể cập nhật trạng thái này!");
                case "7":
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Không thể cập nhật trạng thái này!");
            }

            var model = new FarmOrderUpdateModel { Id = id, Status = Int32.Parse(status) };

            var updateEntity = _mapper.CreateMapper().Map(model, entity);
            await UpdateAsyn(updateEntity);
        }

        public async Task UpdateCancelStatus(int id, string note)
        {
            var entity = await GetAsyn(id);
            if (entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            var model = new FarmOrderUpdateCancelModel { Id = id, Status = (int)FarmOrderEnum.Đãhủy, Note = note };
            var updateEntity = _mapper.CreateMapper().Map(model, entity);
            await UpdateAsyn(updateEntity);
        }

        public async Task<List<RevenuseOfFarmer>> GetRevenues(string from, string to)
        {
            var farmers = _userManager.Users.Where(x => x.AspNetUserRoles.Any(y => y.Role.Name == "farmer")).ToList();

            var farmersRevenus = new List<RevenuseOfFarmer>();
            foreach (var farmer in farmers)
            {
                var farmOrders = new List<FarmOrderRevenuseModel>();
                if (from == null || to == null)
                    farmOrders = await Get(x => x.Status == (int)FarmOrderEnum.Đãhoànthành && x.Farm.FarmerId == farmer.Id).ProjectTo<FarmOrderRevenuseModel>(_mapper).ToListAsync();
                else
                    farmOrders = await Get(x => x.Status == (int)FarmOrderEnum.Đãhoànthành && x.Farm.FarmerId == farmer.Id && x.CreateAt >= Convert.ToDateTime(from) && x.CreateAt <= Convert.ToDateTime(to)).ProjectTo<FarmOrderRevenuseModel>(_mapper).ToListAsync();
                double total = 0;
                foreach (var farmOrder in farmOrders)
                {
                    farmOrder.Status = "Đã hoàn thành";
                    total += farmOrder.Total;
                }

                farmersRevenus.Add(new RevenuseOfFarmer { 
                    UserId = farmer.Id,
                    Name = farmer.Name,
                    TotalRevenues = total,
                    CountFarmOrder = farmOrders.Count,
                    FarmOrders = farmOrders.OrderByDescending(x => x.CreateAt).ToList()
                });
            }

            return farmersRevenus.Where(x => x.FarmOrders.Count > 0).ToList();
        }
    }
}
