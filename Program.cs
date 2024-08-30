using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RYT.Data;
using RYT.Models.Entities;
using RYT.Services.CloudinaryService;
using RYT.Services.Emailing;
using RYT.Services.Payment;
using RYT.Services.PaymentGateway;
using RYT.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddDbContext<RYTDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("default")));
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<RYTDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IEmailService, Emailing>();

builder.Services.AddScoped<IPaymentService, PayStackService>();
builder.Services.AddScoped<IPayments, Payments>();

builder.Services.AddScoped<IPhotoService, PhotoService>();


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

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

Seeder.SeedeMe(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
