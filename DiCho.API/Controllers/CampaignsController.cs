using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiCho.DataService.Models;
using DiCho.DataService.Services;
using System.Linq;
using System.Threading.Tasks;
using DiCho.DataService.ViewModels;
using DiCho.DataService.Commons;

namespace DiCho.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/campaigns")]
    public partial class CampaignsController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        public CampaignsController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        /// <summary>
        /// get campaigns
        /// </summary>
        /// <param name="deliveryZoneId"></param>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Gets(int deliveryZoneId, [FromQuery] CampaignModel model, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _campaignService.GetByZoneId(deliveryZoneId, model, page, size));
        }

        /// <summary>
        /// get campaigns
        /// </summary>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("all")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllCampaign([FromQuery] CampaignModel model, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _campaignService.GetAllCampaign(model, page, size));
        }
        
        /// <summary>
        /// get campaign of farmer applied
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("campaign-farmer-applied/{farmerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCampaignOfFarmer(string farmerId, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _campaignService.GetCampaignOfFarmerApplied(farmerId, page, size));
        }

        /// <summary>
        /// get campaign of farmer apply
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("campaign-farmer-apply/{farmerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCampaignOfFarmerApply(string farmerId, string type, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _campaignService.GetCampaignOfFarmerApply(farmerId, type, page, size));
        }

        /// <summary>
        /// get a campaign by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _campaignService.GetById(id));
        }

        /// <summary>
        /// get a campaign detail by campaignId
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet("detail-apply/{campaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCampaignDetailApply(int campaignId)
        {
            return Ok(await _campaignService.GetCampaignDetailApply(campaignId));
        }

        /// <summary>
        /// search farmer apply to campaign
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("search-apply")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SearchCampaignApply(string farmerId, [FromQuery] CampaignSearchModel model)
        {
            return Ok(await _campaignService.SearchCampaignApply(farmerId, model));
        }

        /// <summary>
        /// get a campaign detail applied by farmerId and campaignId
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet("detail-applied/{farmerId}/{campaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCampaignDetailApplied(string farmerId, int campaignId)
        {
            return Ok(await _campaignService.GetCampaignDetailApplied(farmerId, campaignId));
        }
        
        /// <summary>
        /// search farmer applied to campaign
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("search-applied")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SearchCampaignApplied(string farmerId, [FromQuery] CampaignSearchModel model)
        {
            return Ok(await _campaignService.SearchCampaignApplied(farmerId, model));
        }

        /// <summary>
        /// create a campaign
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create(CampaignCreateInputModel entity)
        {
            await _campaignService.Create(entity);
            return Ok("Create Successfully");
        }

        /// <summary>
        /// update a campaign
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, CampaignUpdateInputModel entity)
        {
            await _campaignService.Update(id, entity);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// delete a campaign
        /// </summary>
        /// <param name="id"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(int id, string note)
        {
            await _campaignService.Delete(id, note);
            return Ok("Delete successfully!");
        }

        /// <summary>
        /// get campaigns apply request
        /// </summary>
        /// <returns></returns>
        [HttpGet("apply-request")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCampaignApplyRequest()
        {
            return Ok(await _campaignService.GetApplyRequest());
        }

        /// <summary>
        /// get farm by Apply Request
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("farm-apply-request/{campaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFarmApplyRequest(int campaignId, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _campaignService.GetFarmApplyRequest(campaignId, page, size));
        }

        /// <summary>
        /// get harvest by Apply Request
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="farmId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("harvest-apply-request/{campaignId}/{farmId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetHarvestApplyRequest(int campaignId, int farmId, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _campaignService.GetHarvestApplyRequest(campaignId, farmId, page, size));
        }

        /// <summary>
        /// get farm can join
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet("farm-join/{farmerId}/{campaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFarmsCanJoin(string farmerId, int campaignId)
        {
            return Ok(await _campaignService.GetFarmsCanJoinCampaign(farmerId, campaignId));
        }

        /// <summary>
        /// get harvest can apply of farm
        /// </summary>
        /// <param name="farmId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet("harvest-apply/{farmId}/{campaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetHarvestApply(int farmId, int campaignId)
        {
            return Ok(await _campaignService.GetHarvestApplyOfFarm(farmId, campaignId));
        }
        
        /// <summary>
        /// get harvest detail by id
        /// </summary>
        /// <param name="harvestId"></param>
        /// <returns></returns>
        [HttpGet("harvest-detail/{harvestId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetHarvestDetail(int harvestId)
        {
            return Ok(await _campaignService.GetHarvestDetail(harvestId));
        }

        /// <summary>
        /// count campaign farmer applied
        /// </summary>
        /// <param name="farmerId"></param>
        /// <returns></returns>
        [HttpGet("count-campaign-applied/{farmerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CountCampaignApplied(string farmerId)
        {
            return Ok(await _campaignService.CountCampaignApplied(farmerId));
        }

        /// <summary>
        /// count campaign farmer can apply
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("count-campaign-apply/{farmerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CountCampaignApply(string farmerId, string type)
        {
            return Ok(await _campaignService.CountCampaignApply(farmerId, type));
        }

        /// <summary>
        /// suggest apply for farmer
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet("{farmerId}/{campaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCampaignForFarmerApply(string farmerId, int campaignId)
        {
            return Ok(await _campaignService.GetCampaignForFarmerApply(farmerId, campaignId));
        }
    }
}
