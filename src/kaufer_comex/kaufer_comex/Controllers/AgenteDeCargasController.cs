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
            try
            {
                var dados = await _context.AgenteDeCargas
                    .OrderBy(a => a.NomeAgenteCarga)
                    .ToListAsync();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Erro ao carregar os dados. Tente novamente";
                return View();
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AgenteDeCarga agenteDeCarga)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var agenteExistente = await _context.AgenteDeCargas
                        .AnyAsync(a => a.NomeAgenteCarga == agenteDeCarga.NomeAgenteCarga);

                    if (agenteExistente)
                    {
                        TempData["MensagemErro"] = $"Já existe esse agente cadastrado";
                        return View(agenteDeCarga);
                    }

                    _context.AgenteDeCargas.Add(agenteDeCarga);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(agenteDeCarga);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.AgenteDeCargas.FindAsync(id);
                if (dados == null)
                    return NotFound();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AgenteDeCarga agenteDeCarga)
        {
            try
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
            catch {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.AgenteDeCargas.FindAsync(id);

                if (id == null)
                    return NotFound();

                return View(dados);
            }
            catch
            {
                return NotFound();  
            }
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
            try
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
            catch {
                TempData["MensagemErro"] = $"Este agente está vinculado a um embarque. Não pode ser excluído";
                return View();
            }

        }
    }
}
