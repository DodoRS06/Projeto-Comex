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
            var dados = await _context.AgenteDeCargas
                .OrderBy(a => a.NomeAgenteCarga)
                .ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AgenteDeCarga agenteDeCarga)
        {
            if (ModelState.IsValid)
            {
                var agenteExistente = await _context.AgenteDeCargas
                    .AnyAsync(a => a.NomeAgenteCarga == agenteDeCarga.NomeAgenteCarga);

                if (agenteExistente)
                { 
                    ModelState.AddModelError("NomeAgenteCarga", "Já existe um agente de carga com esse nome.");
                    return View(agenteDeCarga);
                }

                _context.AgenteDeCargas.Add(agenteDeCarga);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(agenteDeCarga);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.AgenteDeCargas.FindAsync(id);
            if (dados == null)
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


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.AgenteDeCargas.FindAsync(id);

            if (id == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.AgenteDeCargas.FindAsync(id);

            if (id == null)
                return NotFound();

            _context.AgenteDeCargas.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
