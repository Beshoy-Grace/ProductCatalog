using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Common.Category.Request;
using ProductCatalog.Common.Category.Response;
using ProductCatalog.Service.Interfaces;
using ProductCatalog.Service.Services;
using System.Net.Http;

public class CategoryController : Controller
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(GetAllCategoryDTO req)
    {
        var request = req ?? new GetAllCategoryDTO();
        var categories = await _service.GetAllAsync(request);
        return View(categories);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryReqDTO category)
    {
        if (!ModelState.IsValid) return View(category);
        await _service.CreateAsync(category);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateCategoryReqDTO category)
    {
        if (!ModelState.IsValid) return View(category);
        await _service.UpdateAsync(category);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
       var result =  await _service.DeleteAsync(id);
        if(!result.IsSuccess)
        {
            ViewData["ErrorMessage"] = result?.Errors?.FirstOrDefault()?.Value ?? "An unexpected error occurred.";
            var category = await _service.GetByIdAsync(id);
            return View(category);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var category = await _service.GetByIdAsync(id); // You'll need this method
        if (category == null)
            return NotFound();

        return View(category);
    }

}
