using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AquariumForum.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Register Database Context
builder.Services.AddDbContext<AquariumForumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AquariumForumContext") ??
    throw new InvalidOperationException("Connection string 'AquariumForumContext' not found.")));

// 🔥 Register Identity with ApplicationUser (Fixes the error)
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // Optional: Add roles
    .AddDefaultUI() // Ensures Identity UI pages (Login, Register, etc.) are available
    .AddEntityFrameworkStores<AquariumForumContext>();

// 🔥 Explicitly Register SignInManager & UserManager (Fix for missing service)
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// 🔥 ORDER MATTERS! Authentication MUST come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages().WithStaticAssets();

app.Run();
