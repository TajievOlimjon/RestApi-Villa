using System.Net;

namespace Web.Responses
{
    public class APIResponse
    {
        public int  StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;        
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
