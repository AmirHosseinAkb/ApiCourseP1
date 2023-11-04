
using Data;
using Data.Contracts;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Jwt;
using WebFramework.Configuration;
using WebFramework.Filters;
using WebFramework.Middlewares;

namespace ApiServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ApiLocalConnection"))
            ) ;

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddJwtAuthentication();
            var app = builder.Build();

            app.UseCustomExceptionHandler();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}