using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class AgenteDeCargasController : Controller
    {
        private readonly AppDbContext _context;

        public AgenteDeCargasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.AgenteDeCargas.ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AgenteDeCarga agenteDeCarga)
        {
            if(ModelState.IsValid)
            {
                _context.AgenteDeCargas.Add(agenteDeCarga);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(agenteDeCarga);
        }
    }
}
