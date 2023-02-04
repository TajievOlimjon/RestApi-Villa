using System.Net;

namespace WebVilla.Responses
{
    public class APIResponse
    {
        public int  StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
