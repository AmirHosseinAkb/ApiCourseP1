using Common;
using Common.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace WebFramework.Api
{
    public class ApiResult
    {
        public string Message { get; set; }
        public bool IsSucceeded { get; set; }
        public ApiResultStatusCode  ApiResultStatusCode { get; set; }

        public ApiResult(bool isSucceeded,ApiResultStatusCode apiResultStatusCode,string? message= null)
        {
            IsSucceeded = isSucceeded;
            ApiResultStatusCode = apiResultStatusCode;
            Message = message ?? apiResultStatusCode.ToDisplay();
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, result.Content);
        }

        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success);
        }

        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.BadRequest);
        }

        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            string? message = null;
            if(result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(e => (string[])e.Value).Distinct();
                message = string.Join("|", errorMessages);
            }
            return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
        }

        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.NotFound);
        }

        public static implicit operator ApiResult(NotFoundObjectResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.NotFound, result.Value.ToString());
        }
    }

    public class ApiResult<TData>:ApiResult where TData : class
    {
        public TData Data { get; set; }
        public ApiResult(bool isSucceeded, ApiResultStatusCode apiResultStatusCode,TData data, string? message = null)
            :base(isSucceeded, apiResultStatusCode, message)
        {
            Data = data;
        }
        
        public static implicit operator ApiResult<TData> (TData data)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, data);
        }

        public static implicit operator ApiResult<TData>(ContentResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.Success,null, result.Content);
        }

        public static implicit operator ApiResult<TData>(OkResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, null);
        }
        public static implicit operator ApiResult<TData>(OkObjectResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, (TData)result.Value);
        }
        public static implicit operator ApiResult<TData>(BadRequestResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null);
        }
        public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        {
            string message = null;
            if(result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(e => (string[])e.Value);
                message = string.Join("|", errorMessages);
            }
            return new ApiResult<TData>(false,ApiResultStatusCode.BadRequest,(TData)result.Value,message);
        }

        public static implicit operator ApiResult<TData>(NotFoundResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, null);
        }
        public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        {
            return new ApiResult<TData>(false,ApiResultStatusCode.NotFound,(TData)result.Value,null);
        }
    }
}
