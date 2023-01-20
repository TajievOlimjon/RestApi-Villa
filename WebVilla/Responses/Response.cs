namespace WebVilla.Responses
{
    public class Response<T>
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public Response(int code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
        public Response(int code, string message)
        {
            Code = code;
            Message = message;
        }
        public Response()
        {

        }
    }
}
