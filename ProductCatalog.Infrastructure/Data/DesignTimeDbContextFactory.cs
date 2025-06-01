using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProductCatalog.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductCatalogContext>
    {
        public ProductCatalogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductCatalogContext>();

            // Use the connection string configured for your Azure SQL Database
            optionsBuilder.UseSqlServer("Data Source=192.168.1.85;Initial Catalog=ProductCatalogDB;User ID=SJM_FVDBUser;Password=S@raSJM328311;Integrated Security=False;Trust Server Certificate=True;Encrypt=False");

            return new ProductCatalogContext(optionsBuilder.Options);
        }
    }
}
