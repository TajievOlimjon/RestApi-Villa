using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebVilla.Data;
using WebVilla.Models;
using WebVilla.Repozitories.Repozitory;

namespace WebVilla.Repozitories.RepozitoryServices;

public class VillaRepozitory :RepozitoryService<Villa>, IVillaRepozitory
{
    private readonly ApplicationContext _context; 
    public VillaRepozitory(ApplicationContext context):base(context)
    {
        _context=context;
    }
    public async Task<Villa> UpdateAsync(Villa entity)
    {
        entity.UpdatedAt=DateTime.UtcNow;
       _context.Villas.Update(entity);
       await _context.SaveChangesAsync();
       return entity;
    }
}
