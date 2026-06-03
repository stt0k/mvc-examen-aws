using Microsoft.EntityFrameworkCore;
using mvc_examen_aws.Models;

namespace mvc_examen_aws.Repositories
{
    public class ZapatillaRepository
    {
        private readonly ZapatillaContext _context;

        public ZapatillaRepository(ZapatillaContext context)
        {
            _context = context;
        }

        public async Task<List<Zapatilla>> GetAllAsync()
        {
            return await _context.Zapatillas.OrderByDescending(z => z.IdProducto).ToListAsync();
        }

        public async Task CreateAsync(Zapatilla zapatilla)
        {
            _context.Zapatillas.Add(zapatilla);
            await _context.SaveChangesAsync();
        }
    }
}
