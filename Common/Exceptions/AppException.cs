namespace Common.Exceptions
{
    public class AppException : Exception
    {
        public ApiResultStatusCode ApiResultStatusCode { get; set; }

        public AppException(string message,ApiResultStatusCode apiResultStatusCode):base(message)
        {
            ApiResultStatusCode=apiResultStatusCode;
        }
    }
}
