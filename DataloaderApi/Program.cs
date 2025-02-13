
using DataloaderApi.Dao;
using DataloaderApi.DataRead;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
namespace DataloaderApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var connectionString = builder.Configuration.GetConnectionString("dataloaderConnection");

            builder.Services.AddHangfire(configuration => configuration      
                  .UseSqlServerStorage(connectionString)
                   
                  ); 

              builder.Services.AddHangfireServer();

            // db config hangfire
            builder.Services.AddDbContextPool<Applicationcontext>(options =>

                options.UseSqlServer(connectionString)
               

                );

            builder.Services.AddScoped(typeof(ICsvLoadDao<>), typeof(CsvLoaderDao<>));
            builder.Services.AddScoped<DataProcess>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(policy=>
            policy.WithOrigins("https://localhost:7046", "http://localhost:7046")
            .AllowAnyMethod()
            .AllowAnyHeader()

                );
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseHangfireDashboard();

            app.MapControllers();

            app.Run();
        }
    }
}
