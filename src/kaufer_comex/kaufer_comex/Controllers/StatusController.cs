using DocumentFormat.OpenXml.Office2010.Excel;
using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class StatusController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;
        public StatusController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error; 
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Status
                    .OrderBy(a => a.StatusAtual)
                        .ToListAsync();

                return View(dados);
            }
            catch(Exception)
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

                return View();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Status Status)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var StatusExistente = await _context.Status
                  .AnyAsync(a => a.StatusAtual == Status.StatusAtual);

                    if (StatusExistente)
                    {
                        ModelState.AddModelError("StatusAtual", "Esse Status já está cadastrado.");
                        return View(Status);
                    }
                    _context.Status.Add(Status);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");

                }
                return View(Status);
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

                var dados = await _context.Status.FindAsync(id);

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
        public async Task<IActionResult> Edit(int id, Status Status)
        {
            try
            {
                if (id != Status.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    _context.Update(Status);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");

                }

                return View();
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Status.FindAsync(id);

                if (dados == null)
                    return NotFound();

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
                {
                    return _error.NotFoundError();
                }
                var dados = await _context.Status.FindAsync(id);

                if (dados == null)
                {
                    return _error.NotFoundError();
                }
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

                var dados = await _context.Status.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                _context.Status.Remove(dados);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
    }
}
