using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Common.Product.Request;
using System.Net.Http;
using ProductCatalog.Service.Interfaces;
using ProductCatalog.Common.Category.Request;
using System.Text.Json;
using System.Text;
using ProductCatalog.Common.User.Request;

namespace ProductCatalog.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginReqDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.LoginAsync(model);
            if (result.IsSuccess)
            {
                HttpContext.Session.SetString("Token", result.ResponseData.Token);
                return RedirectToAction("Index", "Category");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateUserDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

         var response = await _accountService.CreateUserAsync(model);

            if (response.IsSuccess)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed.");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Unauthorized()
        {
            return View();
        }
    }

}
