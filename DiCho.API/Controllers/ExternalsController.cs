using DiCho.DataService.Services;
using DiCho.DataService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiCho.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/externals")]
    public class ExternalsController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;
        private readonly ITradeZoneMapService _tradeZoneMapService;
        private readonly IVehicleRoutingService _vehicleRoutingService;

        public ExternalsController(IFirebaseService firebaseService, ITradeZoneMapService tradeZoneMapService, IVehicleRoutingService vehicleRoutingService)
        {
            _firebaseService = firebaseService;
            _tradeZoneMapService = tradeZoneMapService;
            _vehicleRoutingService = vehicleRoutingService;
        }

        /// <summary>
        /// vehicel routing
        /// </summary>
        /// <returns></returns>
        [HttpPost("vehicel-routing")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Routing(int vehicleNumber)
        {
            var result = await _vehicleRoutingService.VehicleRouting1(vehicleNumber);
            return Ok(result);
        }

        ///// <summary>
        ///// distance matrix
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("distance-matrix")]
        //[MapToApiVersion("1")]
        //public async Task<IActionResult> Create_distance_matrix()
        //{
        //    var result = await _vehicleRoutingService.Create_distance_matrix();
        //    return Ok(result);
        //}


        /// <summary>
        /// get zone by address
        /// </summary>
        /// <returns></returns>
        [HttpGet("zone")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CheckZoneForCampagin(string address)
        {
            var result = await _tradeZoneMapService.CheckZoneForCampagin(address);
            return Ok(result);
        }

        /// <summary>
        /// upload images to firebase
        /// </summary>
        /// <param name="files"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Upload([FromForm] IFormFile files, string folder)
        {
            var result = await _firebaseService.UploadFileToFirebase(files, folder);
            return Ok(result);
        }
        
        /// <summary>
        /// get groupzone
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetListZone()
        {
            var result = await _tradeZoneMapService.GetListZone();
            return Ok(result);
        }
        
        /// <summary>
        /// get groupzone
        /// </summary>
        /// <returns></returns>
        [HttpGet("a/{longitude}/{latitude}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAddressFromLatLong(double longitude, double latitude)
        {
            var result = await _tradeZoneMapService.GetAddressFromLatLong(longitude, latitude);
            return Ok(result);
        }
        
        /// <summary>
        /// get notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("notification/{userId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetNotiRedis(string userId)
        {
            return Ok(await _firebaseService.GetNotiRedis(userId));
        }
        
        /// <summary>
        /// delete notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("notification/{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteNotiRedis(int id)
        {
            await _firebaseService.DeleteNotiRedis(id);
            return Ok("Delete successfully!");
        }

    }
}
