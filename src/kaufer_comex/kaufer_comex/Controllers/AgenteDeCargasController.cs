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

        public async Task<IActionResult> Edit (int? id)
        {
            if (id == null)
                return  NotFound();

            var dados = await _context.AgenteDeCargas.FindAsync(id);
            if(dados == null)
                    return NotFound();

            return View(dados);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AgenteDeCarga agenteDeCarga)
        {
            if (id != agenteDeCarga.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.AgenteDeCargas.Update(agenteDeCarga);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.AgenteDeCargas.FindAsync(id);

            if (id == null)
                return NotFound();

            return View(dados);
        }
    }
}
