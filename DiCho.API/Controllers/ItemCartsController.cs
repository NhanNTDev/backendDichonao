using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiCho.DataService.Models;
using DiCho.DataService.Services;
using System.Linq;
using System.Threading.Tasks;
using DiCho.DataService.ViewModels;
using DiCho.DataService.Commons;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Text;
using System.Collections.Generic;
using System;

namespace DiCho.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/item-carts")]
    public partial class ItemCartsController : ControllerBase
    {
        private readonly IItemCartService _itemCartService;

        public ItemCartsController(IItemCartService itemCartService)
        {
            _itemCartService=itemCartService;
        }
        
        /// <summary>
        /// get items cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Gets(string customerId)
        {
            var check = await _itemCartService.Gets(customerId);
            if(check.CampaignId == null)
                return Ok(new List<string>());
            else
                return Ok(check);
        }

        /// <summary>
        /// add a item to cart
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create(ItemCartCreateInputModel model)
        {
            await _itemCartService.Create(model);
            return Ok("Add Successfully!");
        }

        /// <summary>
        /// update a item in cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="harvestCampaignId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPut("{customerId}/{harvestCampaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(string customerId, int harvestCampaignId, int quantity)
        {
            await _itemCartService.Update(customerId, harvestCampaignId, quantity);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// delete a item in cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="harvestCampaignId"></param>
        /// <returns></returns>
        [HttpDelete("{customerId}/{harvestCampaignId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(string customerId, int harvestCampaignId)
        {
            await _itemCartService.Delete(customerId, harvestCampaignId);
            return Ok("Delete successfully!");
        }
        
        /// <summary>
        /// delete a cart of customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete("cart/{customerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteCart(string customerId)
        {
            await _itemCartService.DeleteCart(customerId);
            return Ok("Delete successfully!");
        }
        
        /// <summary>
        /// count item in cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [MapToApiVersion("1")]
        [HttpGet("count/{customerId}")]
        public async Task<int> Count(string customerId)
        {
            var result = await _itemCartService.Count(customerId);
            return result;
        }
    }
}
