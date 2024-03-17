using Microsoft.AspNetCore.Mvc;
using kaufer_comex.Models;
using Microsoft.EntityFrameworkCore;
namespace kaufer_comex.Controllers
{
    public class DespachanteController : Controller
    {
        private readonly AppDbContext _context;

        public DespachanteController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.Despachantes.ToListAsync();

            return View(dados);
        }

    }
}