using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.IServices;
using ProductCatalog.Application.Services;
using ProductCatalog.Framework.Logging.Extensions;
using ProductCatalog.Framework.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddExceptionLogging();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            return services;
        }
    }
}
