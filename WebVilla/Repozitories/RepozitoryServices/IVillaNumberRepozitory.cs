using WebVilla.Models;
using WebVilla.Repozitories.Repozitory;

namespace WebVilla.Repozitories.RepozitoryServices
{
    public interface IVillaNumberRepozitory:IRepozitoryService<VillaNumber>
    {
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
    }
}
