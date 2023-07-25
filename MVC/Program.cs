using Library.DAL.Abstractions;
using Library.DAL.EF;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
{
    options.Cookie.Name = "CookieAuth";
    options.LoginPath = "/Users/Login";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StaffPolicy", policy =>
    policy.RequireClaim("UserType","staff"));
});

builder.Services.AddScoped<IBookManager, BookManager>();
builder.Services.AddScoped<IReservationManager, ReservationManager>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();


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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "UsersRoute",
    pattern: "{controller=Users}/{action=Login}");



app.Run();
