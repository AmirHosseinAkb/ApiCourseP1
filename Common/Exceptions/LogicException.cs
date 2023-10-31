namespace Common.Exceptions
{
    public class LogicException:AppException
    {
        public LogicException(string message) : base(message, ApiResultStatusCode.LogicError)
        {
        }
    }
}
