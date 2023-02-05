using Web.Models.DTOs.VillaDTOs;

namespace Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(CreateVillaDto villaCreatedDto);
        Task<T> UpdateAsync<T>(UpdateVillaDto updateVillaDto);
        Task<T> DeleteAsync<T>(int id);
    }
}
