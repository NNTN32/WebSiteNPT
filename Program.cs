using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebDACS.Repositories;
using WebShopNPT.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebSiteDacsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>()
        .AddDefaultTokenProviders()
        .AddDefaultUI()
        .AddEntityFrameworkStores<WebSiteDacsContext>();

builder.Services.AddRazorPages();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProduct, EFProductRepo>();
builder.Services.AddScoped<ICategory, EFCategoryRepo>();
builder.Services.AddScoped<IBrand, EFBrandRepo>();
var app = builder.Build();

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
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
