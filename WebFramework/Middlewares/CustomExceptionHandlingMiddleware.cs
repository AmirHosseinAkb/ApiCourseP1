using System;
using System.Net;
using Common;
using Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebFramework.Api;

namespace WebFramework.Middlewares
{
    public static class MiddleWaresExtensionMethods
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app) =>
            app.UseMiddleware<CustomExceptionHandlingMiddleware>();
    }
    public class CustomExceptionHandlingMiddleware //This MiddleWare Just Works For Errors That Thrown
                                                   //Not When We Return An Error
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public CustomExceptionHandlingMiddleware(RequestDelegate next,ILogger<CustomExceptionHandlingMiddleware> logger
            ,IHostEnvironment env)
        {
            _next=next;
            _logger=logger;
            _env = env;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            ApiResultStatusCode apiStatusCode = ApiResultStatusCode.ServerError;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            string message = null;
            try
            {
                await _next(httpContext);
            }
            catch (AppException exception)
            {
                _logger.LogError(exception.Message);
                if (_env.IsDevelopment())
                {
                    apiStatusCode = exception.ApiStatusCode;
                    httpStatusCode = exception.HttpStatusCode;
                    var dic = new Dictionary<string, string>()
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace
                    };
                    if (exception.InnerException != null)
                    {
                        dic["InnerException.Message"] = exception.InnerException.Message;
                        dic["InnerException.StackTrace"] = exception.InnerException.StackTrace;
                    }

                    if (exception.AdditionalData != null)
                        dic["AdditionalData"] = JsonConvert.SerializeObject(exception.AdditionalData);
                    message = JsonConvert.SerializeObject(dic);
                }
                else
                {
                    message = exception.Message;
                }

                await WriteToResponseAsync();
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogError(ex, ex.Message);
                await SetUnAuthorizedResponseAsync(ex);
                await WriteToResponseAsync();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, ex.Message);
                await SetUnAuthorizedResponseAsync(ex);
                await WriteToResponseAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>()
                    {
                        ["Exception"] = ex.Message,
                        ["StackTrace"] = ex.StackTrace
                    };
                    if (ex.InnerException != null)
                    {
                        dic["InnerException.Message"] = ex.InnerException.Message;
                        dic["InnerException.StackTrace"] = ex.InnerException.StackTrace;
                    }

                    message = JsonConvert.SerializeObject(dic);
                }
                await WriteToResponseAsync();
            }
            

            async Task WriteToResponseAsync()
            {
                if (httpContext.Response.HasStarted)
                    await httpContext.Response.WriteAsync("The Response Has Sent In Advance");
                var apiResult = new ApiResult(false, apiStatusCode, message);
                var json = JsonConvert.SerializeObject(apiResult);
                httpContext.Response.ContentType="application/json";
                httpContext.Response.StatusCode=(int)httpStatusCode;
                await httpContext.Response.WriteAsync(json);
            }

            async Task SetUnAuthorizedResponseAsync(Exception ex)
            {
                apiStatusCode = ApiResultStatusCode.UnAuthorized;
                httpStatusCode = HttpStatusCode.Unauthorized;
                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>()
                    {
                        ["Exception"] = ex.Message,
                        ["StackTrace"] = ex.StackTrace
                    };
                    if (ex.InnerException != null)
                    {
                        dic["InnerException.Message"] = ex.InnerException.Message;
                        dic["InnerException.StackTrace"] = ex.InnerException.StackTrace;
                    }

                    if (ex is SecurityTokenExpiredException exception)
                        dic["SecurityTokenExpireDate"]=exception.Expires.ToString("yy-MM-dd");

                    message = JsonConvert.SerializeObject(dic);
                }
            }
        }
    }
}
