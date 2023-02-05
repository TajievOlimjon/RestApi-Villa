using Web.Models.DTOs.VillaDTOs;
using Web.Responses;
using Web.Services.IServices;
using static Villa.Services.SD;

namespace Web.Services
{
    public class VillaService :BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public string _villaUrl;
        public VillaService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");
        }
        public async Task<T> CreateAsync<T>(CreateVillaDto villaCreatedDto)
        {
            var apiRequest =await SendAsync<T>( new APIRequest
            {
                ApiType=ApiType.POST,
                Data=villaCreatedDto,
                Url=_villaUrl+ "/api/Villa"
            });

            return apiRequest;
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            var apiRequest = await SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.DELETE,
                Url = _villaUrl + "/api/Villa/"+id
            });

            return apiRequest;
        }

        public async Task<T> GetAllAsync<T>()
        {
            var apiRequest = await SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = _villaUrl + "/api/Villa"
            });

            return apiRequest;
        }

        public async Task<T> GetAsync<T>(int id)
        {
            var apiRequest = await SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = _villaUrl + "/api/Villa/" + id
            });

            return apiRequest;
        }

        public async Task<T> UpdateAsync<T>(UpdateVillaDto updateVillaDto)
        {
            var apiRequest = await SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.PUT,
                Data = updateVillaDto,
                Url = _villaUrl + "/api/Villa"
            });

            return apiRequest;
        }
    }
}
