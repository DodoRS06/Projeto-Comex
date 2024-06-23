using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class FornecedorServicosController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public FornecedorServicosController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }

        public async Task<IActionResult> Index()
        {
            List<FornecedorServico> dados;

            try
            {
                //Recuperando Fornecedores ordenados pelo nome
                dados = await _context.FornecedorServicos
                    .OrderBy(a => a.Nome)
                    .ToListAsync();

                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar fornecedores. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Fornecedores do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorServico fornecedorservico)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Recuperando fornecedores existentes
                    var fornecedorExistente = await _context.FornecedorServicos
                    .AnyAsync(f => f.Nome == fornecedorservico.Nome && f.TipoServico == fornecedorservico.TipoServico);

                    //Retornando mensagem caso o fornecedor já exista
                    if (fornecedorExistente)
                    {
                        TempData["MensagemErro"] = $"Esse fornecedor já está cadastrado.";
                        return View(fornecedorservico);
                    }

                    _context.FornecedorServicos.Add(fornecedorservico);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(fornecedorservico);
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao salvar o Fornecedor no banco de dados. {ex.Message}";
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

                //Recupernado Fornecedor pelo id
                var dados = await _context.FornecedorServicos.FindAsync(id);

                //Retornando erro se não existir fornecedor com o id passado
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
        public async Task<IActionResult> Edit(int id, FornecedorServico fornecedorservico)
        {
            try
            {
                //Testando se os ids são diferentes e retornando erro
                if (id != fornecedorservico.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    _context.FornecedorServicos.Update(fornecedorservico);
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

                //Recuperando Fornecedor com o id passado
                var dados = await _context.FornecedorServicos.FindAsync(id);

                //Retornando erro se o id do fornecedor passado for nulo
                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar detalhes da DCE com ID {id}. {ex.Message}";
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

                //Recuperando Fornecedor com o id passado
                var dados = await _context.FornecedorServicos.FindAsync(id);

                //Retornando erro se o id do Fornecedor passado for nulo
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

                //Recuperando Fornecedor com o id passado
                var dados = await _context.FornecedorServicos.FindAsync(id);

                //Retornando erro se o id do Fornecedor passado for nulo
                if (dados == null)
                    return _error.NotFoundError();

                //Verificando se fornecedor está sendo usado em DCE
                bool fornecedorUsado = await _context.DCEs.AnyAsync(d => d.FornecedorServicoId == id);
                if (fornecedorUsado)
                {
                    TempData["MensagemErro"] = "Não é possível excluir este fornecedor, pois ele está sendo usado em um DCE.";
                    return RedirectToAction("Index");
                }

                _context.FornecedorServicos.Remove(dados);
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
