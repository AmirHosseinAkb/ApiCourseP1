
using Common;
using Data;
using Data.Contracts;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc.Authorization;
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

            SiteSettings siteSettings=builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>()!; 


            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ApiLocalConnection"))
            ) ;

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));
            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddJwtAuthentication(siteSettings);
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