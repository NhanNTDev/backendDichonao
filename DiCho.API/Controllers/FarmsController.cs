using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiCho.DataService.Models;
using DiCho.DataService.Services;
using System.Linq;
using System.Threading.Tasks;
using DiCho.DataService.ViewModels;
using DiCho.DataService.Commons;
using Microsoft.AspNetCore.Authorization;

namespace DiCho.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/farms")]
    public partial class FarmsController : ControllerBase
    {
        private readonly IFarmService _farmService;
        public FarmsController(IFarmService farmService){
            _farmService=farmService;
        }

        /// <summary>
        /// get farms
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "farmer")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Gets(string farmerId, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _farmService.Gets(farmerId, page, size));
        }

        /// <summary>
        /// get a farm by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _farmService.GetById(id));
        }
        
        /// <summary>
        /// get farms in campaign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("farm-in-campaign/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFarmInCampaign(int id)
        {
            return Ok(await _farmService.GetFarmInCampaign(id));
        }

        /// <summary>
        /// statistical of farm by campaignId
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet("statistical/{campaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> FarmStatistical(int campaignId)
        {
            return Ok(await _farmService.FarmStatistical(campaignId));
        }

        /// <summary>
        /// create a farm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]

        public async Task<IActionResult> Create([FromForm] FarmCreateInputModel entity)
        {
            await _farmService.Create(entity);
            return Ok("Create successfully!");
        }

        /// <summary>
        /// update a farm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, [FromForm] FarmUpdateInputModel entity)
        {
            await _farmService.Update(id, entity);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// delete a farm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(int id)
        {
            await _farmService.Delete(id);
            return Ok("Delete successfully!");
        }

        /// <summary>
        /// get farms by name
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("search-name")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFarmByName(string farmerId, [FromQuery] FarmNameModel model)
        {
            return Ok(await _farmService.GetFarmByName(farmerId, model));
        }

        /// <summary>
        /// count farm by farmerId
        /// </summary>
        /// <param name="farmerId"></param>
        /// <returns></returns>
        [HttpGet("count-farm/{farmerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CountFarmByFarmerId(string farmerId)
        {
            return Ok(await _farmService.CountFarmByFarmerId(farmerId));
        }
    }
}
