using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiCho.DataService.Models;
using DiCho.DataService.Services;
using System.Linq;
using System.Threading.Tasks;
using DiCho.DataService.ViewModels;
using DiCho.DataService.Commons;
using System.Collections.Generic;

namespace DiCho.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/product-harvest-in-campaigns")]
    public partial class ProductHarvestInCampaignsController : ControllerBase
    {
        private readonly IProductHarvestInCampaignService _harvestCampaignService;
        public ProductHarvestInCampaignsController(IProductHarvestInCampaignService harvestCampaignService)
        {
            _harvestCampaignService = harvestCampaignService;
        }

        /// <summary>
        /// get list of product harvest in campaign
        /// </summary>
        /// <param name="deliveryZoneId"></param>
        /// <param name="model"></param>
        /// <param name="categorys"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Gets(int deliveryZoneId, [FromQuery] ProductHarvestInCampaignModel model, [FromQuery] List<string> categorys, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _harvestCampaignService.Gets(deliveryZoneId, model, categorys, page, size));
        }

        /// <summary>
        /// get list harvests in campaign by farmId
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="farmId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("campaign/{campaignId}/farm/{farmId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetHarvestCampaignOfFarm(int campaignId, int farmId, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _harvestCampaignService.GetHarvestCampaignOfFarm(campaignId, farmId, page, size));
        }
        
        /// <summary>
        /// search list products in campaign by farmId 
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="farmId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("search/{campaignId}/{farmId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SearchHarvestCampaignOfFarm(int campaignId, int farmId, [FromQuery] ProductHarvestCampaignSearchModel model)
        {
            return Ok(await _harvestCampaignService.SearchHarvestCampaignOfFarm(campaignId, farmId, model));
        }

        /// <summary>
        /// search list products in campaign by delivery zone id
        /// </summary>
        /// <param name="deliveryZoneId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("search/{deliveryZoneId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SearchHarvestCampaignOfCustomer(int deliveryZoneId, [FromQuery] ProductHarvestCampaignSearchModel model)
        {
            return Ok(await _harvestCampaignService.SearchHarvestCampaignOfCustomer(deliveryZoneId, model));
        }
        
        /// <summary>
        /// count harvests in campaign of farm
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="farmId"></param>
        /// <returns></returns>
        [HttpGet("count/{campaignId}/{farmId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CountHarvestCampaignOfFarm(int campaignId, int farmId)
        {
            return Ok(await _harvestCampaignService.CountHarvestCampaignOfFarm(campaignId, farmId));
        }

        /// <summary>
        /// get a product harvest in campaign detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetHarvestCampaignDetailById(int id)
        {
            return Ok(await _harvestCampaignService.GetHarvestCampaignDetailById(id));
        }
        
        /// <summary>
        /// get a product harvest in campaign by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _harvestCampaignService.GetById(id));
        }
        
        /// <summary>
        /// origin a product harvest in campaign by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("origin/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetOriginProductHarvestById(int id)
        {
            return Ok(await _harvestCampaignService.GetOriginProductHarvestById(id));
        }

        /// <summary>
        /// add list of product harvest into campaign
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create(List<ProductHarvestCampaignCreateModel> entity)
        {
            await _harvestCampaignService.Create(entity);
            return Ok("Create successfully!");
        }

        /// <summary>
        /// update a product harvest in camapaign
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, ProductHarvestCampaignUpdateModel entity)
        {
            await _harvestCampaignService.Update(id, entity);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// accept product harvest in campaign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("accept")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateAccept(List<int> id)
        {
            await _harvestCampaignService.UpdateAccept(id);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// reject harvest in campaign
        /// </summary>
        /// <param name="id"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPut("reject")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateReject(int id, string note)
        {
            await _harvestCampaignService.UpdateReject(id, note);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// delete a harvest in campaign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(int id)
        {
            await _harvestCampaignService.Delete(id);
            return Ok("Delete successfully!");
        }
    }
}
