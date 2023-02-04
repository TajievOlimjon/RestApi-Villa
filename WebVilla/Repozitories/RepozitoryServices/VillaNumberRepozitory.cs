using WebVilla.Data;
using WebVilla.Models;
using WebVilla.Repozitories.Repozitory;

namespace WebVilla.Repozitories.RepozitoryServices
{
    public class VillaNumberRepozitory:RepozitoryService<VillaNumber>,IVillaNumberRepozitory
    {
        private readonly ApplicationContext _context;
        public VillaNumberRepozitory(ApplicationContext context):base(context)
        {
            _context = context;
        }
        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.VillaNumbers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
