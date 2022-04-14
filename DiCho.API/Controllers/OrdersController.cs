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
    [Route("api/v{version:apiVersion}/orders")]
    public partial class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IShipmentService _shipmentService;
        public OrdersController(IOrderService orderService, IShipmentService shipmentService)
        {
            _orderService = orderService;
            _shipmentService = shipmentService;
        }
        
        [HttpPost("testabc")]
        [MapToApiVersion("1")]
        public IActionResult BinPackingMip1(double[] weights)
        {
            return Ok(_orderService.BinPackingMip1(weights));
        }
        
        /// <summary>
        /// get orders
        /// </summary>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllOrder([FromQuery] OrderModel model, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _orderService.GetAllOrder(model, page, size));
        }
        
        /// <summary>
        /// get orders
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("customer/{customerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetOrderOfCustomer(string customerId)
        {
            return Ok(await _shipmentService.GetOrderOfCustomer(customerId));
        }

        /// <summary>
        /// get a order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _orderService.GetById(id));
        }

        /// <summary>
        /// get ship cost
        /// </summary>
        /// <param name="productCost"></param>
        /// <param name="address"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet("ship-cost")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ShipCostOfOrder(double productCost, string address, int campaignId)
        {
            return Ok(await _orderService.ShipCostOfOrder(productCost, address, campaignId));
        }
        
        /// <summary>
        /// create a order
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create(OrderCreateModelInput entity)
        {
            return Ok(await _orderService.Create(entity));
        }

        /// <summary>
        /// feed back a order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("feedback/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Feedback(int id, FeedbackOrderModel entity)
        {
            await _orderService.Feedback(id, entity);
            return Ok("Update successfully!");
        }
        
        /// <summary>
        /// change status for order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, OrderUpdateModel entity)
        {
            await _orderService.Update(id, entity);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// reject/cancel a order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> RejectOrder(int id, string note)
        {
            await _orderService.RejectOrder(id, note);
            return Ok("Reject successfully!");
        }

        /// <summary>
        /// update driver for order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("warehouse/update-driver")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateDriverForOrderByWarehouse(List<UpdateDriverForOrderByWarehouse> model)
        {
            await _orderService.UpdateDriverForOrderByWarehouse(model);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// get orders in warehouse
        /// </summary>
        /// <param name="warehouseManagerId"></param>
        /// <param name="flag"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("warehouse/{warehouseManagerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeliveryToCustomerForWareHouseManger(string warehouseManagerId, bool flag, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _orderService.DeliveryToCustomerForWareHouseManger(warehouseManagerId, flag, page, size));
        }
        
        /// <summary>
        /// get order for driver
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("driver/{driverId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeliveryToCustomerForDriver(string driverId, string status, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _orderService.DeliveryToCustomerForDriver(driverId, status, page, size));
        }
    }
}
