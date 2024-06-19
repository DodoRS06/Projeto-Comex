using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class VeiculosController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public VeiculosController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error; 
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Veiculos
                        .OrderBy(a => a.Motorista)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Veiculo veiculo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);


                    Veiculo novoVeiculo = new Veiculo
                    {
                        ProcessoId = processoId,
                        Placa = veiculo.Placa,
                        Motorista = veiculo.Motorista,


                    };
                    _context.Veiculos.Add(novoVeiculo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = novoVeiculo.ProcessoId });
                }

                return View(veiculo);
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

                var dados = await _context.Veiculos.FindAsync(id);

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Veiculo veiculo)
        {
            try
            {

                if (id != veiculo.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    if (Request.Form.ContainsKey("ProcessoId"))
                    {
                        veiculo.ProcessoId = Convert.ToInt32(Request.Form["ProcessoId"]);
                    }
                    _context.Veiculos.Update(veiculo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = veiculo.ProcessoId });
                }
                return View();
            }
            catch
            {
                return NotFound();
            }

        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Veiculos.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Veiculos
                .Include(d => d.Processo)
                .FirstOrDefaultAsync(d => d.Id == id); ;

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Veiculos.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                if (User.IsInRole("Admin"))
                {
                    _context.Veiculos.Remove(dados);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
                }
                return _error.UnauthorizedError();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }

        }
    }
}
