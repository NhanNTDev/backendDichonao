using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiCho.DataService.Models;
using DiCho.DataService.Services;
using System.Linq;
using DiCho.DataService.ViewModels;
using System.Threading.Tasks;
using DiCho.DataService.Commons;

namespace DiCho.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/ware-houses")]
    public partial class WareHousesController : ControllerBase
    {
        private readonly IWareHouseService _wareHouseService;
        private readonly IShipmentService _shipmentService;
        public WareHousesController(IWareHouseService wareHouseService, IShipmentService shipmentService)
        {
            _wareHouseService=wareHouseService;
            _shipmentService = shipmentService;
        }

        /// <summary>
        /// get warehouses
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllWarehouse(string name)
        {
            return Ok(await _wareHouseService.GetAllWarehouse(name));
        }
        
        /// <summary>
        /// get warehouse by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetWarehouse(int id)
        {
            return Ok(await _wareHouseService.GetWarehouse(id));
        }

        /// <summary>
        /// get warehouse by id
        /// </summary>
        /// <param name="warehouseManagerId"></param>
        /// <returns></returns>
        [HttpGet("manager/{warehouseManagerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetWarehouseByWarehouseManager(string warehouseManagerId)
        {
            return Ok(await _wareHouseService.GetWarehouseByWarehouseManager(warehouseManagerId));
        }
        
        /// <summary>
        /// create a warehouse
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateWarehouse(WareHouseCreateModel model)
        {
            await _wareHouseService.CreateWarehouse(model);
            return Ok("Create successfully!");
        }
        
        /// <summary>
        /// update a warehouse
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateWarehouse(int id, WareHouseUpdateModel model)
        {
            await _wareHouseService.UpdateWarehouse(id, model);
            return Ok("Update successfully!");
        }
        
        /// <summary>
        /// delete a warehouse
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            await _wareHouseService.DeleteWarehouse(id);
            return Ok("Delete successfully!");
        }
        
        /// <summary>
        /// Dashboard of warehouse manager
        /// </summary>
        /// <param name="warehouseManagerId"></param>
        /// <returns></returns>
        [HttpGet("dashboard/{warehouseManagerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DashBoardOfWarehouse(string warehouseManagerId)
        {
            return Ok(await _shipmentService.DashBoardOfWarehouse(warehouseManagerId));
        }
        
    }
}
