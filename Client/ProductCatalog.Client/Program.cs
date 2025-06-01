using ProductCatalog.Service.Services;
using ProductCatalog.Service;
using ProductCatalog.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using ProductCatalog.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();


builder.Services.AddControllersWithViews();


builder.Services.AddRepository();
builder.Services.AddHttpClient<ICategoryService, CategoryService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7177/"); // Your API base URL
});

builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7177/"); // Your API base URL
});

builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7177/"); // Your API base URL
});

var app = builder.Build();

app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<UnauthorizedRedirectMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=login}/{id?}");

app.Run();
