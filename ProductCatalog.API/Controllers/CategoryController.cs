using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.IServices;
using ProductCatalog.Common.Category.Request;
using ProductCatalog.Common.User.Request;

namespace ProductCatalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
      
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoryDTO getAllCategoryDTO)
        {
            var response = await _categoryService.GetCategories(getAllCategoryDTO);
            if (!response.IsSuccess)
            {
               return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryReqDTO createCategoryReqDTO)
        {
            var response = await _categoryService.CreateCategory(createCategoryReqDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryReqDTO updateCategoryReqDTO)
        {
            var response = await _categoryService.UpdateCategory(updateCategoryReqDTO);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.DeleteCategory(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _categoryService.GetCategory(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
