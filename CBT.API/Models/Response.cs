namespace CBT.API.Models
{
    public class Response
    {
        public string ResponseCode { get; set; } = string.Empty;
        public string ResponseMessage { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
