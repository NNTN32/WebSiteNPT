using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShopNPT.Models;


public class WebSiteDacsContext : IdentityDbContext<User>
{
    public WebSiteDacsContext(DbContextOptions<WebSiteDacsContext>options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Brand> Brands { get; set; }
}
