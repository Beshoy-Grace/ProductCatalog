using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Infrastructure.Data;
using System.Text;

namespace ProductCatalog.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection Configurations(this IServiceCollection services, ConfigurationManager configuration)
        {
             services.AddAutoMapper(typeof(ProductCatalog.Application.Mappings.MappingProfile));

            services.AddDbContext<ProductCatalogContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
                )
            );


            #region Identity Configuration

            services.AddIdentity<IdentityUser, IdentityRole>()
             .AddEntityFrameworkStores<ProductCatalogContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });
            #endregion
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Configure CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            return services;
        }
    }
}
