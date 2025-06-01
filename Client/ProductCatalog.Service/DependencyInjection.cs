using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Service.Interfaces;
using ProductCatalog.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IAccountService, AccountService>();
            return services;
        }
    }
}
