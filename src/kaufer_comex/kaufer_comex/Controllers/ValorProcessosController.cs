using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class ValoresProcessosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorService _error;

        public ValoresProcessosController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error; 
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.ValorProcessos
                    .ToListAsync();

                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
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
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ValorProcesso valorprocesso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);
                    decimal valorExw = valorprocesso.ValorExw ?? 0;
                    decimal valorFobFca = valorprocesso.ValorFobFca ?? 0;
                    decimal freteInternacional = valorprocesso.FreteInternacional ?? 0;
                    decimal seguroInternacional = valorprocesso.SeguroInternaciona ?? 0;

                    decimal valorTotalCif = valorExw + valorFobFca + freteInternacional + seguroInternacional;

                    ValorProcesso novoValor = new ValorProcesso
                    {
                        ProcessoId = processoId,
                        ValorExw = valorExw,
                        ValorFobFca = valorFobFca,
                        FreteInternacional = freteInternacional,
                        SeguroInternaciona = seguroInternacional,
                        ValorTotalCif = valorTotalCif,
                        Moeda = valorprocesso.Moeda,
                    };

                    _context.ValorProcessos.Add(novoValor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = novoValor.ProcessoId });
                }

                return View(valorprocesso);
            }
                 catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.ValorProcessos.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, ValorProcesso valorprocesso)
        {
            try
            {
                if (id != valorprocesso.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    if (Request.Form.ContainsKey("ProcessoId"))
                    {
                        valorprocesso.ProcessoId = Convert.ToInt32(Request.Form["ProcessoId"]);
                    }
                    _context.ValorProcessos.Update(valorprocesso);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = valorprocesso.ProcessoId });
                }

                return View(valorprocesso);
            }
            catch (Exception)
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

                var dados = await _context.ValorProcessos.FindAsync(id);

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
                    var dados = await _context.ValorProcessos.FindAsync(id);

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

                var dados = await _context.ValorProcessos.FindAsync(id);

                if (id == null)
                    return _error.NotFoundError();

                _context.ValorProcessos.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }

        }
    }
}
