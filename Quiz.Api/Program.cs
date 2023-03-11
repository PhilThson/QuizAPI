﻿using Quiz.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Quiz.Infrastructure.Interfaces;
using Quiz.Infrastructure.Services;
using Quiz.Data.Data;
using System.Runtime.InteropServices;
using Quiz.Api.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using NLog;
using NLog.Web;
using Quiz.Api.Utilities;
using DinkToPdf.Contracts;
using DinkToPdf;

internal class Program
{
    private static void Main(string[] args)
    {
        var logger = NLog.LogManager
            .Setup()
            .LoadConfigurationFromAppSettings()
            .GetCurrentClassLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            //do generowania dokumentów PDF za pomocą RazorView
            builder.Services.AddMvc().AddControllersAsServices();

            //Ładowanie zewnętrznych bibliotek
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

            //generowanie odpowiedzi Json
            builder.Services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition
                = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<QuizDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<IDataService, DataService>();
            builder.Services.AddScoped<IDocumentService, DocumentService>();
            builder.Services.AddScoped<IRazorRendererService, RazorRendererService>();

            //Wykorzystanie biblioteki DinkToPdf jako wrappera na 'wkhtmltopdf'
            //silnika do zamiany kodu html na dokkument PDF
            //wkhtmltopdf - command line tools to render HTML into PDF and various
            //image formats using the Qt WebKit rendering engine
            builder.Services.AddSingleton(typeof(IConverter),
                new SynchronizedConverter(new PdfTools()));

            //bez ustawienia domyślngeo typu/polityki uwierzytelniania,
            //Authorization Middleware nie wiedzialby, ktora polityke ma testowac
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                //bez podania nazwy, bedzie 'Cookies'
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.Cookie.HttpOnly = true;
                    o.LoginPath = "/api/user/login";
                    o.LogoutPath = "/api/user/logout";
                    o.Cookie.Name = "quiz-user";
                });

            builder.Logging
                .ClearProviders()
                .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);

            builder.Host.UseNLog();

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
        catch (Exception ex)
        {
            logger.Error(ex);
            throw;
        }
        finally
        {
            NLog.LogManager.Shutdown();
        }
    }
}