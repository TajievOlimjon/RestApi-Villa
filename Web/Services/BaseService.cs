using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using Villa.Services;
using Web.Responses;
using Web.Services.IServices;

namespace Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse ResponseModel { get; set; }
        public IHttpClientFactory _httpClientFactory { get; set; }
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.ResponseModel = new();
            _httpClientFactory = httpClientFactory;
        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("WebVilla");
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Headers.Add("Accept","application/json");
                httpRequestMessage.RequestUri = new Uri(apiRequest.Url);
                if(apiRequest.Data is not null)
                {
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");

                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        httpRequestMessage.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        httpRequestMessage.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        httpRequestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        httpRequestMessage.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage httpResponseMessage =null;
                httpResponseMessage = await client.SendAsync(httpRequestMessage);

                var apiContent=await httpResponseMessage.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponse;
            }
            catch(Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var response = JsonConvert.DeserializeObject<T>(res);
                return response;
            }
        }

    }
}
