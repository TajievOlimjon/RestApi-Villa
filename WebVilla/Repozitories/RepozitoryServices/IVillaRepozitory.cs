using System.Linq.Expressions;
using WebVilla.Models;
using WebVilla.Repozitories.Repozitory;

namespace WebVilla.Repozitories.RepozitoryServices;

public interface IVillaRepozitory:IRepozitoryService<Villa>
{ 
    Task<Villa> UpdateAsync(Villa entity);
}
