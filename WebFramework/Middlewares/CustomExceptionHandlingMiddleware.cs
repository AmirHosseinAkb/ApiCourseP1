using Common;
using Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebFramework.Api;

namespace WebFramework.Middlewares
{
    public static class MiddleWaresExtensionMethods
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app) =>
            app.UseMiddleware<CustomExceptionHandlingMiddleware>();
    }
    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlingMiddleware> _logger;
        public CustomExceptionHandlingMiddleware(RequestDelegate next,ILogger<CustomExceptionHandlingMiddleware> logger)
        {
            _next=next;
            _logger=logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (AppException exception)
            {
                _logger.LogError(exception.Message);
                var apiResult = new ApiResult(false, exception.ApiResultStatusCode,exception.Message);
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(apiResult));
            }
            catch (Exception e)
            {
                _logger.LogError("an error occurred");
                var apiResult = new ApiResult(false, ApiResultStatusCode.ServerError);
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(apiResult));
            }
        }
    }
}
