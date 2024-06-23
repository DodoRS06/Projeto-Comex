using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class AgenteDeCargasController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public AgenteDeCargasController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
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
            catch (Exception)
            {
                return _error.InternalServerError();
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
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.AgenteDeCargas.FindAsync(id);
                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
 
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AgenteDeCarga agenteDeCarga)
        {
            try
            {
                if (id != agenteDeCarga.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    _context.AgenteDeCargas.Update(agenteDeCarga);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch(Exception)
            {
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.AgenteDeCargas.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch { return _error.InternalServerError(); }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();
                if (User.IsInRole("Admin"))
                {
                    var dados = await _context.AgenteDeCargas.FindAsync(id);

                    if (dados == null)
                        return _error.NotFoundError();

                    return View(dados);
                }
                return _error.UnauthorizedError();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.AgenteDeCargas.FindAsync(id);

                if (id == null)
                    return _error.NotFoundError();

                _context.AgenteDeCargas.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException )
            {
                TempData["MensagemErro"] = $"Esse Agente está vinculado a um processo e não pode ser excluído. ";
                return View();
            }

        }
    }
}
