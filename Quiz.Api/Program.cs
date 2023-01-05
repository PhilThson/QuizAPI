using Quiz.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Quiz.Infrastructure.Interfaces;
using Quiz.Infrastructure.Services;
using Quiz.Data.Data;
using DinkToPdf;
using DinkToPdf.Contracts;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //do generowania dokumentów PDF za pomocą RazorView
        builder.Services.AddMvc().AddControllersAsServices();
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
        builder.Services.AddSingleton(typeof(IConverter),
            new SynchronizedConverter(new PdfTools()));

        var app = builder.Build();

        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //    app.Seed();
        //}
        app.UseSwagger();
        app.UseSwaggerUI();
        app.Seed();

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}