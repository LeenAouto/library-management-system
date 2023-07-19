using Library.DAL.EF;
using Library.ConsoleUI.Display;
using Library.Entities;
using System;
using Library.DAL.Abstractions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Library.ConsoleUI;
using Microsoft.EntityFrameworkCore;
//==============================================================================

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables();
}

var builder = new ConfigurationBuilder();
BuildConfig(builder);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Build())
    .Enrich.FromLogContext()
    .WriteTo.File(
    "D:\\Program Files\\source\\repos\\LibraryConsole\\Library.Helper\\LogFiles\\log.txt", 
    rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Logger.Information("Starting the app");

var host = Host.CreateDefaultBuilder()
    .ConfigureServices( services =>
    {
        services.AddSingleton<IBookManager, BookManager>();
        services.AddSingleton<IUserManager, UserManager>();
        services.AddSingleton<IReservationManager, ReservationManager>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        //services.AddSingleton<DbContext, LibraryDbContext>();
        services.AddSingleton<IDisplay, Display>();
        services.AddSingleton<IApp, App>();
        //services.AddDbContext<LibraryDbContext>(); //Learn This
    })
    .UseSerilog().Build();

var svc = host.Services.GetRequiredService<IApp>();
svc.Run();
