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
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiCho.DataService.Services
{
    public partial interface IItemCartService
    {
        Task<ItemCartViewModel> Gets(string customerId);
        Task<ItemCartCreateInputModel> Create(ItemCartCreateInputModel model);
        Task Update(string customerId, int harvestCampaignId, int quantity);
        Task Delete(string customerId, int harvestCampaignId);
        Task DeleteCart(string customerId);
        Task<int> Count(string customerId);
    }
    public partial class ItemCartService : IItemCartService
    {
        private readonly IConfigurationProvider _mapper;
        private readonly ICampaignService _campaignService;
        private readonly IProductHarvestInCampaignService _harvestCampaignService;
        private readonly IRedisCacheClient _redisCacheClient;

        public ItemCartService(ICampaignService campaignService, IProductHarvestInCampaignService harvestCampaignService, IRedisCacheClient redisCacheClient,
            IMapper mapper = null)
        {
            _mapper = mapper.ConfigurationProvider;
            _campaignService = campaignService;
            _harvestCampaignService = harvestCampaignService;
            _redisCacheClient = redisCacheClient;
        }

        public async Task<ItemCartViewModel> Gets(string customerId)
        {
            var itemCarts = new ItemCartViewModel();
            var itemCartsRedis = await _redisCacheClient.Db1.GetAsync<List<ItemCartCreateModel>>("MyList");

            var cart = itemCartsRedis.Where(x => x.CustomerId == customerId).ToList();

            if ((cart?.Count ?? 0) <= 0)
                return new ItemCartViewModel();

            var harvestCampaign = new HarvestCampaignMapItemCartModel();
            var farms = new List<FarmItemCartViewModel>();
            foreach (var item in cart)
            {
                harvestCampaign = _harvestCampaignService.Get(x => x.Id == item.HarvestCampaignId).ProjectTo<HarvestCampaignMapItemCartModel>(_mapper).FirstOrDefault();
                var farm = harvestCampaign.Harvest;
                if (farms.Count == 0)
                    farms.Add(farm.Farm);
                else if (farms.Count > 0 && farms.Where(x => x.Id == farm.FarmId).Count() != 0)
                    farms.Remove(farm.Farm);
                else
                    farms.Add(farm.Farm);
            }

            var campaign = _campaignService.Get(x => x.Id == harvestCampaign.CampaignId).FirstOrDefault();

            itemCarts.CampaignId = campaign.Id;
            itemCarts.CampaignName = campaign.Name;
            itemCarts.ExpectedDeliveryTime = campaign.EndAt.Value.AddDays(2);
            itemCarts.Checked = false;
            itemCarts.Farms = farms;
            foreach (var farm in itemCarts.Farms)
            {
                var harvestCampaigns = new List<HarvestCampaignItemCartViewModel>();
                foreach (var item in cart)
                {
                    var harvestCampaignAdd = _harvestCampaignService.Get(x => x.Id == item.HarvestCampaignId).ProjectTo<HarvestCampaignMapItemCartModel>(_mapper).FirstOrDefault();
                    if (farm.Id == harvestCampaignAdd.Harvest.FarmId)
                    {
                        harvestCampaigns.Add(new HarvestCampaignItemCartViewModel
                        {
                            ItemCartId = item.Id,
                            Id = item.HarvestCampaignId,
                            ProductName = harvestCampaignAdd.ProductName,
                            Image = harvestCampaignAdd.Harvest.Image1,
                            Price = harvestCampaignAdd.Price,
                            Quantity = item.Quantity,
                            Unit = harvestCampaignAdd.Unit,
                            ValueChangeOfUnit = harvestCampaignAdd.ValueChangeOfUnit,
                            Total = item.Quantity * harvestCampaignAdd.Price,
                            Checked = false,
                            MaxQuantity = harvestCampaignAdd.Quantity
                        });
                    }
                }
                farm.HarvestInCampaigns = harvestCampaigns;
            }
            return itemCarts;
        }

        public async Task<ItemCartCreateInputModel> Create(ItemCartCreateInputModel model)
        {
            var itemCarts = new List<ItemCartCreateModel>();
            var itemCartsRedis = await _redisCacheClient.Db1.GetAsync<List<ItemCartCreateModel>>("MyList");

            var cartRedis = itemCartsRedis.Where(x => x.CustomerId == model.CustomerId).ToList();
            var cart = itemCartsRedis.Where(x => x.CustomerId == model.CustomerId && x.HarvestCampaignId == model.HarvestCampaignId).ToList();
            int? campaignId = 0;
            if ((cartRedis?.Count ?? 0) > 0)
            {
                foreach (var item in cartRedis)
                {
                    campaignId = _harvestCampaignService.Get(x => x.Id == item.HarvestCampaignId).FirstOrDefault().CampaignId;
                }
            }
            var campaignIdInput = _harvestCampaignService.Get(x => x.Id == model.HarvestCampaignId).FirstOrDefault().CampaignId;
            if (campaignId != campaignIdInput && campaignId != 0)
            {
                var harvestCampaign = _harvestCampaignService.Get(x => x.Id == model.HarvestCampaignId).FirstOrDefault();
                if (harvestCampaign.Quantity < model.Quantity)
                    throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{harvestCampaign.ProductName} Chỉ còn {harvestCampaign.Quantity} ({harvestCampaign.Unit})!!");
                var idCount = 0;
                foreach (var item in cartRedis)
                    itemCartsRedis.Remove(item);
                foreach (var itemCartRedis in itemCartsRedis)
                {
                    idCount += 1;
                    itemCarts.Add(new ItemCartCreateModel
                    {
                        CustomerId = itemCartRedis.CustomerId,
                        HarvestCampaignId = itemCartRedis.HarvestCampaignId,
                        Id = idCount,
                        Quantity = itemCartRedis.Quantity,
                    });
                }
                itemCarts.Add(new ItemCartCreateModel
                {
                    CustomerId = model.CustomerId,
                    HarvestCampaignId = model.HarvestCampaignId,
                    Id = idCount,
                    Quantity = model.Quantity,
                });
                await _redisCacheClient.Db1.AddAsync<List<ItemCartCreateModel>>("MyList", itemCarts);
            }
            else if ((cart?.Count ?? 0) > 0)
            {
                await UpdateTask(model.CustomerId, model.HarvestCampaignId, model.Quantity);
            }
            else
            {
                var id = 0;
                if ((itemCartsRedis?.Count ?? 0) > 0)
                {
                    var harvestCampaign = _harvestCampaignService.Get(x => x.Id == model.HarvestCampaignId).FirstOrDefault();
                    if (harvestCampaign.Quantity < model.Quantity)
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{harvestCampaign.ProductName} Chỉ còn {harvestCampaign.Quantity} ({harvestCampaign.Unit})!!");
                    foreach (var itemCartRedis in itemCartsRedis)
                    {
                        id += 1;
                        itemCarts.Add(new ItemCartCreateModel
                        {
                            CustomerId = itemCartRedis.CustomerId,
                            HarvestCampaignId = itemCartRedis.HarvestCampaignId,
                            Id = id,
                            Quantity = itemCartRedis.Quantity,
                        });
                    }
                    itemCarts.Add(new ItemCartCreateModel
                    {
                        CustomerId = model.CustomerId,
                        HarvestCampaignId = model.HarvestCampaignId,
                        Quantity = model.Quantity,
                        Id = id + 1
                    });

                    await _redisCacheClient.Db1.AddAsync<List<ItemCartCreateModel>>("MyList", itemCarts);
                }
                else
                {
                    var harvestCampaign = _harvestCampaignService.Get(x => x.Id == model.HarvestCampaignId).FirstOrDefault();
                    if (harvestCampaign.Quantity < model.Quantity)
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{harvestCampaign.ProductName} Chỉ còn {harvestCampaign.Quantity} ({harvestCampaign.Unit})!!");
                    itemCarts.Add(new ItemCartCreateModel
                    {
                        Id = 1,
                        CustomerId = model.CustomerId,
                        HarvestCampaignId = model.HarvestCampaignId,
                        Quantity = model.Quantity
                    });
                    await _redisCacheClient.Db1.AddAsync("MyList", itemCarts);
                }
            }

            return model;
        }

        public async Task Update(string customerId, int harvestCampaignId, int quantity)
        {
            var itemCarts = new List<ItemCartCreateModel>();
            var itemCartsRedis = await _redisCacheClient.Db1.GetAsync<List<ItemCartCreateModel>>("MyList");

            var cart = itemCartsRedis.Where(x => x.CustomerId == customerId && x.HarvestCampaignId == harvestCampaignId).ToList();
            if (cart.Count == 0)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            var idCount = 0;
            if ((cart?.Count ?? 0) > 0)
            {
                foreach (var item in cart)
                {
                    var harvestCampaign = _harvestCampaignService.Get(x => x.Id == item.HarvestCampaignId).FirstOrDefault();
                    if (harvestCampaign.Quantity < quantity)
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{harvestCampaign.ProductName} Chỉ còn {harvestCampaign.Quantity} ({harvestCampaign.Unit})!!");
                    itemCartsRedis.Remove(item);
                    itemCarts.Add(new ItemCartCreateModel
                    {
                        CustomerId = item.CustomerId,
                        HarvestCampaignId = item.HarvestCampaignId,
                        Quantity = quantity,
                        Id = item.Id
                    });
                    foreach (var itemCartRedis in itemCartsRedis)
                    {
                        idCount += 1;
                        if (item.Id > idCount)
                        {
                            itemCarts.Add(new ItemCartCreateModel
                            {
                                CustomerId = itemCartRedis.CustomerId,
                                HarvestCampaignId = itemCartRedis.HarvestCampaignId,
                                Id = idCount,
                                Quantity = itemCartRedis.Quantity,
                            });
                        }
                        else
                        {
                            var value = idCount + 1;
                            itemCarts.Add(new ItemCartCreateModel
                            {
                                CustomerId = itemCartRedis.CustomerId,
                                HarvestCampaignId = itemCartRedis.HarvestCampaignId,
                                Id = value,
                                Quantity = itemCartRedis.Quantity,
                            });
                        }
                    }
                }

                await _redisCacheClient.Db1.AddAsync<List<ItemCartCreateModel>>("MyList", itemCarts);
            }
        }

        public async Task Delete(string customerId, int harvestCampaignId)
        {
            var itemCarts = new List<ItemCartCreateModel>();
            var itemCartsRedis = await _redisCacheClient.Db1.GetAsync<List<ItemCartCreateModel>>("MyList");

            var cart = itemCartsRedis.Where(x => x.CustomerId == customerId && x.HarvestCampaignId == harvestCampaignId).ToList();
            if (cart == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");

            var idCount = 0;
            if ((cart?.Count ?? 0) > 0)
            {
                foreach (var item in cart)
                {
                    itemCartsRedis.Remove(item);
                    foreach (var itemCartRedis in itemCartsRedis)
                    {
                        idCount += 1;

                        if (item.Id > idCount)
                        {
                            itemCarts.Add(new ItemCartCreateModel
                            {
                                CustomerId = itemCartRedis.CustomerId,
                                HarvestCampaignId = itemCartRedis.HarvestCampaignId,
                                Id = idCount,
                                Quantity = itemCartRedis.Quantity,
                            });
                        }
                        else
                        {
                            var value = idCount + 1;
                            itemCarts.Add(new ItemCartCreateModel
                            {
                                CustomerId = itemCartRedis.CustomerId,
                                HarvestCampaignId = itemCartRedis.HarvestCampaignId,
                                Id = value,
                                Quantity = itemCartRedis.Quantity,
                            });
                        }
                    }
                }

                await _redisCacheClient.Db1.AddAsync<List<ItemCartCreateModel>>("MyList", itemCarts);
            }
        }

        public async Task DeleteCart(string customerId)
        {
            var itemCarts = new List<ItemCartCreateModel>();
            var itemCartsRedis = await _redisCacheClient.Db1.GetAsync<List<ItemCartCreateModel>>("MyList");

            var cart = itemCartsRedis.Where(x => x.CustomerId == customerId).ToList();
            if (cart == null)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");

            var idCount = 0;
            if ((cart?.Count ?? 0) > 0)
            {
                foreach (var item in cart)
                    itemCartsRedis.Remove(item);
                foreach (var itemCartRedis in itemCartsRedis)
                {
                    idCount += 1;
                    itemCarts.Add(new ItemCartCreateModel
                    {
                        CustomerId = itemCartRedis.CustomerId,
                        HarvestCampaignId = itemCartRedis.HarvestCampaignId,
                        Id = idCount,
                        Quantity = itemCartRedis.Quantity,
                    });
                }
                await _redisCacheClient.Db1.AddAsync<List<ItemCartCreateModel>>("MyList", itemCarts);
            }
        }

        public async Task<int> Count(string customerId)
        {
            var itemCartsRedis = await _redisCacheClient.Db1.GetAsync<List<ItemCartCreateModel>>("MyList");

            var countItemCart = itemCartsRedis.Where(x => x.CustomerId == customerId).Count();
            return countItemCart;
        }

        private async Task UpdateTask(string customerId, int harvestCampaignId, int quantity)
        {
            var itemCarts = new List<ItemCartCreateModel>();
            var itemCartsRedis = await _redisCacheClient.Db1.GetAsync<List<ItemCartCreateModel>>("MyList");

            var cart = itemCartsRedis.Where(x => x.CustomerId == customerId && x.HarvestCampaignId == harvestCampaignId).ToList();
            if (cart.Count == 0)
                throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
            var idCount = 0;
            if ((cart?.Count ?? 0) > 0)
            {
                foreach (var item in cart)
                {
                    quantity += item.Quantity;
                    var harvestCampaign = _harvestCampaignService.Get(x => x.Id == item.HarvestCampaignId).FirstOrDefault();
                    if (harvestCampaign.Quantity < quantity)
                        throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{harvestCampaign.ProductName} Chỉ còn {harvestCampaign.Quantity} ({harvestCampaign.Unit})!!");
                    itemCartsRedis.Remove(item);
                    itemCarts.Add(new ItemCartCreateModel
                    {
                        CustomerId = item.CustomerId,
                        HarvestCampaignId = item.HarvestCampaignId,
                        Quantity = quantity,
                        Id = item.Id
                    });
                    foreach (var itemCartRedis in itemCartsRedis)
                    {
                        idCount += 1;
                        if (item.Id > idCount)
                        {
                            itemCarts.Add(new ItemCartCreateModel
                            {
                                CustomerId = itemCartRedis.CustomerId,
                                HarvestCampaignId = itemCartRedis.HarvestCampaignId,
                                Id = idCount,
                                Quantity = itemCartRedis.Quantity,
                            });
                        }
                        else
                        {
                            var value = idCount + 1;
                            itemCarts.Add(new ItemCartCreateModel
                            {
                                CustomerId = itemCartRedis.CustomerId,
                                HarvestCampaignId = itemCartRedis.HarvestCampaignId,
                                Id = value,
                                Quantity = itemCartRedis.Quantity,
                            });
                        }
                    }
                }

                await _redisCacheClient.Db1.AddAsync<List<ItemCartCreateModel>>("MyList", itemCarts);
            }
        }

        //public async Task<List<CartInCampaignModel>> Gets(string customerId)
        //{
        //    var listItemCart = await Get(x => x.CustomerId == customerId).ProjectTo<ItemCartModel>(_mapper).ToListAsync();
        //    var listCampaign = await _campaignService.Get(x => x.HarvestCampaigns.Count > 0 && x.HarvestCampaigns.Any(y => y.ItemCarts.Any(z => z.CustomerId == customerId))).ProjectTo<CartInCampaignModel>(_mapper).ToListAsync();
        //    var listHarvestCampaign = await _harvestCampaignService.Get(x => x.ItemCarts.Count > 0 && x.ItemCarts.Any(x => x.CustomerId == customerId)).ProjectTo<HarvestCampaignMappingCartModel>(_mapper).ToListAsync();
        //    var listCheck = new List<HarvestCampaignMappingCartModel>();
        //    var listItem = new List<ItemCartModel>();
        //    var result = new List<CartInCampaignModel>().Distinct().ToList();
        //    foreach (var campaign in listCampaign)
        //    {
        //        foreach (var harvestCampaign in listHarvestCampaign)
        //        {
        //            if (harvestCampaign.CampaignId == campaign.Id)
        //            {
        //                foreach (var item in listItemCart)
        //                {
        //                    if (harvestCampaign.Id == item.HarvestCampaignId)
        //                    {
        //                        item.Total = harvestCampaign.Price * item.Quantity;
        //                        listItem.Add(item);
        //                    }
        //                    else
        //                        listItem.Remove(item);
        //                }
        //                listCheck.Add(harvestCampaign);
        //                harvestCampaign.ItemCarts = listItem.Distinct().ToList();
        //            }
        //            else
        //                listCheck.Remove(harvestCampaign);
        //        }
        //        campaign.HarvestCampaigns = listCheck.Distinct().ToList();
        //        result.Add(campaign);
        //    }
        //    return result;
        //}

        //public async Task<ItemCartCreateInputModel> Create(ItemCartCreateInputModel model)
        //{
        //    var harvestCampaign = _harvestCampaignService.Get(x => x.Id == model.HarvestCampaignId).FirstOrDefault();
        //    if (harvestCampaign.Quantity < model.Quantity)
        //        throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{harvestCampaign.ProductName} đã hết hàng!");
        //    var cart = Get(x => x.HarvestCampaignId == model.HarvestCampaignId && x.CustomerId == model.CustomerId).FirstOrDefault();
        //    var entity = _mapper.CreateMapper().Map<ItemCart>(model);
        //    if (cart != null)
        //    {
        //        cart.Quantity += model.Quantity;
        //        if (harvestCampaign.Quantity < cart.Quantity)
        //            throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{harvestCampaign.ProductName} đã hết hàng!");
        //        await UpdateAsyn(cart);
        //    }
        //    else
        //        await CreateAsyn(entity);
        //    return model;
        //}

        //public async Task<ItemCart> Update(int id, ItemCartUpdateModel model)
        //{
        //    var entity = await GetAsyn(id);
        //    var harvestCampaign = _harvestCampaignService.Get(x => x.Id == entity.HarvestCampaignId).FirstOrDefault();
        //    if (harvestCampaign.Inventory < model.Quantity)
        //        throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"{harvestCampaign.ProductName} đã hết hàng!");
        //    if (model.Id != id)
        //        throw new ErrorResponse((int)HttpStatusCode.BadRequest, $"Vui lòng nhập đúng!");
        //    if (model.Id != id || entity == null)
        //        throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
        //    var updateEntity = _mapper.CreateMapper().Map(model, entity);
        //    await UpdateAsyn(updateEntity);
        //    return updateEntity;
        //}
        //public async Task<ItemCart> Delete(int id)
        //{
        //    var entity = Get(id);
        //    if (entity == null)
        //        throw new ErrorResponse((int)HttpStatusCode.NotFound, $"Không tìm thấy!");
        //    await DeleteAsyn(entity);
        //    return entity;
        //}

        //public async Task<int> Count(string customerId)
        //{
        //    var result = await Get(x => x.CustomerId == customerId).CountAsync();
        //    return result;
        //}
    }
}
