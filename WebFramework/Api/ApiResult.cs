using Common;
using Common.Utilities;
using System.ComponentModel.DataAnnotations;

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
    }

    public class ApiResult<TData>:ApiResult where TData : ApiResult
    {
        public TData Data { get; set; }
        public ApiResult(bool isSucceeded, ApiResultStatusCode apiResultStatusCode,TData data, string? message = null)
            :base(isSucceeded, apiResultStatusCode, message)
        {
           
        }
    }
}
