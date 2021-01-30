namespace Shared
{
    public class RequestFailure
    {
        public string Message { get; set; }

        public RequestFailure(string message)
        {
            Message = message;
        }
    }
}