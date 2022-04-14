using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiCho.DataService.Models;
using DiCho.DataService.Services;
using System.Linq;
using System.Threading.Tasks;
using DiCho.DataService.Commons;
using DiCho.DataService.ViewModels;
using System.Collections.Generic;

namespace DiCho.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/farm-orders")]
    public partial class FarmOrdersController : ControllerBase
    {
        private readonly IFarmOrderService _farmOrderService;
        private readonly IOrderService _orderService;
        private readonly IVehicleRoutingService _vehicleRoutingService;
        public FarmOrdersController(IFarmOrderService farmOrderService, IOrderService orderService, IVehicleRoutingService vehicleRoutingService)
        {
            _farmOrderService = farmOrderService;
            _orderService = orderService;
            _vehicleRoutingService = vehicleRoutingService;
        }

        /// <summary>
        /// filter farmOrders 
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("all/{farmerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Gets(string farmerId, [FromQuery] FarmOrderModel model, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _orderService.GetAllFarmOrder(farmerId, model, page, size));
        }
        
        /// <summary>
        /// count farmOrders by status
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("count/{farmerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CountAllFarmOrderbyStatus(string farmerId, string status)
        {
            return Ok(await _orderService.CountAllFarmOrderbyStatus(farmerId, status));
        }

        /// <summary>
        /// get list farmOrder of farm
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="farmId"></param>
        /// <returns></returns>
        [HttpGet("{campaignId}/{farmId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFarmOrder(int campaignId, int farmId)
        {
            return Ok(await _farmOrderService.GetFarmOrder(campaignId, farmId));
        }

        /// <summary>
        /// get list feedback of farm by farm id
        /// </summary>
        /// <param name="farmId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("feedback/{farmId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFeedbackOfFarm(int farmId, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _orderService.GetFeedbackOfFarm(farmId, page, size));
        }

        /// <summary>
        /// get farmOrder by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFarmOrderDetail(int id)
        {
            return Ok(await _orderService.GetFarmOrderDetail(id));
        }

        ///// <summary>
        ///// get farmOrders for warehouse manager
        ///// </summary>
        ///// <param name="warehouseManagerId"></param>
        ///// <param name="flag"></param>
        ///// <param name="page"></param>
        ///// <param name="size"></param>
        ///// <returns></returns>
        //[HttpGet("warehouse-manager")]
        //[MapToApiVersion("1")]
        //public async Task<IActionResult> GetGroupFarmOrderForDelivery(string warehouseManagerId, bool flag , int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        //{
        //    var check = await _orderService.GetGroupFarmOrderForDelivery(warehouseManagerId, flag, page, size);
        //    return Ok(check);
        //}

        /// <summary>
        /// get farmOrders for warehouse manager
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        [HttpGet("warehouse-manager")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetGroupFarmOrderForDelivery(int warehouseId)
        {
            var check = await _vehicleRoutingService.GetFarmsCollection(warehouseId);
            return Ok(check);
        }

        /// <summary>
        /// update driver for farmOrder
        /// </summary>
        /// <param name="modelInput"></param>
        /// <returns></returns>
        [HttpPut("update-driver")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateFarmOrderDriver(List<FarmOrderUpdateDriverInputModel> modelInput)
        {
            await _farmOrderService.UpdateFarmOrderDriver(modelInput);
            return Ok("Update Successfully!");
        }
        
        /// <summary>
        /// get farmOrders by farm
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        [HttpGet("farm/{farmId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFarmOrderByFarm(int farmId)
        {
            var check = await _orderService.GetFarmOrderByFarm(farmId);
            
            return Ok(check);
        }

        /// <summary>
        /// get farmOrders for driver
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("driver/{driverId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetGroupFarmOrder(string driverId, string status, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _orderService.GetGroupFarmOrderForDriver(driverId, status, page, size));
        }
        
        /// <summary>
        /// update status of farmOrder
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            await _farmOrderService.UpdateStatus(id, status);
            return Ok("Update successfully!");
        }
        
        /// <summary>
        /// get farmOrder by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPut("cancel/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateCancelStatus(int id, string note)
        {
            await _farmOrderService.UpdateCancelStatus(id, note);
            return Ok("Cancel successfully!");
        }
        
        /// <summary>
        /// get farmOrder by campaignId
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet("campaign/{campaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFarmOrderByCampaign(int campaignId)
        {
            return Ok(await _farmOrderService.GetFarmOrderByCampaign(campaignId));
        }
        
        /// <summary>
        /// get revenues of farmer by time
        /// </summary>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <returns></returns>
        [HttpGet("revenuse")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetRevenues(string timeFrom, string timeTo)
        {
            return Ok(await _farmOrderService.GetRevenues(timeFrom, timeTo));
        }
        
    }
}
