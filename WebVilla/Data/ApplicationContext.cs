using Microsoft.EntityFrameworkCore;
using WebVilla.Models;

namespace WebVilla.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
    }
}
