using Quiz.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Quiz.Infrastructure.Interfaces;
using Quiz.Infrastructure.Services;
using Quiz.Data.Data;
using System.Runtime.InteropServices;
using Quiz.Api.Extensions;

internal class Program
{
    const string AuthSchema = "cookie";

    private static void Main(string[] args)
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
            new string[] {builder.Environment.ContentRootPath, wkHtmlToPdfFileName});

        //var context = new CustomAssemblyLoadContext();
        //context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(),
        //    wkHtmlToPdfPath));

        //generowanie odpowiedzi Json
        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition
            = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<QuizDbContext>(o =>
            o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

        builder.Services.AddScoped<IDataService, DataService>();
        //builder.Services.AddScoped<IDocumentService, DocumentService>();
        //builder.Services.AddScoped<IRazorRendererService, RazorRendererService>();

        //Wykorzystanie biblioteki DinkToPdf jako wrappera na 'wkhtmltopdf'
        //silnika do zamiany kodu html na dokkument PDF
        //wkhtmltopdf - command line tools to render HTML into PDF and various
        //image formats using the Qt WebKit rendering engine
        //builder.Services.AddSingleton(typeof(IConverter),
        //    new SynchronizedConverter(new PdfTools()));

        //bez ustawienia domyślngeo typu/polityki uwierzytelniania,
        //Authorization Middleware nie wiedzialby, ktora polityke ma testowac
        builder.Services.AddAuthentication(AuthSchema)
            //bez podania nazwy, bedzie 'Cookies'
            .AddCookie(AuthSchema, o => o.LoginPath = "/api/user/login");

        //Brak uwierzytelnienie spowoduje przekierowanie na podany endpoint


        //ciastko jest wysylane przy kazdym zadaniu do tej samej domeny

        //builder.Services.AddAuthorization();
        //    c.AddPolicy("user", p =>
        //        p.AddAuthenticationSchemes(AuthSchema)
        //        .RequireClaim("user")));

        var app = builder.Build();

        //Zakomentowane - bo były problemy przy uruchamianiu w Dockerze
        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI();
            app.Seed();
        //}

        app.UseHttpsRedirection();

        app.UseAuthentication();
        //Po dodaniu atrybutu Authorize rzucalo 500tka
        //dlatego dodane zostalo authorization
        //to dlatego ze ClaimsIdentity musi posiadac zgodny Authentication Type
        //Odpowiedz 404 bieze sie z domyslnego przekierowania (302) na strone logowania
        //Microsoftu, ale API go nie posiada wiec jest 404
        //Doslownie:
        //https://localhost:7011/Account/Login?ReturnUrl=/api/User/data
        app.UseAuthorization();

        app.HandleExceptions();

        app.MapControllers();

        app.Run();
    }
}