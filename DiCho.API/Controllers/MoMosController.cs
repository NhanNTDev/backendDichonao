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
    [Route("api/v{version:apiVersion}/momos")]
    [ApiVersion("1")]
    [ApiController]
    public class MoMosController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public MoMosController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Momo payment checkout
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ReturnUrl(UrlReturn model)
        {
            return Ok(await _orderService.ReturnUrl(model));
        }
    }
}
