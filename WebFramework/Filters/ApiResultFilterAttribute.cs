﻿using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebFramework.Api;

namespace WebFramework.Filters
{
    public class ApiResultFilterAttribute:ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult)
            {
                
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, okObjectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = okObjectResult.StatusCode };
            }
            else if (context.Result is OkResult okResult)
            {
                var apiResult = new ApiResult(true, ApiResultStatusCode.Success);
                context.Result = new JsonResult(apiResult) { StatusCode = okResult.StatusCode };
            }
            else if (context.Result is BadRequestObjectResult badRequestObjectResult)
            {
                var message = badRequestObjectResult.Value.ToString();
                if (badRequestObjectResult.Value is ValidationProblemDetails errors)
                {
                    var errorMessages = errors.Errors.SelectMany(p => (string[])p.Value).Distinct();
                    message = string.Join(" | ", errorMessages);
                }
                var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest, message);
                context.Result = new JsonResult(apiResult) { StatusCode = badRequestObjectResult.StatusCode };
            }
            else if (context.Result is NotFoundObjectResult notFoundObjectResult)
            {
                var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
            }
            else if(context.Result is ObjectResult objectResult1 && objectResult1.Value is ProblemDetails problemDetails)
            {
                if (problemDetails.Status == 400)
                {
                    var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest);
                    context.Result = new JsonResult(apiResult) { StatusCode = 400 };
                }
                else if (problemDetails.Status == 404)
                {
                    var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound);
                    context.Result = new JsonResult(apiResult) { StatusCode = 404 };
                }
            }
            else if (context.Result is ContentResult contentResult)
            {
                var apiResult = new ApiResult(true, ApiResultStatusCode.Success, contentResult.Content);
                context.Result = new JsonResult(apiResult) { StatusCode = contentResult.StatusCode };
            }
            
            else if (context.Result is ObjectResult objectResult && objectResult.StatusCode == null
                && !(objectResult.Value is ApiResult))
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = objectResult.StatusCode };
            }
            base.OnResultExecuting(context);
        }
    }
}
