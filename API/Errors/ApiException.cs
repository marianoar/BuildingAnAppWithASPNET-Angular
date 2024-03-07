namespace API.Errors
{
    //this is going to contain the response of what we going to send back to the client
    public class ApiException
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public ApiException(int statusCode, string message, string details)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
