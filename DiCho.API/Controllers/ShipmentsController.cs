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
    [Route("api/v{version:apiVersion}/shipments")]
    public partial class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        private readonly IVehicleRoutingService _vehicleRoutingService;

        public ShipmentsController(IShipmentService shipmentService, IVehicleRoutingService vehicleRoutingService)
        {
            _shipmentService=shipmentService;
            _vehicleRoutingService = vehicleRoutingService;
        }

        /// <summary>
        /// get shipments for a driver
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("driver/{driverId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetShipMentForDriver(string driverId, string status, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _shipmentService.GetShipMentForDriver(driverId, status, page, size));
        }
        
        /// <summary>
        /// count tasks of driver
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [HttpGet("tasks/{driverId}")]
        [MapToApiVersion("1")]
        public IActionResult TaskTransportsOfDriver(string driverId)
        {
            return Ok(_shipmentService.TasksOfDriver(driverId));
        }
        
        /// <summary>
        /// get a shipment detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetShipmentId(int id)
        {
            return Ok(await _shipmentService.GetShipmentId(id));
        }

        /// <summary>
        /// get shipments for a warehouse
        /// </summary>
        /// <param name="warehouseManagerId"></param>
        /// <param name="flag"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("warehouse/{warehouseManagerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetShipmentForWarehouseManager(string warehouseManagerId, bool flag, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _shipmentService.GetShipmentForWarehouseManager(warehouseManagerId, flag, page, size));
        }

        /// <summary>
        /// create shipment for warehouse
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <returns></returns>
        [HttpPost("routing-problem/{warehouseId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create(int warehouseId)
        {
            await _vehicleRoutingService.CreateShipmentForRoutingProblem1(warehouseId);
            return Ok("Create Successfully!");
        }

        /// <summary>
        /// update shipment for driverId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("warehouse/assign-work")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateShipmentForDriver(List<ShipmentUpdateModel> model)
        {
            await _shipmentService.UpdateShipmentForDriver(model);
            return Ok("Update Successfully!");
        }

        /// <summary>
        /// update complete task of shipment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("driver/complete-task/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateStatusComplete(int id)
        {
            await _shipmentService.UpdateStatusComplete(id);
            return Ok("Update successfully");
        }

        ///// <summary>
        ///// delete shipment in warehouse
        ///// </summary>
        ///// <param name="warehouseId"></param>
        ///// <returns></returns>
        //[HttpDelete("{warehouseId}")]
        //[MapToApiVersion("1")]
        //public async Task<IActionResult> DeleteRemaining(int warehouseId)
        //{
        //    await _shipmentService.DeleteRemaining(warehouseId);
        //    return Ok("Delete Successfully!");
        //}
        
    }
}
