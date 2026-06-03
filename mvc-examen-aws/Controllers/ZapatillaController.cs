using Microsoft.AspNetCore.Mvc;
using mvc_examen_aws.Models;
using mvc_examen_aws.Services;

namespace mvc_examen_aws.Controllers
{
    public class ZapatillaController : Controller
    {
        private readonly ZapatillaService _service;

        public ZapatillaController(ZapatillaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var zapatillas = await _service.GetAllAsync();
            return View(zapatillas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Zapatilla zapatilla, IFormFile? archivo)
        {
            if (!ModelState.IsValid)
                return View(zapatilla);

            await _service.CreateAsync(zapatilla, archivo);
            return RedirectToAction(nameof(Index));
        }
    }
}
