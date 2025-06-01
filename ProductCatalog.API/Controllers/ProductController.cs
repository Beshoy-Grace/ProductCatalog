using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.IServices;
using ProductCatalog.Common.Product.Request;
using ProductCatalog.Common.User.Request;

namespace ProductCatalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProductController : ControllerBase
    {
      
        private readonly IProductService  _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productService.GetAllProductsAsync();
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateProductReqDTO createProductReqDTO)
        {
            var response = await _productService.CreateProductAsync(createProductReqDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update( [FromBody] UpdateProductReqDTO updateProductReqDTO)
        {
            var response = await _productService.UpdateProductAsync(updateProductReqDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetByCategory/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var response = await _productService.GetAllProductsByCategoryAsync(categoryId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


    }
}
