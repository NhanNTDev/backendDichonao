using DiCho.Core.Custom;
using DiCho.DataService.Commons;
using DiCho.DataService.Models;
using DiCho.DataService.Services;
using DiCho.DataService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DiCho.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/users")]

    public class UsersController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        private readonly ICampaignService _campaignService;
        private readonly IShipmentService _shipmentService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IOrderService _orderService;
        public UsersController(IJWTService jwtService, ICampaignService campaignService, IShipmentService shipmentService, IWareHouseService wareHouseService, IOrderService orderService)
        {
            _jwtService = jwtService;
            _campaignService = campaignService;
            _shipmentService = shipmentService;
            _wareHouseService = wareHouseService;
            _orderService = orderService;
        }

        /// <summary>
        /// statistical of admin
        /// </summary>
        /// <returns></returns>
        [HttpGet("admin/statistical")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> StatisticalOfAdmin()
        {
            return Ok(await _orderService.StatisticalOfAdmin());
        }
        
        /// <summary>
        /// statistical of farmer
        /// </summary>
        /// <param name="farmerId"></param>
        /// <returns></returns>
        [HttpGet("statistical/{farmerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> StatisticalOfFarmer(string farmerId)
        {
            return Ok(await _orderService.StatisticalOfFarmer(farmerId));
        }
        
        /// <summary>
        /// dashboard of farmer
        /// </summary>
        /// <param name="farmerId"></param>
        /// <returns></returns>
        [HttpGet("count/dashboard")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DashBoard(string farmerId)
        {
            return Ok(await _campaignService.DashBoard(farmerId));
        }
        
        /// <summary>
        /// dashboard of admin
        /// </summary>
        /// <returns></returns>
        [HttpGet("admin/dashboard")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DashBoardOfAdmin()
        {
            return Ok(await _orderService.DashBoardOfAdmin());
        }

        /// <summary>
        /// get user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetUserId(string id)
        {
            return Ok(await _jwtService.GetUserId(id));
        }

        /// <summary>
        /// get list loyal customers of farmer
        /// </summary>
        /// <param name="farmerId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("loyal-customers/farm/{farmerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetInfomationOfUsers(string farmerId, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _orderService.GetInfomationOfUsers(farmerId, page, size));
        }
        
        /// <summary>
        /// check duplicate user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("check-duplicate")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CheckDuplicateUser(string username)
        {
            return Ok(await _jwtService.CheckDuplicateUser(username));
        }
        
        /// <summary>
        /// register a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] AspNetUsersCreateModel model)
        {
            return Ok(await _jwtService.CreateUser(model));
        }
        
        /// <summary>
        /// register a user by admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("admin")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateUserForAdmin([FromBody] CreateUserForAdmin model)
        {
            return Ok(await _jwtService.CreateUserForAdmin(model));
        }
        
        /// <summary>
        /// register a user by warehouseManager
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("warehouseManager")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateUserForDelivery([FromBody] CreateUserForDelivery model)
        {
            return Ok(await _jwtService.CreateUserForDelivery(model));
        }

        /// <summary>
        /// login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [MapToApiVersion("1")]
        [Route("login")]
        [ProducesResponseType(typeof(TokenModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login(AspNetUserLoginModel model)
        {
            return Ok(await _jwtService.Login(model));
        }

        /// <summary>
        /// update a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("update-user/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateUser(string id, AspNetUsersUpdateModel model)
        {
            return Ok(await _jwtService.UpdateUser(id, model));
        }
        
        /// <summary>
        /// change image a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("change-image/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeImage(string id, [FromForm] AspNetUsersUpdateImageInputModel model)
        {
            return Ok(await _jwtService.UpdateImage(id, model));
        }
        
        /// <summary>
        /// change image a customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("change-image-customer/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateImageForCustomer(string id, AspNetUsersUpdateImageModel model)
        {
            await _jwtService.UpdateImageForCustomer(id, model);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// change password a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("change-password")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangePassword(AspNetUsersChangePasswordModel model)
        {
            return Ok(await _jwtService.ChangePassword(model));
        }
        
        /// <summary>
        /// count user
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        [MapToApiVersion("1")]
        public IActionResult CountUser()
        {
            return Ok(_jwtService.CountUser());
        }

        /// <summary>
        /// get user by role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("user/{role}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetUserByRole(string role, [FromQuery] UserDataMapModel model, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _jwtService.GetUserByRole(role, model, page, size));
        }

        /// <summary>
        /// get all driver in warehouse
        /// </summary>
        /// <param name="wareHouseId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("all/driver/{wareHouseId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllDriverInWareHouse(int wareHouseId, int type)
        {
            return Ok(await _shipmentService.GetAllDriverInWareHouse(wareHouseId, type));
        }
        
        /// <summary>
        /// get driver in warehouse
        /// </summary>
        /// <param name="wareHouseId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("driver/{wareHouseId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetDriverInWareHouse(int wareHouseId, int type)
        {
            return Ok(await _shipmentService.GetDriverInWareHouse(wareHouseId, type));
        }
        
        /// <summary>
        /// get driver ready in warehouse
        /// </summary>
        /// <param name="wareHouseId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("driver-ready/{wareHouseId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetDriverReadyInWareHouse(int wareHouseId, int type)
        {
            return Ok(await _shipmentService.GetDriverReadyInWareHouse(wareHouseId, type));
        }

        /// <summary>
        /// get user free warehouse manager 
        /// </summary>
        /// <returns></returns>
        [HttpGet("warehouse")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetWareHouseManger()
        {
            return Ok(await _wareHouseService.GetWareHouseManger());
        }

        /// <summary>
        /// ban and unban user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("ban-unban/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> BanAndUnBanUser(string id)
        {
            await _jwtService.BanAndUnBanUser(id);
            return Ok("Successfully");
        }

        /// <summary>
        /// forgot password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPut("forgot-password/{username}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ForgotPassword(string username, string newPassword)
        {
            return Ok(await _jwtService.ConfirmForgotPassword(username, newPassword));
        }

    }
}
