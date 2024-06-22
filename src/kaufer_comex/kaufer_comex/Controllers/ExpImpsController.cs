using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class ExpImpsController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public ExpImpsController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //Recuperando Exportadores e importadores ordenados pelo nome
                var dados = await _context.ExpImps
                    .OrderBy(e => e.Nome)
                    .ToListAsync();

                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar exportador/importador. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar exportador/importador do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
        }
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpImp expimp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Recuperando exportadores/importadores existentes
                    var expImpExistente = await _context.ExpImps
                   .AnyAsync(d => d.Nome == expimp.Nome && d.TipoExpImp == expimp.TipoExpImp);

                    //Retornando mensagem caso o exportador/importador já exista
                    if (expImpExistente)
                    {
                        TempData["MensagemErro"] = $"Esse exportador/importador já está cadastrado.";
                        return View(expimp);
                    }

                    _context.ExpImps.Add(expimp);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(expimp);
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao salvar o exportador/importador no banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao processar o formulário. {ex.Message}";
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                //Retornando erro se o id for nulo
                if (id == null)
                    return _error.NotFoundError();

                //Recuperando exportador/importador pelo id
                var dados = await _context.ExpImps.FindAsync(id);

                //Retornando erro se não existir exportador/importador com o id passado
                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao editar item com ID {id}. {ex.Message}";
                return _error.InternalServerError();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpImp expimp)
        {
            try
            {
                //Testando se os ids são diferentes e retornando erro
                if (id != expimp.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    _context.ExpImps.Update(expimp);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar o banco de dados ao editar item com ID {id}. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao editar item com ID {id}. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro inesperado ao editar item com ID {id}. {ex.Message}";
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                //Retornando erro se o id passado for nulo
                if (id == null)
                    return _error.NotFoundError();

                //Recuperando exportador/importador pelo id
                var dados = await _context.ExpImps.FindAsync(id);

                //Retornando erro se não existir exportador/importador com o id passado
                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar detalhes do exportador/importador com ID {id}. {ex.Message}";
                return _error.InternalServerError();
            }
        }


        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                //Retornando erro se o id passado for nulo
                if (id == null)
                    return _error.NotFoundError();

                //Recuperando exportador/importador pelo id
                var dados = await _context.ExpImps.FindAsync(id);

                //Retornando erro se não existir exportador/importador com o id passado
                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar detalhes do item com ID {id} para exclusão. {ex.Message}";
                return _error.InternalServerError();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                //Retornando erro se o id passado for nulo
                if (id == null)
                    return _error.NotFoundError();

                //Recuperando exportador/importador pelo id
                var dados = await _context.ExpImps.FindAsync(id);

                //Retornando erro se não existir exportador/importador com o id passado
                if (dados == null)
                    return _error.NotFoundError();

                _context.ExpImps.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir item com ID {id}: erro de atualização do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir item com ID {id}. {ex.Message}";
                return _error.InternalServerError();
            }
        }
    }
}
