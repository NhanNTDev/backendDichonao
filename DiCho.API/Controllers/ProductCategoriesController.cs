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
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/product-categories")]
    public partial class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoriesController(IProductCategoryService productCategoryService){
            _productCategoryService=productCategoryService;
        }

        /// <summary>
        /// get categories product
        /// </summary>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Gets([FromQuery] ProductCategoryModel model, int page = CommonConstants.DefaultPage, int size = CommonConstants.DefaultPaging)
        {
            return Ok(await _productCategoryService.Gets(model, page, size));
        }

        /// <summary>
        /// get a category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productCategoryService.GetById(id));
        }

        /// <summary>
        /// create a category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create(ProductCategoryCreateModel entity)
        {
            var result = await _productCategoryService.Create(entity);
            return Ok(result);
        }

        /// <summary>
        /// update a category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, ProductCategoryUpdateModel entity)
        {
            await _productCategoryService.Update(id, entity);
            return Ok("Update successfully!");
        }

        /// <summary>
        /// delete a category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productCategoryService.Delete(id);
            return Ok("Delete successfully!");
        }
    }
}
