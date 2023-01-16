using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Apartments.Data;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
   policy.RequireRole("Admin"));
});


// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Apartmentss");
    options.Conventions.AllowAnonymousToPage("/Apartmentss/Index");
    options.Conventions.AllowAnonymousToPage("/Apartmentss/Details");
    options.Conventions.AuthorizeFolder("/Clients", "AdminPolicy");
});
builder.Services.AddDbContext<ApartmentsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApartmentsContext") ?? throw new InvalidOperationException("Connection string 'ApartmentsContext' not found.")));

builder.Services.AddDbContext<ApartmentsIdentityContext>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("ApartmentsContext") ?? throw new InvalidOperationException("Connectionstring 'ApartmentContext' not found.")));
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
options.SignIn.RequireConfirmedAccount = true)
 .AddRoles<IdentityRole>()
 .AddEntityFrameworkStores<ApartmentsIdentityContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
