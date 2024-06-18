using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class DespachosController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public DespachosController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }

        // GET : Despachos
        public async Task<IActionResult> Index()
        {
            try
            {

                var dados = await _context.Despachos
                     .OrderByDescending(p => p.Id)
                     .ToListAsync();

                return View(dados);
            }
            catch(Exception)
            {
               
                return _error.InternalServerError();
            }
        }

        // GET : Despachos/Create
        public IActionResult Create(int? id)
        {
            try
            {
                if (id == null)
                {
                    return _error.NotFoundError();
                }

                ViewData["ProcessoId"] = id.Value;

                return View();
            }
            catch(Exception)
            {
                return _error.InternalServerError();
            }
        }

        // POST : Despachos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Despacho despacho)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var despachoExistente = await _context.Despachos
                        .AnyAsync(a => a.NumeroDue == despacho.NumeroDue);

                    if (despachoExistente)
                    {
                        ModelState.AddModelError("NumeroDue", "Esse número DUE já está cadastrado.");
                        return View(despacho);
                    }

                    int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);

                    Despacho novoDespacho = new Despacho
                    {
                        ProcessoId = processoId,
                        NumeroDue = despacho.NumeroDue,
                        DataDue = despacho.DataDue,
                        DataAverbacao = despacho.DataAverbacao,
                        DataConhecimento = despacho.DataConhecimento,
                        DataExportacao = despacho.DataExportacao,
                        ConhecimentoEmbarque = despacho.ConhecimentoEmbarque,
                        CodPais = despacho.CodPais,
                        Tipo = despacho.Tipo,
                        Parametrizacao = despacho.Parametrizacao,

                    };

                    _context.Despachos.Add(novoDespacho);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = novoDespacho.ProcessoId });
                }


                return View(despacho);
            }
            catch(Exception)
            {
                return _error.InternalServerError();
            }
        }

        // GET: Despachos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Despachos.FindAsync(id);
                if (dados == null)
                    return _error.NotFoundError();

                ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");

                return View(dados);
            }
            catch(Exception)
            {
                return _error.InternalServerError();
            }

        }

        // POST: Despachos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Despacho despacho)
        {
            try
            {
                if (id != despacho.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    if (Request.Form.ContainsKey("ProcessoId"))
                    {
                        despacho.ProcessoId = Convert.ToInt32(Request.Form["ProcessoId"]);
                    }
                    _context.Despachos.Update(despacho);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = despacho.ProcessoId });
                }

                ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");
                return View();
            }
            catch
            {
                
                return View(despacho);
            }
        }

        // GET : Despachos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Despachos
                    .Include(d => d.Processo)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch(Exception)
            {
                return _error.InternalServerError();
            }
        }

        // GET: Despachos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();
                if (User.IsInRole("Admin"))
                {
                    var dados = await _context.Despachos
                    .Include(d => d.Processo)
                    .FirstOrDefaultAsync(d => d.Id == id);

                    if (dados == null)
                        return _error.NotFoundError();

                    return View(dados);
                }
                return _error.UnauthorizedError();
            }
            catch(Exception)
            {
                return _error.InternalServerError();
            }
        }

        // POST: Despachos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Despachos.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                if (User.IsInRole("Admin"))
                {
                    _context.Despachos.Remove(dados);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
                }

                return _error.UnauthorizedError();
            }
            catch(Exception)
            {
                return _error.InternalServerError();
            }
        }
    }
}
