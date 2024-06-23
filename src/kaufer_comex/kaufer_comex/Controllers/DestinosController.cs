using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class DestinosController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public DestinosController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }
        //GET: Destinos/Index
        public async Task<IActionResult> Index()
        {
            try
            {

                var dados = await _context.Destinos
                    .OrderBy(a => a.NomePais)
                    .ToListAsync();

                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Destinos. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Destinos do banco de dados. {ex.Message}";
                return _error.BadRequestError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Destinos do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }

        }

        //GET: Destinos/Create
        public IActionResult Create()
        {

            return View();
        }


        //POST: Destinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Destino destino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var destinoExistente = await _context.Destinos
                   .AnyAsync(a => a.NomePais == destino.NomePais);

                    if (destinoExistente)
                    {
                        ModelState.AddModelError("NomePais", "Esse destino já está cadastrado.");
                        return View(destino);
                    }
                    _context.Destinos.Add(destino);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(destino);
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar Destino. {ex.Message}";
                return _error.ConflictError();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        //GET: Destinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Destinos.FindAsync(id);
                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch(Exception)
            {
                return _error.InternalServerError();
            }
        }


        //POST: Destinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Destino destino)
        {
            try
            {
                if (id != destino.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    _context.Destinos.Update(destino);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao editar Destino. {ex.Message}";
                return _error.ConflictError();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        //GET: Destinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Destinos.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Destino. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Destino do banco de dados. {ex.Message}";
                return _error.BadRequestError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado: {ex.Message}. Por favor, tente novamente.";
                return _error.InternalServerError();
            }
        }

        //GET: Destinos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();
                if (User.IsInRole("Admin"))
                {
                    var dados = await _context.Destinos.FindAsync(id);

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

        //POST: Destinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();
                if (User.IsInRole("Admin"))
                {
                    var dados = await _context.Destinos.FindAsync(id);

                    if (dados == null)
                        return _error.NotFoundError();

                    _context.Destinos.Remove(dados);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return _error.UnauthorizedError();
            }
			catch (DbUpdateException ex)
			{
				TempData["MensagemErro"] = $"Esse destino está vinculado a um processo e não pode ser excluído. {ex.Message}";
				return _error.ConflictError();
			}
			catch (Exception)
            {
                return _error.InternalServerError();
            }
		}
    }
}

