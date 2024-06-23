using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class CadastroDespesasController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public CadastroDespesasController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }

        public async Task<IActionResult> Index()
        {
            List<CadastroDespesa> dados;

            try
            {
                //Recuperando Despesas ordenados pelo nome
                dados = await _context.CadastroDespesas
                    .OrderBy(a => a.NomeDespesa)
                    .ToListAsync();

                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar despesas. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Despesas do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CadastroDespesa cadastrodespesa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Recuperando despesas existentes
                    var despesaExistente = await _context.CadastroDespesas
                   .AnyAsync(d => d.NomeDespesa == cadastrodespesa.NomeDespesa);

                    //Retornando mensagem caso a despesa já exista
                    if (despesaExistente)
                    {
                        TempData["MensagemErro"] = $"Essa despesa já está cadastrada.";
                        return View(cadastrodespesa);
                    }
                    _context.CadastroDespesas.Add(cadastrodespesa);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(cadastrodespesa);
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao salvar a despesa no banco de dados. {ex.Message}";
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

                //Recuperando despesa pelo id
                var dados = await _context.CadastroDespesas.FindAsync(id);

                //Retornando erro se não existir despesa com o id passado
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
        public async Task<IActionResult> Edit(int id, CadastroDespesa cadastrodespesa)
        {
            try
            {
                //Testando se os ids são diferentes e retornando erro
                if (id != cadastrodespesa.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    _context.CadastroDespesas.Update(cadastrodespesa);
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

                //Recuperando Despesa com o id passado
                var dados = await _context.CadastroDespesas.FindAsync(id);

                //Retornando erro se o id do fornecedor passado for nulo
                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar detalhes da despesa com ID {id}. {ex.Message}";
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

                //Recuperando Despesa com o id passado
                var dados = await _context.CadastroDespesas.FindAsync(id);

                //Retornando erro se o id do fornecedor passado for nulo
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

                //Recuperando Despesa com o id passado
                var dados = await _context.CadastroDespesas.FindAsync(id);

                //Retornando erro se o id do fornecedor passado for nulo
                if (dados == null)
                    return _error.NotFoundError();

                //Verificando se fornecedor está sendo usado em DCE
                bool despesaUsada = await _context.DCEs.AnyAsync(d => d.CadastroDespesaId == id);
                if (despesaUsada)
                {
                    TempData["MensagemErro"] = "Não é possível excluir esta despesa, pois ela está sendo usada em um DCE.";
                    return View();
                }

                _context.CadastroDespesas.Remove(dados);
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
