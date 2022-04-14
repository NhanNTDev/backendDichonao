using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DiCho.DataService.Models;
using DiCho.DataService.Services;
using System.Linq;
using DiCho.DataService.ViewModels;
using DiCho.DataService.Commons;
using System.Threading.Tasks;

namespace DiCho.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/product-systems")]
    public partial class ProductSystemsController : ControllerBase
    {
        private readonly IProductSystemService _productSystemService;
        public ProductSystemsController(IProductSystemService productSystemService)
        {
            _productSystemService=productSystemService;
        }

        /// <summary>
        /// get products
        /// </summary>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Gets([FromQuery] ProductSystemModel model, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _productSystemService.Gets(model, page, size));
        }

        /// <summary>
        /// get all product system
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAllProductSystem()
        {
            return Ok(await _productSystemService.GetAllProductSystem());
        }
        
        /// <summary>
        /// get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productSystemService.GetById(id));
        }

        /// <summary>
        /// create a product
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]

        public async Task<IActionResult> Create(ProductSystemCreateModel entity)
        {
            await _productSystemService.Create(entity);
            return Ok("Create successfully!");
        }

        /// <summary>
        /// update a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, ProductSystemUpdateModel entity)
        {
            await _productSystemService.Update(id, entity);
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
            await _productSystemService.Delete(id);
            return Ok("Delete successfully!");
        }
    }
}
