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
    public partial interface ICampaignService
    {
        Task<List<HarvestSuggestFarmerApplyModel>> GetCampaignForFarmerApply(string farmerId, int campaignId);
        Task<DynamicModelsResponse<CampaignModel>> GetByZoneId(int deliveryZoneId, CampaignModel model, int page, int size);
        Task<DynamicModelsResponse<CampaignModel>> GetAllCampaign(CampaignModel model, int page, int size);
        Task<DynamicModelsResponse<CampaignOfFarmerAppliedModel>> GetCampaignOfFarmerApplied(string farmerId, int page, int size);
        Task<List<CampaignSearchModel>> SearchCampaignApplied(string farmerId, CampaignSearchModel model);
        Task<DynamicModelsResponse<CampaignOfFarmerApplyModel>> GetCampaignOfFarmerApply(string farmerId, string type, int page, int size);
        Task<List<CampaignSearchModel>> SearchCampaignApply(string farmerId, CampaignSearchModel model);
        Task<CampaignFarmZoneModel> GetById(int id);
        Task<CampaignDetailApplyModel> GetCampaignDetailApply(int campaignId);
        Task<CampaignDetailApplieModel> GetCampaignDetailApplied(string farmerId, int campaignId);
        Task<CampaignCreateModel> Create(CampaignCreateInputModel model);
        Task<Campaign> Update(int id, CampaignUpdateInputModel model);
        Task<Campaign> Delete(int id, string note);
        Task<List<CampaignApplyRequestModel>> GetApplyRequest();
        Task<DynamicModelsResponse<FarmMappingHarvestInCampaign>> GetFarmApplyRequest(int campaignId, int page, int size);
        Task<DynamicModelsResponse<HarvestCampaignApplyRequest>> GetHarvestApplyRequest(int campaignId, int farmId, int page, int size);
        Task<List<FarmApplyModel>> GetFarmsCanJoinCampaign(string farmerId, int campaignId);
        Task<List<HarvestApplyModel>> GetHarvestApplyOfFarm(int farmId, int campaignId);
        Task<HarvestDetailModel> GetHarvestDetail(int harvestId);
        Task<int> CountCampaignApply(string farmerId, string type);
        Task<int> CountCampaignApplied(string farmerId);
        Task<DashBoardOfFarmer> DashBoard(string farmerId);
    }
    public partial class CampaignService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly IFarmService _farmService;
        private readonly ICampaignDeliveryZoneService _campaignDeliveryZoneService;
        private readonly IProductHarvestService _harvestService;
        private readonly IProductHarvestInCampaignService _harvestCampaignService;
        private readonly ITradeZoneMapService _tradeZoneMapService;
        private readonly IFirebaseService _firebaseService;
        private readonly IFarmOrderService _farmOrderService;
        private readonly IProductSalesCampaignService _productSalesCampaignService;


        public CampaignService(ICampaignRepository repository, IFarmOrderService farmOrderService, IProductSalesCampaignService productSalesCampaignService,
            IFirebaseService firebaseService, ITradeZoneMapService tradeZoneMapService, IProductHarvestInCampaignService harvestCampaignService, IProductHarvestService harvestService, IFarmService farmService, ICampaignDeliveryZoneService campaignDeliveryZoneService,
            IUnitOfWork unitOfWork, IMapper mapper = null) : base(unitOfWork, repository)
        {
            _mapper = mapper.ConfigurationProvider;
            _farmService = farmService;
            _campaignDeliveryZoneService = campaignDeliveryZoneService;
            _harvestService = harvestService;
            _harvestCampaignService = harvestCampaignService;
            _tradeZoneMapService = tradeZoneMapService;
            _firebaseService = firebaseService;
            _farmOrderService = farmOrderService;
            _productSalesCampaignService = productSalesCampaignService;
        }

        public async Task<List<HarvestSuggestFarmerApplyModel>> GetCampaignForFarmerApply(string farmerId, int campaignId)
        {
            var campaign = await Get( x => x.Id == campaignId).ProjectTo<CampaignSuggestFarmerApply>(_mapper).FirstOrDefaultAsync();
            if (campaign == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy");

            var farms = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).ProjectTo<FarmApplyModel>(_mapper).ToListAsync();
            var farmsApply = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId).ProjectTo<FarmSuggestFarmerApply>(_mapper).ToListAsync();
            foreach (var farm in farms)
                farmsApply.Remove(farmsApply.Where(x => farm.Name.Contains(x.Name)).FirstOrDefault());

            var harvestsApply = new List<HarvestSuggestFarmerApplyModel>();
            foreach (var farm in farmsApply)
            {
                var harvestsData = farm.ProductHarvests.Where(x => x.Active && x.InventoryTotal > 0).ToList();
                foreach (var harvestData in harvestsData)
                {
                    var harvest = _harvestService.Get(x => x.Id == harvestData.Id).ProjectTo<HarvestSuggestFarmerApplyModel>(_mapper).FirstOrDefault();
                    if (harvest == null)
                        return new List<HarvestSuggestFarmerApplyModel> { };
                    if (!harvestData.ProductSystem.ProductSalesCampaigns.Any(x => x.ProductSystemId == harvestData.ProductSystemId && x.CampaignId == campaignId))
                        harvestsApply.Remove(harvest);
                    else 
                    {
                        foreach (var product in harvestData.ProductSystem.ProductSalesCampaigns)
                        {
                            if (harvestData.InventoryTotal > product.Capacity)
                                harvest.InventoryTotal = product.Capacity;
                            harvest.Capacity = product.Capacity;
                        }
                        harvest.InventoryHarvest = harvestData.InventoryTotal;
                        harvest.FarmName = harvestData.Farm.Name;
                        harvest.MinPrice = harvestData.ProductSystem.MinPrice;
                        harvest.MaxPrice = harvestData.ProductSystem.MaxPrice;
                        harvest.UnitSystem = harvestData.ProductSystem.Unit;
                        harvestsApply.Add(harvest);
                    }
                }
            }
            return harvestsApply;
        }

        public async Task<DynamicModelsResponse<CampaignModel>> GetByZoneId(int deliveryZoneId, CampaignModel model, int page, int size)
        {
            model.Status = model.Status switch
            {
                "Sắp mở bán" => "0",
                "Đang diễn ra" => "1",
                "Đã kết thúc" => "2",
                "Đã hủy" => "3",
                "0" => "0",
                "1" => "1",
                "2" => "2",
                "3" => "3",
                _ => model.Status = null
            };
            if (deliveryZoneId == 0)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Vui lòng nhập địa chỉ để tiếp tục...!");
            //var zone = await _tradeZoneMapService.CheckZone(campaignZoneId);

            var resultFilter = Get(x => x.Status == (int)CampaignEnum.Đangdiễnra && x.CampaignDeliveryZones.Any(x => x.DeliveryZoneId == deliveryZoneId)).ProjectTo<CampaignModel>(_mapper)
                .DynamicFilter(model)
                .Select<CampaignModel>(CampaignModel.Fields.ToArray().ToDynamicSelector<CampaignModel>())
                .PagingIQueryable(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);
            var listCampaign = await resultFilter.Item2.ToListAsync();
            if (listCampaign.Count == 0)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Khu Vực hiện tại chưa có chiến dịch nào được mở bán!");

            foreach (var campaign in listCampaign)
            {
                campaign.FarmInCampaign = _farmService.Get(x => x.Active && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).Count();
                campaign.Status = campaign.Status switch
                {
                    "0" => "Sắp mở bán",
                    "1" => "Đang diễn ra",
                    "2" => "Đã kết thúc",
                    "3" => "Đã hủy",
                    _ => ""
                };
                campaign.ExpectedDeliveryTime = campaign.EndAt.Value.AddDays(2);
            }

            var result = new DynamicModelsResponse<CampaignModel>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = resultFilter.Item1 },
                Data = listCampaign.Where(x => x.FarmInCampaign > 0).OrderByDescending(y => y.StartAt).ToList()
            };
            return result;
        }

        public async Task<DynamicModelsResponse<CampaignModel>> GetAllCampaign(CampaignModel model, int page, int size)
        {
            model.Status = model.Status switch
            {
                "Sắp mở bán" => "0",
                "Đang diễn ra" => "1",
                "Đã kết thúc" => "2",
                "Đã hủy" => "3",
                "0" => "0",
                "1" => "1",
                "2" => "2",
                "3" => "3",
                _ => model.Status = null
            };

            var resultFilter = Get().ProjectTo<CampaignModel>(_mapper)
                .DynamicFilter(model)
                .Select<CampaignModel>(CampaignModel.Fields.ToArray().ToDynamicSelector<CampaignModel>());
            var listCampaign = await resultFilter.ToListAsync();
            var listFarmInCampaign = new List<FarmMappingCampaignModel>();

            foreach (var campaign in listCampaign)
            {
                campaign.FarmInCampaign = _farmService.Get(x => x.Active && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).Count();
                campaign.Status = campaign.Status switch
                {
                    "0" => "Sắp mở bán",
                    "1" => "Đang diễn ra",
                    "2" => "Đã kết thúc",
                    "3" => "Đã hủy",
                    _ => ""
                };
                campaign.ExpectedDeliveryTime = campaign.EndAt.Value.AddDays(2);
            }

            var listPaging = listCampaign.OrderByDescending(x => x.StartAt).ToList().PagingList(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            var result = new DynamicModelsResponse<CampaignModel>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = listPaging.Item1 },
                Data = listPaging.Item2
            };
            return result;
        }

        public async Task<DynamicModelsResponse<CampaignOfFarmerAppliedModel>> GetCampaignOfFarmerApplied(string farmerId, int page, int size)
        {
            var listCampaign = await Get(x => x.Status == (int)CampaignEnum.Đangdiễnra || x.Status == (int)CampaignEnum.Chờthamgia)
            .ProjectTo<CampaignOfFarmerAppliedModel>(_mapper).ToListAsync();

            foreach (var campaign in listCampaign)
            {

                var farms = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).ProjectTo<FarmApplyModel>(_mapper).ToListAsync();
                campaign.FarmInCampaign = _farmService.Get(x => x.Active && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).Count();

                var timeApplys = new List<DateTime?>();
                foreach (var farm in farms)
                {
                    var harvestCampaigns = _harvestCampaignService.Get(x => x.Status != (int)HarvestCampaignEnum.Đãngừngbán && x.Status != (int)HarvestCampaignEnum.Đãbịtừchối &&
                    x.Harvest.FarmId == farm.Id && x.CampaignId == campaign.Id).ToList();
                    timeApplys.AddRange(harvestCampaigns.Select(x => x.CreateAt).ToList());
                }
                campaign.TimeApply = timeApplys.Min();
                campaign.Farms = farms.Select(x => x.Name).ToList();
                campaign.Status = campaign.Status switch
                {
                    "0" => "Sắp mở bán",
                    "1" => "Đang diễn ra",
                    "2" => "Đã kết thúc",
                    "3" => "Đã hủy",
                    _ => ""
                };
            }

            var resultCampaign = listCampaign.Where(x => x.Farms.Count > 0).ToList();

            var listPaging = resultCampaign.PagingList(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            var result = new DynamicModelsResponse<CampaignOfFarmerAppliedModel>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = listPaging.Item1 },
                Data = listPaging.Item2
            };
            return result;
        }

        public async Task<List<CampaignSearchModel>> SearchCampaignApplied(string farmerId, CampaignSearchModel model)
        {
            var listCampaign = Get().ProjectTo<CampaignSearchModel>(_mapper)
                .DynamicFilter(model)
                .Select<CampaignSearchModel>(CampaignSearchModel.Fields.ToArray().ToDynamicSelector<CampaignSearchModel>())
                .ToList();
            foreach (var campaign in listCampaign)
            {
                var farms = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).ProjectTo<FarmApplyModel>(_mapper).ToListAsync();
                campaign.Farms = farms.Select(x => x.Name).ToList();
                campaign.Status = campaign.Status switch
                {
                    "0" => "Sắp mở bán",
                    "1" => "Đang diễn ra",
                    "2" => "Đã kết thúc",
                    "3" => "Đã hủy",
                    _ => ""
                };
            }
            var resultCampaign = listCampaign.Where(x => x.Farms.Count > 0).ToList();
            return resultCampaign;
        }

        public async Task<DynamicModelsResponse<CampaignOfFarmerApplyModel>> GetCampaignOfFarmerApply(string farmerId, string type, int page, int size)
        {
            var listCampaign = new List<CampaignOfFarmerApplyModel>();
            if (type == null)
                listCampaign = await Get(x => x.Status == (int)CampaignEnum.Chờthamgia).ProjectTo<CampaignOfFarmerApplyModel>(_mapper).ToListAsync();
            else
                listCampaign = await Get(x => x.Status == (int)CampaignEnum.Chờthamgia && x.Type.ToLower() == type.ToLower()).ProjectTo<CampaignOfFarmerApplyModel>(_mapper).ToListAsync();
            foreach (var campaign in listCampaign)
            {
                var farms = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId && x.ProductHarvests.Any(y => y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.CampaignId == campaign.Id))).ProjectTo<FarmNameModel>(_mapper).ToListAsync();
                var listFarm = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId).ProjectTo<FarmNameModel>(_mapper).ToListAsync();
                foreach (var farm in farms)
                    listFarm.Remove(listFarm.Where(x => farm.Name.Contains(x.Name)).FirstOrDefault());

                campaign.FarmInCampaign = _farmService.Get(x => x.Active && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).Count();
                campaign.Farms = listFarm.Select(x => x.Name).ToList();
                campaign.Status = campaign.Status switch
                {
                    "0" => "Sắp mở bán",
                    "1" => "Đang diễn ra",
                    "2" => "Đã kết thúc",
                    "3" => "Đã hủy",
                    _ => ""
                };
            }

            var resultCampaign = listCampaign.Where(x => x.Farms.Count > 0).ToList();

            var listPaging = resultCampaign.OrderBy(x => x.Type).ToList().PagingList(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);

            var result = new DynamicModelsResponse<CampaignOfFarmerApplyModel>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = listPaging.Item1 },
                Data = listPaging.Item2
            };
            return result;
        }

        public async Task<List<CampaignSearchModel>> SearchCampaignApply(string farmerId, CampaignSearchModel model)
        {
            var listCampaign = Get(x => x.Status == (int)CampaignEnum.Chờthamgia).ProjectTo<CampaignSearchModel>(_mapper)
                .DynamicFilter(model)
                .Select<CampaignSearchModel>(CampaignSearchModel.Fields.ToArray().ToDynamicSelector<CampaignSearchModel>())
                .ToList();
            foreach (var campaign in listCampaign)
            {
                var farms = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).ProjectTo<FarmApplyModel>(_mapper).ToListAsync();
                var listFarm = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId).ProjectTo<FarmNameModel>(_mapper).ToListAsync();
                foreach (var farm in farms)
                    listFarm.Remove(listFarm.Where(x => farm.Name.Contains(x.Name)).FirstOrDefault());

                campaign.Farms = listFarm.Select(x => x.Name).ToList();
                campaign.Status = campaign.Status switch
                {
                    "0" => "Sắp mở bán",
                    "1" => "Đang diễn ra",
                    "2" => "Đã kết thúc",
                    "3" => "Đã hủy",
                    _ => ""
                };
            }
            var resultCampaign = listCampaign.Where(x => x.Farms.Count > 0).ToList();
            return resultCampaign;
        }

        public async Task<CampaignFarmZoneModel> GetById(int id)
        {
            var campaign = await Get(x => x.Id == id).ProjectTo<CampaignFarmZoneModel>(_mapper).FirstOrDefaultAsync();
            if (campaign == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy");

            campaign.FarmInCampaign = _farmService.Get(x => x.Active && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).Count();
            campaign.Status = campaign.Status switch
            {
                "0" => "Sắp mở bán",
                "1" => "Đang diễn ra",
                "2" => "Đã kết thúc",
                "3" => "Đã hủy",
                _ => ""
            };
            campaign.ExpectedDeliveryTime = campaign.EndAt.Value.AddDays(2);
            return campaign;
        }

        public async Task<CampaignDetailApplyModel> GetCampaignDetailApply(int campaignId)
        {
            var campaign = await Get(x => x.Id == campaignId).ProjectTo<CampaignDetailApplyModel>(_mapper).FirstOrDefaultAsync();
            if (campaign == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy");

            campaign.FarmInCampaign = _farmService.Get(x => x.Active && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).Count();
            var campaignDeliveryzones = _campaignDeliveryZoneService.Get(x => x.CampaignId == campaign.Id).ProjectTo<CampaignDeliveryZoneModel>(_mapper).ToList();
            campaign.DeliveryZoneName = campaignDeliveryzones.Select(x => x.DeliveryZoneName).ToList();

            foreach (var productSales in campaign.ProductSalesCampaigns)
            {
                var data = _productSalesCampaignService.Get(x => x.Id == productSales.Id).ProjectTo<ProductSalesCampaignViewModel>(_mapper).FirstOrDefault();
                productSales.ProductName = data.ProductSystem.Name;
                productSales.Unit = data.ProductSystem.Unit;
            }
            campaign.Status = campaign.Status switch
            {
                "0" => "Sắp mở bán",
                "1" => "Đang diễn ra",
                "2" => "Đã kết thúc",
                "3" => "Đã hủy",
                _ => ""
            };
            return campaign;
        }

        public async Task<CampaignDetailApplieModel> GetCampaignDetailApplied(string farmerId, int campaignId)
        {
            var campaign = await Get(x => x.Id == campaignId).ProjectTo<CampaignDetailApplieModel>(_mapper).FirstOrDefaultAsync();
            if (campaign == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy");
            campaign.FarmInCampaign = _farmService.Get(x => x.Active && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).Count();
            var farms = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId && x.ProductHarvests.Any(y => y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.CampaignId == campaign.Id))).ProjectTo<FarmApplyModel>(_mapper).ToListAsync();
            campaign.Farms = farms;
            var timeApplys = new List<DateTime?>();
            foreach (var farm in farms)
            {
                var harvestCampaigns = _harvestCampaignService.Get(x => x.Status != (int)HarvestCampaignEnum.Đãngừngbán && x.Status != (int)HarvestCampaignEnum.Đãbịtừchối &&
                x.Harvest.FarmId == farm.Id && x.CampaignId == campaign.Id).ToList();
                timeApplys.AddRange(harvestCampaigns.Select(x => x.CreateAt).ToList());
            }
            campaign.TimeApply = timeApplys.Min();

            var campaignDeliveryzones = _campaignDeliveryZoneService.Get(x => x.CampaignId == campaign.Id).ProjectTo<CampaignDeliveryZoneModel>(_mapper).ToList();
            campaign.DeliveryZoneName = campaignDeliveryzones.Select(x => x.DeliveryZoneName).ToList();
            foreach (var productSale in campaign.ProductSalesCampaigns)
            {
                var data = _productSalesCampaignService.Get(x => x.Id == productSale.Id).ProjectTo<ProductSalesCampaignViewModel>(_mapper).FirstOrDefault();
                productSale.Unit = data.ProductSystem.Unit;
                productSale.ProductName = data.ProductSystem.Name;
            }
            campaign.Status = campaign.Status switch
            {
                "0" => "Sắp mở bán",
                "1" => "Đang diễn ra",
                "2" => "Đã kết thúc",
                "3" => "Đã hủy",
                _ => ""
            };

            return campaign;
        }

        public async Task<CampaignCreateModel> Create(CampaignCreateInputModel modelInput)
        {
            if (Get(x => x.Name == modelInput.Name && x.Status == (int)CampaignEnum.Đangdiễnra).Any())
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Chiến dịch đang mở!");
            if (modelInput.Images == null)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Ảnh không được để trống!");

            var zones = await _tradeZoneMapService.GetListZone();

            var campaignDeliveryZones = new List<CampaignDeliveryZoneCreateModel>();
            foreach (var deliveryZone in modelInput.DeliveryZoneId)
                campaignDeliveryZones.Add(new CampaignDeliveryZoneCreateModel { DeliveryZoneId = deliveryZone });

            var model = new CampaignCreateModel
            {
                Description = modelInput.Description,
                Name = modelInput.Name,
                CampaignZoneId = modelInput.CampaignZoneId,
                Type = modelInput.Type,
                StartAt = modelInput.StartAt,
                StartRecruitmentAt = modelInput.StartAt,
                EndAt = modelInput.EndAt,
                EndRecruitmentAt = modelInput.StartAt.Value.AddDays(3),
                CampaignDeliveryZones = campaignDeliveryZones,
                ProductSalesCampaigns = modelInput.ProductSalesCampaigns,
                CampaignZoneName = zones.Where(x => x.Id == modelInput.CampaignZoneId).FirstOrDefault().Name
            };

            if (modelInput.Images.Count == 1)
                model.Image1 = modelInput.Images[0];
            else if (modelInput.Images.Count == 2)
            {
                model.Image1 = modelInput.Images[0];
                model.Image2 = modelInput.Images[1];
            }
            else if (modelInput.Images.Count == 3)
            {
                model.Image1 = modelInput.Images[0];
                model.Image2 = modelInput.Images[1];
                model.Image3 = modelInput.Images[2];
            }
            else if (modelInput.Images.Count == 4)
            {
                model.Image1 = modelInput.Images[0];
                model.Image2 = modelInput.Images[1];
                model.Image3 = modelInput.Images[2];
                model.Image4 = modelInput.Images[3];
            }
            else if (modelInput.Images.Count == 5)
            {
                model.Image1 = modelInput.Images[0];
                model.Image2 = modelInput.Images[1];
                model.Image3 = modelInput.Images[2];
                model.Image4 = modelInput.Images[3];
                model.Image5 = modelInput.Images[4];
            }

            var entity = _mapper.CreateMapper().Map<Campaign>(model);
            foreach (var campaignDeliveryZone in entity.CampaignDeliveryZones)
            {
                campaignDeliveryZone.CampaignId = entity.Id;
                campaignDeliveryZone.DeliveryZoneName = zones.Where(x => x.Id == campaignDeliveryZone.DeliveryZoneId).FirstOrDefault().Name;
            }

            foreach (var productSales in entity.ProductSalesCampaigns)
                productSales.CampaignId = entity.Id;
            await CreateAsyn(entity);
            return model;
        }

        public async Task<Campaign> Update(int id, CampaignUpdateInputModel modelInput)
        {
            var entity = await GetAsyn(id);
            var farm = _farmService.Get(x => x.Active && x.ProductHarvests.Any(y => y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.CampaignId == id))).FirstOrDefault();
            if (entity == null || entity.Status == (int)CampaignEnum.Đãkếtthúc)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            if (farm != null)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Chiến dịch này đã có nông trại tham gia!");

            var campaignDeliverys = _campaignDeliveryZoneService.Get(x => x.CampaignId == entity.Id).ToList();
            _campaignDeliveryZoneService.RemoveRange(campaignDeliverys);
            var productSales = _productSalesCampaignService.Get(x => x.CampaignId == entity.Id).ToList();
            _productSalesCampaignService.RemoveRange(productSales);

            var zones = await _tradeZoneMapService.GetListZone();

            var campaignDeliveryZones = new List<CampaignDeliveryZoneCreateModel>();
            foreach (var deliveryZone in modelInput.DeliveryZoneId)
                campaignDeliveryZones.Add(new CampaignDeliveryZoneCreateModel { DeliveryZoneId = deliveryZone });

            var model = new CampaignUpdateModel
            {
                Description = modelInput.Description,
                Name = modelInput.Name,
                CampaignZoneId = modelInput.CampaignZoneId,
                Type = modelInput.Type,
                Id = modelInput.Id,
                CampaignDeliveryZones = campaignDeliveryZones,
                ProductSalesCampaigns = modelInput.ProductSalesCampaigns,
                CampaignZoneName = zones.Where(x => x.Id == modelInput.CampaignZoneId).FirstOrDefault().Name
        };

            if (modelInput.Images.Count == 1)
                model.Image1 = modelInput.Images[0];
            else if (modelInput.Images.Count == 2)
            {
                model.Image1 = modelInput.Images[0];
                model.Image2 = modelInput.Images[1];
            }
            else if (modelInput.Images.Count == 3)
            {
                model.Image1 = modelInput.Images[0];
                model.Image2 = modelInput.Images[1];
                model.Image3 = modelInput.Images[2];
            }
            else if (modelInput.Images.Count == 4)
            {
                model.Image1 = modelInput.Images[0];
                model.Image2 = modelInput.Images[1];
                model.Image3 = modelInput.Images[2];
                model.Image4 = modelInput.Images[3];
            }
            else if (modelInput.Images.Count == 5)
            {
                model.Image1 = modelInput.Images[0];
                model.Image2 = modelInput.Images[1];
                model.Image3 = modelInput.Images[2];
                model.Image4 = modelInput.Images[3];
                model.Image5 = modelInput.Images[4];
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

            foreach (var campaignDeliveryZone in updateEntity.CampaignDeliveryZones)
            {
                campaignDeliveryZone.CampaignId = updateEntity.Id;
                campaignDeliveryZone.DeliveryZoneName = zones.Where(x => x.Id == campaignDeliveryZone.DeliveryZoneId).FirstOrDefault().Name;
            }

            foreach (var productSalesCampaign in updateEntity.ProductSalesCampaigns)
                productSalesCampaign.CampaignId = updateEntity.Id;
            await UpdateAsyn(updateEntity);
            return updateEntity;
        }
        public async Task<Campaign> Delete(int id, string note)
        {
            var entity = Get(id);
            var harvestCampaign = _harvestCampaignService.Get(x => x.CampaignId == id).FirstOrDefault();
            if (entity == null || entity.Status == (int)CampaignEnum.Đãhủy)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            if (harvestCampaign != null)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Chiến dịch này đang bán hàng!");
            entity.Status = (int)CampaignEnum.Đãhủy;
            entity.Note = note;
            await UpdateAsyn(entity);
            return entity;
        }

        public async Task<List<CampaignApplyRequestModel>> GetApplyRequest()
        {
            var campaigns = await Get(x => x.Status == (int)CampaignEnum.Chờthamgia).ProjectTo<CampaignApplyRequestModel>(_mapper).ToListAsync();

            foreach (var campaign in campaigns)
            {
                var farms = await _farmService.Get(x => x.Active && x.ProductHarvests.Any(x => x.ProductHarvestInCampaigns.Any(x => x.CampaignId == campaign.Id))).ProjectTo<FarmMappingHarvestInCampaign>(_mapper).ToListAsync();
                foreach (var farm in farms)
                    farm.HarvestApplyRequest = await _harvestCampaignService.Get(x => x.Status == (int)HarvestCampaignEnum.Chờxácnhận && farm.Id == x.Harvest.FarmId && x.CampaignId == campaign.Id).ProjectTo<HarvestCampaignApplyRequest>(_mapper).CountAsync();
                campaign.FarmApplyRequest = farms.Where(x => x.HarvestApplyRequest > 0).Count();
            }
            var listCampaign = campaigns.Where(x => x.FarmApplyRequest > 0).ToList();

            return listCampaign;
        }

        public async Task<DynamicModelsResponse<FarmMappingHarvestInCampaign>> GetFarmApplyRequest(int campaignId, int page, int size)
        {
            var farms = await _farmService.Get(x => x.Active && x.ProductHarvests.Any(x => x.ProductHarvestInCampaigns.Any(x => x.CampaignId == campaignId))).ProjectTo<FarmMappingHarvestInCampaign>(_mapper).ToListAsync();

            foreach (var farm in farms)
                farm.HarvestApplyRequest = await _harvestCampaignService.Get(x => x.Status == (int)HarvestCampaignEnum.Chờxácnhận && farm.Id == x.Harvest.FarmId && x.CampaignId == campaignId).ProjectTo<HarvestCampaignApplyRequest>(_mapper).CountAsync();
            var listPaging = farms.Where(x => x.HarvestApplyRequest > 0).ToList().PagingList(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);
            var result = new DynamicModelsResponse<FarmMappingHarvestInCampaign>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = listPaging.Item1 },
                Data = listPaging.Item2
            };
            return result;
        }

        public async Task<DynamicModelsResponse<HarvestCampaignApplyRequest>> GetHarvestApplyRequest(int campaignId, int farmId, int page, int size)
        {
            var harvestInCampaigns = await _harvestCampaignService.Get(x => x.Status == (int)HarvestCampaignEnum.Chờxácnhận && farmId == x.Harvest.FarmId && x.CampaignId == campaignId).ProjectTo<HarvestCampaignApplyRequest>(_mapper).ToListAsync();

            foreach (var harvestInCampaign in harvestInCampaigns)
            {
                harvestInCampaign.Key = harvestInCampaign.Id;
                harvestInCampaign.UnitOfSystem = harvestInCampaign.Harvest.ProductSystem.Unit;
                switch (harvestInCampaign.Status)
                {
                    case "0":
                        harvestInCampaign.Status = "Chờ xác nhận";
                        break;
                    case "1":
                        harvestInCampaign.Status = "Đã xác nhận";
                        break;
                    case "2":
                        harvestInCampaign.Status = "Đã hết hàng";
                        break;
                    case "3":
                        harvestInCampaign.Status = "Đã bị từ chối";
                        break;
                    case "4":
                        harvestInCampaign.Status = "Đã hủy";
                        break;
                }
            }

            var listPaging = harvestInCampaigns.PagingList(page, size, CommonConstants.LimitPaging, CommonConstants.DefaultPaging);
            var result = new DynamicModelsResponse<HarvestCampaignApplyRequest>
            {
                Metadata = new PagingMetadata { Page = page, Size = size, Total = listPaging.Item1 },
                Data = listPaging.Item2
            };

            return result;
        }

        public async Task<List<FarmApplyModel>> GetFarmsCanJoinCampaign(string farmerId, int campaignId)
        {
            var campaign = Get(x => x.Id == campaignId).FirstOrDefault();
            var farms = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId && x.ProductHarvests.Any(y => y.Active && y.ProductHarvestInCampaigns.Count > 0 && y.ProductHarvestInCampaigns.Any(z => z.Status != (int)HarvestCampaignEnum.Đãbịtừchối && z.CampaignId == campaign.Id))).ProjectTo<FarmApplyModel>(_mapper).ToListAsync();
            var listFarm = await _farmService.Get(x => x.Active && x.FarmZoneId == campaign.CampaignZoneId && x.FarmerId == farmerId).ProjectTo<FarmApplyModel>(_mapper).ToListAsync();
            foreach (var farm in farms)
                listFarm.Remove(listFarm.Where(x => farm.Name.Contains(x.Name)).FirstOrDefault());
            return listFarm;
        }

        public async Task<List<HarvestApplyModel>> GetHarvestApplyOfFarm(int farmId, int campaignId)
        {
            var harvests = await _harvestService.Get(x => x.Active && x.Farm.Id == farmId && x.InventoryTotal > 0 && x.ProductSystem.ProductSalesCampaigns.Any(y => y.CampaignId == campaignId)).ProjectTo<HarvestApplyModel>(_mapper).ToListAsync();
            return harvests;
        }

        public async Task<HarvestDetailModel> GetHarvestDetail(int harvestId)
        {
            var harvest = await _harvestService.Get(x => x.Active && x.Id == harvestId).ProjectTo<HarvestDetailModel>(_mapper).FirstOrDefaultAsync();
            if (harvest == null)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Không tồn tại!");
            if (harvest.InventoryTotal == 0)
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Mùa vụ này không còn sản phẩm nào!");
            return harvest;
        }

        public async Task<int> CountCampaignApply(string farmerId, string type)
        {
            var search = new CampaignSearchModel();
            var campaigns = await SearchCampaignApply(farmerId, search);
            if (type != null)
            {
                var result = campaigns.Where(x => x.Type.ToLower() == type.ToLower()).ToList();
                return result.Count;
            } else
                return campaigns.Count;
        }

        public async Task<int> CountCampaignApplied(string farmerId)
        {
            var search = new CampaignSearchModel();
            var campaigns = await SearchCampaignApplied(farmerId, search);
            return campaigns.Count;
        }

        public async Task<DashBoardOfFarmer> DashBoard(string farmerId)
        {
            var farms = _farmService.Get(x => x.Active && x.FarmerId == farmerId).Count();
            var harvests = _harvestService.Get(x => x.Active && x.Farm.FarmerId == farmerId).Count();
            var orderConfirms = _farmOrderService.Get(x => x.Status == (int)FarmOrderEnum.Chờxácnhận && x.Farm.FarmerId == farmerId).Count();
            var farmOrders = await _farmOrderService.Get(x => x.Status == (int)FarmOrderEnum.Đãhoànthành && x.Farm.FarmerId == farmerId).ProjectTo<FarmOrderMapOrderDataModel>(_mapper).ToListAsync();

            var customerIds = new List<CustomerOrder>();
            foreach (var farmOrder in farmOrders)
            {
                switch (customerIds.Count)
                {
                    case 0:
                        customerIds.Add(new CustomerOrder { Id = farmOrder.Order.CustomerId });
                        break;
                    case > 0 when customerIds.Any(x => x.Id == farmOrder.Order.CustomerId):
                        customerIds.Remove(new CustomerOrder { Id = farmOrder.Order.CustomerId });
                        break;
                    default:
                        customerIds.Add(new CustomerOrder { Id = farmOrder.Order.CustomerId });
                        break;
                }
            }

            return new DashBoardOfFarmer {
                Farms = farms,
                Harvests = harvests,
                OrderConfirms = orderConfirms,
                CustomerOrder = customerIds.Count
            };
        }
    }
}
