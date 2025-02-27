
using System.Text;
using DataloaderApi.Dao;
using DataloaderApi.DataRead;
using DataloaderApi.Extension;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            builder.Services.AddSwaggerAuth();

            var connectionString = builder.Configuration.GetConnectionString("dataloaderConnection");

            builder.Services.AddHangfire(configuration => configuration      
                  .UseSqlServerStorage(connectionString)
                   
                  );


            builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(connectionString));


            // Authentication 
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication();
            builder.Services.AddIdentityApiEndpoints<IdentityUser>()
            .AddEntityFrameworkStores<IdentityContext>();

            // Hangfire
            builder.Services.AddHangfireServer();
            // db config hangfire
            builder.Services.AddDbContextPool<Applicationcontext>(options =>

                options.UseSqlServer(connectionString)
               

                );


            //Dependency Injection
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddScoped(typeof(ICsvLoadDao<>), typeof(CsvLoaderDao<>));
            builder.Services.AddScoped(typeof(IAuthHandlingDao), typeof(AuthHandlingDao));
            builder.Services.AddScoped<DataProcess>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                    policy.WithOrigins("https://localhost:7046", "http://localhost:7046")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapIdentityApi<IdentityUser>();
            app.UseCors("AllowSpecificOrigins");
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseHangfireDashboard();

            app.MapControllers();

            app.Run();
        }
    }
}
