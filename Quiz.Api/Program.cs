using Quiz.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Quiz.Infrastructure.Interfaces;
using Quiz.Infrastructure.Services;
using Quiz.Data.Data;
using System.Runtime.InteropServices;
using Quiz.Api.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Quiz.Api.Utilities;
using DinkToPdf.Contracts;
using DinkToPdf;
using Quiz.Infrastructure.Logging;
using Quiz.Infrastructure.Helpers;
using Quiz.Data.Helpers;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // generowanie dokumentów PDF za pomocą RazorView
        builder.Services.AddMvc().AddControllersAsServices();

        // ładowanie zewnętrznych bibliotek
        var wkHtmlToPdfFileName = "libwkhtmltox";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            wkHtmlToPdfFileName += ".so";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            wkHtmlToPdfFileName += ".dylib";

        var wkHtmlToPdfPath = Path.Combine(
            new string[] { builder.Environment.ContentRootPath, wkHtmlToPdfFileName });

        var context = new CustomAssemblyLoadContext();
        context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(),
            wkHtmlToPdfPath));

        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition
                = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<QuizDbContext>(o =>
            o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

        builder.Services.AddScoped<IDataService, DataService>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddScoped<IRazorRendererService, RazorRendererService>();

        builder.Services.Configure<HostOptions>(config =>
            config.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);

        builder.Services.AddHostedService<LoggerBackgroundService>();
        builder.Services.AddSingleton<IBackgroundJobQueue, BackgroundJobQueue>();

        //Wykorzystanie biblioteki DinkToPdf jako wrappera na 'wkhtmltopdf'
        //silnika do zamiany kodu html na dokkument PDF
        //(wkhtmltopdf - command line tools to render HTML into PDF and various
        //image formats using the Qt WebKit rendering engine)
        builder.Services.AddSingleton(typeof(IConverter),
            new SynchronizedConverter(new PdfTools()));

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Cookie.HttpOnly = true;
                o.LoginPath = "/api/user/login";
                o.LogoutPath = "/api/user/logout";
                o.Cookie.Name = QuizConstants.UserCookieName;
            });

        builder.Services.AddSingleton<ILoggerProvider, QuizLoggerProvider>();

        var app = builder.Build();

        //Zakomentowane - bo były problemy przy uruchamianiu w Dockerze
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        app.Seed();
        //}

        //app.UseHttpsRedirection();
        app.HandleExceptions();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}