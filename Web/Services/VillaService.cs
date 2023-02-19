using Web.Models.DTOs.VillaDTOs;
using Web.Responses;
using Web.Services.IServices;
using static Villa.Services.SD;

namespace Web.Services
{
    public class VillaService :BaseService, IVillaService
    {
        private readonly IHttpClientFactory _clientFactory;
        public string _villaUrl;
        public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory =clientFactory;
            _villaUrl = configuration.GetSection("ServiceUrls:WebVilla").Value;
        }
        public async Task<T> CreateAsync<T>(CreateVillaDto villaCreatedDto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType=ApiType.POST,
                Data=villaCreatedDto,
                Url=_villaUrl+ "/api/v1/villa"
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.DELETE,
                Url = _villaUrl + "/api/v1/Villa/"+id
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = _villaUrl + "/api/v1/Villa/GetVillas"
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = _villaUrl + "/api/v1/villa/" + id
            });
        }

        public async Task<T> UpdateAsync<T>(UpdateVillaDto updateVillaDto)
        {
            return await SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.PUT,
                Data = updateVillaDto,
                Url = _villaUrl + "/api/v1/villa"
            });
        }
    }
}
