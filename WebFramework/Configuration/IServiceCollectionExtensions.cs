using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace WebFramework.Configuration;

public static class IServiceCollectionExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var secretKey = Encoding.UTF8.GetBytes("this is my custom Secret key for authentication");
            var validationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.FromMinutes(0),

                ValidateIssuer = true,
                ValidIssuer = "MyWebsite",

                ValidateAudience = true,
                ValidAudience = "MyWebsite",

                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey)
            };
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = validationParameters;
        }); 
    }
}