using DiCho.DataService.Models;
using DiCho.DataService.Services;
using DiCho.DataService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZaloDotNetSDK;

namespace DiCho.API.Controllers
{
    [Route("api/v{version:apiVersion}/webhooks")]
    [ApiVersion("1")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private readonly IZaloService _zaloService;

        public WebhooksController(IZaloService zaloService)
        {
            _zaloService = zaloService;
        }

        /// <summary>
        /// send message from zalo oa
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SendMessage()
        {
            await _zaloService.SendMessage();
            return Ok("Ok");
        }
        
        /// <summary>
        /// get info zalo user
        /// </summary>
        /// <returns></returns>
        [HttpPost("login/zalo")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetInfo(string code)
        {
            return Ok(await _zaloService.GetInfo(code));
        }

    }
}
