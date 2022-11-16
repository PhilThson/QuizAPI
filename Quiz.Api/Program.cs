using Quiz.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Quiz.Infrastructure.Interfaces;
using Quiz.Infrastructure.Services;
using Quiz.Data.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<QuizDbContext>(o =>
            o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
        builder.Services.AddScoped<IDataService, DataService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.Seed();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}