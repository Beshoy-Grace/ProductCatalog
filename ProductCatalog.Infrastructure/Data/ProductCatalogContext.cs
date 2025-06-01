using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;


namespace ProductCatalog.Infrastructure.Data
{
    public class ProductCatalogContext : IdentityDbContext<IdentityUser>
    {


        private readonly IHttpContextAccessor _context;

        public ProductCatalogContext(DbContextOptions<ProductCatalogContext> options, IHttpContextAccessor context) : base(options)
        {
            _context = context;
        }
        public ProductCatalogContext()
        {

        }
        public ProductCatalogContext(DbContextOptions<ProductCatalogContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            base.OnModelCreating(modelBuilder);
          
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var _userId = "";
            if (_context.HttpContext != null)
            {
                _userId = _context.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (_userId == null)
                {
                    _userId = string.Empty;
                }
            }
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries())
            {
                foreach (var property in entry.Properties)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            if (entry.Metadata.FindProperty("CreatedBy") != null)
                            {
                                entry.Property("CreatedBy").CurrentValue = !string.IsNullOrEmpty(_userId) ? new Guid(_userId) : null;
                            }
                            if (entry.Metadata.FindProperty("CreatedOn") != null)
                            {
                                entry.Property("CreatedOn").CurrentValue = DateTime.UtcNow;
                            }
                            break;

                        case EntityState.Modified:
                            if (entry.Metadata.FindProperty("UpdatedBy") != null)
                            {
                                entry.Property("UpdatedBy").CurrentValue = !string.IsNullOrEmpty(_userId) ? new Guid(_userId) : null;
                            }
                            if (entry.Metadata.FindProperty("UpdatedOn") != null)
                            {
                                entry.Property("UpdatedOn").CurrentValue = DateTime.UtcNow;
                            }
                            break;
                    }
                }
            }

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
