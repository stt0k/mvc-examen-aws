using Microsoft.EntityFrameworkCore;

namespace mvc_examen_aws.Models
{
    public class ZapatillaContext : DbContext
    {
        public ZapatillaContext(DbContextOptions<ZapatillaContext> options) : base(options) { }

        public DbSet<Zapatilla> Zapatillas { get; set; }
    }
}
