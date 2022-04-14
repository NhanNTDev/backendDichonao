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
    public partial interface IFarmService
    {
        Task<DynamicModelsResponse<FarmViewModel>> Gets(string farmerId, int page, int size);
        Task<FarmModel> GetById(int id);
        Task<FarmCreateModel> Create(FarmCreateInputModel modelInput);
        Task<Farm> Update(int id, FarmUpdateInputModel model);
        Task<Farm> Delete(int id);
        Task<List<FarmNameModel>> GetFarmByName(string farmerId, FarmNameModel model);
        Task<int> CountFarmByFarmerId(string farmerId);
        Task<List<FarmInCampaignModel>> GetFarmInCampaign(int campaignId);
        Task<List<FarmStatisticalModel>> FarmStatistical(int campaignId);
    }
    public partial class FarmService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IFirebaseService _firebaseService;
        private readonly ITradeZoneMapService _tradeZoneMapService;
        private readonly IProductHarvestInCampaignService _harvestCampaignService;
        private readonly IProductHarvestService _productHarvestService;

        public FarmService(IFarmRepository repository, IProductHarvestService productHarvestService,
            IFirebaseService firebaseService, ITradeZoneMapService tradeZoneMapService, IProductHarvestInCampaignService harvestCampaignService,
            IUnitOfWork unitOfWork, IMapper mapper = null) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _firebaseService = firebaseService;
            _tradeZoneMapService = tradeZoneMapService;
            _harvestCampaignService = harvestCampaignService;
            _productHarvestService = productHarvestService;
        }
        public async Task<DynamicModelsResponse<FarmViewModel>> Gets(string farmerId, int page, int size)
        {
            var farms = new List<FarmViewModel>();
            farms = await Get(x => x.Active && x.FarmerId == farmerId).ProjectTo<FarmViewModel>(_mapper).ToListAsync();

            foreach (var farm in farms)
            {
                decimal star = 0;
                var farmData = Get(x => x.Id == farm.Id).ProjectTo<FarmModel>(_mapper).FirstOrDefault();
                var farmOrders = farmData.FarmOrders.Where(x => x.Star != 0).ToList();
                foreach (var farmOrder in farmOrders)
                {
                    star += (decimal)farmOrder.Star;
                }
                farm.Feedbacks = farmOrders.Count;
                if (star > 0)
                {
                    var total = star / farmOrders.Count;
                    var multiplier = Math.Pow(10, 1);
                    total = Math.Ceiling(total * (decimal)multiplier) / (decimal)multiplier;
                    farm.TotalStar = total;
                }
                else
                    farm.TotalStar = 0;
            }
            var listPaging = farms.PagingList(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            var result = new DynamicModelsResponse<FarmViewModel>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = listPaging.Item1 },
                Data = listPaging.Item2
            };
            return result;
        }
         
        public async Task<List<FarmStatisticalModel>> FarmStatistical(int campaignId)
        {
            var farms = await Get(x => x.Active && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaignId))).ProjectTo<FarmStatisticalModel>(_mapper).ToListAsync();

            foreach (var farm in farms)
            {
                var data = Get(x => x.Id == farm.Id).ProjectTo<FarmMapFarmOrderModel>(_mapper).FirstOrDefault();
                var farmOrders = data.FarmOrders.Where(x => x.Status == (int)FarmOrderEnum.Đãhoànthành).ToList();
                double total = 0;
                foreach (var farmOrder in farmOrders)
                {
                    total += farmOrder.Total;  
                }
                farm.CountFarmOrder = farmOrders.Count;
                farm.Total = total;
            }

            return farms;
        }

        public async Task<List<FarmInCampaignModel>> GetFarmInCampaign(int campaignId)
        {
            var farms = await Get(x => x.Active && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaignId))).ProjectTo<FarmInCampaignModel>(_mapper).ToListAsync();

            foreach (var farm in farms)
            {
                var count = 0;
                var data = Get(x => x.Id == farm.Id).ProjectTo<FarmMapToCampaginAndFarmOrderModel>(_mapper).FirstOrDefault();
                foreach (var item in data.ProductHarvests)
                {
                    count += item.ProductHarvestInCampaigns.Where(x => x.CampaignId == campaignId).Count();
                }
                farm.CountHarvestInCampaign = count;
                farm.CountFarmOrder = data.FarmOrders.Count;
            }
            return farms;
        }

        public async Task<FarmModel> GetById(int id)
        {
            var farm = await Get(x => x.Id == id && x.Active).ProjectTo<FarmModel>(_mapper).FirstOrDefaultAsync();
            if (farm == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            decimal star = 0;
            var farmOrders = farm.FarmOrders.Where(x => x.Star != 0).ToList();
            foreach (var farmOrder in farmOrders)
            {
                star += (decimal)farmOrder.Star;
            }
            farm.Feedbacks = farmOrders.Count;
            farm.FarmOrders = farmOrders;
            if (star > 0)
            {
                var total = star / farmOrders.Count;
                var multiplier = Math.Pow(10, 1);
                total = Math.Ceiling(total * (decimal)multiplier) / (decimal)multiplier;
                farm.TotalStar = total;
            }
            else
                farm.TotalStar = 0;
            return farm;
        }
        public async Task<FarmCreateModel> Create(FarmCreateInputModel modelInput)
        {
            var folderUpload = "Product";
            var avatar = await _firebaseService.UploadFileToFirebase(modelInput.Avatar, folderUpload);
            List<string> listLink = await _firebaseService.UploadFilesToFirebase(modelInput.Images, folderUpload);

            var address = await _tradeZoneMapService.GetAddress(modelInput.Address);
            var zone = await _tradeZoneMapService.CheckZone(address);

            var model = new FarmCreateModel
            {
                FarmerId = modelInput.FarmerId,
                Address = address,
                Description = modelInput.Description,
                FarmZoneId = zone.f4,
                FarmZoneName = zone.f1,
                Name = modelInput.Name
            };
            if (Get(x => x.Address == model.Address && x.Active).Any())
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Nông trại này đã tồn tại!");
            if (modelInput.Avatar != null)
                model.Avatar = avatar;
            else
                model.Avatar = "https://firebasestorage.googleapis.com/v0/b/dichonao-f5871.appspot.com/o/Images%2FAvatar%2Fdichonao_default.png?alt=media&token=a63e1bcf-6d00-47b6-94fd-84667446dd21";

            if (listLink.Count == 1)
                model.Image1 = listLink[0];
            else if (listLink.Count == 2)
            {
                model.Image1 = listLink[0];
                model.Image2 = listLink[1];
            }
            else if (listLink.Count == 3)
            {
                model.Image1 = listLink[0];
                model.Image2 = listLink[1];
                model.Image3 = listLink[2];
            }
            else if (listLink.Count == 4)
            {
                model.Image1 = listLink[0];
                model.Image2 = listLink[1];
                model.Image3 = listLink[2];
                model.Image4 = listLink[3];
            }
            else if (listLink.Count == 5)
            {
                model.Image1 = listLink[0];
                model.Image2 = listLink[1];
                model.Image3 = listLink[2];
                model.Image4 = listLink[3];
                model.Image5 = listLink[4];
            }

            var entity = _mapper.CreateMapper().Map<Farm>(model);
            await CreateAsyn(entity);
            return model;
        }
        public async Task<Farm> Update(int id, FarmUpdateInputModel modelInput)
        {
            var entity = await GetAsyn(id);
            if (modelInput.Id != id)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Vui lòng nhập đúng!");
            if (modelInput.Id != id || entity == null || entity.Active == false)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");

            var folderUpload = "Product";
            var avatar = await _firebaseService.UploadFileToFirebase(modelInput.Avatar, folderUpload);
            List<string> listLink = await _firebaseService.UploadFilesToFirebase(modelInput.Images, folderUpload);

            var address = await _tradeZoneMapService.GetAddress(modelInput.Address);
            var zone = await _tradeZoneMapService.CheckZone(address);

            var model = new FarmUpdateModel
            {
                FarmerId = entity.FarmerId,
                Address = address,
                FarmZoneId = zone.f4,
                FarmZoneName = zone.f1,
                Description = modelInput.Description,
                Name = modelInput.Name,
                Id = modelInput.Id
            };

            if (Get(x => x.Active && x.Address == model.Address && entity.Address != model.Address).Any())
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Địa chỉ nông trại này đã tồn tại!");
            if (modelInput.Avatar != null)
                model.Avatar = avatar;
            else
                model.Avatar = entity.Avatar;

            if (listLink.Count == 1)
                model.Image1 = listLink[0];
            else if (listLink.Count == 2)
            {
                model.Image1 = listLink[0];
                model.Image2 = listLink[1];
            }
            else if (listLink.Count == 3)
            {
                model.Image1 = listLink[0];
                model.Image2 = listLink[1];
                model.Image3 = listLink[2];
            }
            else if (listLink.Count == 4)
            {
                model.Image1 = listLink[0];
                model.Image2 = listLink[1];
                model.Image3 = listLink[2];
                model.Image4 = listLink[3];
            }
            else if (listLink.Count == 5)
            {
                model.Image1 = listLink[0];
                model.Image2 = listLink[1];
                model.Image3 = listLink[2];
                model.Image4 = listLink[3];
                model.Image5 = listLink[4];
            }
            else
            {
                model.Image1 = entity.Image1;
                model.Image2 = entity.Image2;
                model.Image3 = entity.Image3;
                model.Image4 = entity.Image4;
                model.Image5 = entity.Image5;
            }

            var updateEntity = _mapper.CreateMapper().Map(model, entity);
            await UpdateAsyn(updateEntity);
            return updateEntity;
        }
        public async Task<Farm> Delete(int id)
        {
            var entity = Get(x => x.Id == id).FirstOrDefault();
            var farm = Get(x => x.Id == id).ProjectTo<FarmMapToCampaginAndFarmOrderModel>(_mapper).FirstOrDefault();
            if (entity == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            if (entity.Active == false)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Đã xóa rồi!");
            if (farm.ProductHarvests.Any(x => x.ProductHarvestInCampaigns.Any(y => y.Status != (int)HarvestCampaignEnum.Đãbịtừchối)))
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Nông trại đang có sản phẩm được bày bán. Không thể xóa!");
            if (farm.ProductHarvests.Count > 0)
            {
                foreach (var productHarvest in farm.ProductHarvests)
                {
                    var productHarvestEntity = _productHarvestService.Get(x => x.Id == productHarvest.Id).FirstOrDefault();
                    if (productHarvest.ProductHarvestInCampaigns.Count > 0)
                    {
                        foreach (var productHarvestInCampaign in productHarvest.ProductHarvestInCampaigns)
                        {
                            var productHarvestInCampaignEntity = _harvestCampaignService.Get(x => x.Id == productHarvestInCampaign.Id).FirstOrDefault();
                            await _harvestCampaignService.DeleteAsyn(productHarvestInCampaignEntity);
                        }
                    }
                    await _productHarvestService.DeleteAsyn(productHarvestEntity);
                }
            }
            await DeleteAsyn(entity);
            return entity;
        }
        public async Task<List<FarmNameModel>> GetFarmByName(string farmerId, FarmNameModel model)
        {
            if (farmerId == null)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Vui lòng nhập nông dân!");
            var resultFilter = Get(x => x.Active && x.FarmerId == farmerId).ProjectTo<FarmNameModel>(_mapper).DynamicFilter(model);
            var result = await resultFilter.ToListAsync();
            return result;
        }

        public async Task<int> CountFarmByFarmerId(string farmerId)
        {
            var result = await Get(x => x.Active && x.FarmerId == farmerId).CountAsync();
            return result;
        }
    }
}
