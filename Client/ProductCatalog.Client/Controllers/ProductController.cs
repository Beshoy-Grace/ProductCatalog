using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Common.Product.Request;
using System.Net.Http;
using ProductCatalog.Service.Interfaces;
using ProductCatalog.Common.Category.Request;

namespace ProductCatalog.Client.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            if (!products.IsSuccess) return BadRequest(products);
            return View(products.ResponseData);
        }

        public async Task<IActionResult> Create()
        {
            // Load categories from API (can also abstract this into a CategoryService)
            var categories = await _categoryService.GetAllAsync(new GetAllCategoryDTO());
            ViewBag.Categories = new SelectList(categories.Result, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductReqDTO model)
        {
            var success = await _productService.CreateProductAsync(model);
            if (success.IsSuccess) return RedirectToAction(nameof(Index));

            ViewBag.Error = "Could not create product.";
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (!product.IsSuccess) return NotFound();
            var categories = await _categoryService.GetAllAsync(new GetAllCategoryDTO());
            ViewBag.Categories = new SelectList(categories.Result, "Id", "Name", product.ResponseData.CategoryId);
            return View(product.ResponseData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductReqDTO model)
        {
            var success = await _productService.UpdateProductAsync(model);
            if (success.IsSuccess) return RedirectToAction(nameof(Index));

            ViewBag.Error = "Could not update product.";
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (!product.IsSuccess) return NotFound();
            return View(product.ResponseData);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (success.IsSuccess) return RedirectToAction(nameof(Index));

            var product = await _productService.GetProductByIdAsync(id);
            ViewData["ErrorMessage"] = "Could not delete product.";
            return View("Delete", product);
        }
    }

}
