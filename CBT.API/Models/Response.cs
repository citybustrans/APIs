namespace CBT.API.Models
{
    public class Response<T>
    {
        public string ResponseCode { get; set; } = string.Empty;
        public string ResponseMessage { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
