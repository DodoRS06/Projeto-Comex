using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class DCEsController : Controller
    {

        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public DCEsController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }

        private async Task Dropdowns(DCE dce = null)
        {

            ViewData["CadastroDespesaId"] = new SelectList(_context.CadastroDespesas, "Id", "NomeDespesa", dce?.CadastroDespesaId);
            ViewData["FornecedorServicoId"] = new SelectList(_context.FornecedorServicos, "Id", "Nome", dce?.FornecedorServicoId);
        }

        //Recuperar nome das despesas a partir do ID (adicionei variável temporária na model)
        private async Task<string> GetDespesaNome(int id)
        {
            var despesa = await _context.CadastroDespesas.FindAsync(id);
            return despesa != null ? despesa.NomeDespesa : "Despesa não encontrada";
        }

        //Recuperar nome dos fornecedores a partir do ID (adicionei variável temporária na model)
        private async Task<string> GetFornecedorNome(int id)
        {
            var fornecedor = await _context.FornecedorServicos.FindAsync(id);
            return fornecedor != null ? fornecedor.Nome : "Fornecedor não encontrado";
        }

        public async Task<IActionResult> Index(int? id)
        {
            //Testar se o id passado é válido
            if (id == null)
                return _error.NotFoundError();

            List<DCE> dados;

            //Recuperando dados do banco
            try
            {
                //Recuperando DCEs com o mesmo id de processo
                dados = await _context.DCEs
                    .Where(d => d.ProcessoId == id)
                    .ToListAsync();
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar DCEs. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar DCEs do banco de dados. {ex.Message}";
                return _error.BadRequestError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar DCEs do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }

            if (dados == null || !dados.Any())
                return _error.NotFoundError();

            ViewData["ProcessoId"] = id;

            //Processando e preparando dados
            try
            {
                //Procurar um nome com aquele id dentro da tabela de despesa e fornecedor
                foreach (var dce in dados)
                {
                    var despesa = await _context.CadastroDespesas.FindAsync(dce.CadastroDespesaId);
                    var fornecedor = await _context.FornecedorServicos.FindAsync(dce.FornecedorServicoId);
                    if (despesa == null)
                    {
                        TempData["MensagemErro"] = $"Despesa não encontrada para o ID {dce.CadastroDespesaId}.";
                        return _error.InternalServerError();
                    }
                    else
                    {
                        dce.CadastroDespesaNome = despesa.NomeDespesa;
                    }
                    if (fornecedor == null)
                    {
                        TempData["MensagemErro"] = $"Fornecedor não encontrado para o ID {dce.FornecedorServicoId}.";
                        return _error.InternalServerError();
                    }
                    else
                    {
                        dce.FornecedorServicoNome = fornecedor.Nome;
                    }
                }

                await Dropdowns();

                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao procurar nomes de despesas ou fornecedores. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao procurar nomes de despesas ou fornecedores. {ex.Message}";
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Create(int? id)
        {
            //Testando se o id passado é válido
            if (id == null)
                return _error.NotFoundError();

            ViewData["ProcessoId"] = id.Value;

            Processo processo;
            List<DCE> dados;

            //Recuperando dados do banco
            try
            {
                //Recuperando um processo com o mesmo id da rota
                processo = await _context.Processos
                    .Where(d => d.Id == id)
                    .FirstOrDefaultAsync();

                if (processo == null)
                    return _error.NotFoundError();

                ViewData["CodProcessoExportacao"] = processo.CodProcessoExportacao;

                //Recuperando DCEs com o mesmo id de processo
                dados = await _context.DCEs
                    .Where(d => d.ProcessoId == id)
                    .ToListAsync();

                //Testando se recuperou alguma DCE
                if (dados == null || !dados.Any())
                    return _error.NotFoundError();
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar processos ou DCEs. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar processos ou DCEs do banco de dados. {ex.Message}";
                return _error.BadRequestError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar processos ou DCEs do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }

            //Lista temporária para armazenar os dados
            var DCETemp = new List<DCE>();

            //Processando e preparando dados
            try
            {
                //Procurar um nome com aquele id dentro da tabela de despesa e fornecedor
                foreach (var dce in dados)
                {

                    dce.CadastroDespesaNome = await GetDespesaNome(dce.CadastroDespesaId);
                    dce.FornecedorServicoNome = await GetFornecedorNome(dce.FornecedorServicoId);
                    DCETemp.Add(dce);
                }

                //Soma dos valores e conversão pra decimal
                decimal totalDCETemp = (decimal)DCETemp.Sum(d => d.Valor);

                ViewBag.DCEsTemp = DCETemp;
                ViewBag.ValorItens = totalDCETemp;

                await Dropdowns();

                return View();
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao procurar nomes de despesas ou fornecedores. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao procurar nomes de despesas ou fornecedores. {ex.Message}";
                return _error.InternalServerError();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DCE dce)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Recupera o ProcessoId do formulário
                    if (!int.TryParse(Request.Form["ProcessoId"], out int processoId))
                    {
                        ModelState.AddModelError("ProcessoId", "ID do processo inválido.");
                        await Dropdowns(dce);
                        return View(dce);
                    }

                    //Atribui o ProcessoId ao objeto DCE
                    dce.ProcessoId = processoId;
                    dce.Valor = (float)Math.Round(dce.Valor, 2);

                    _context.DCEs.Add(dce);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = processoId });
                }

                await Dropdowns(dce);

                return View(dce);
            }
            catch (FormatException ex)
            {
                TempData["MensagemErro"] = $"Erro de formatação ao processar o ID do processo. {ex.Message}";
                return _error.BadRequestError();
                //ModelState.AddModelError("ProcessoId", $"ID do processo inválido. {ex.Message}.");
                //await Dropdowns(dce);
                //return View(dce);
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao salvar o DCE no banco de dados. {ex.Message}";
                return _error.InternalServerError();
                //ModelState.AddModelError("", $"Erro ao salvar os dados no banco de dados. {ex.Message}. Tente novamente mais tarde.");
                //await Dropdowns(dce);
                //return View(dce);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados. {ex.Message}";
                return _error.InternalServerError();
                //ModelState.AddModelError("", $"Erro de conexão com o banco de dados. {ex.Message}. Tente novamente mais tarde.");
                //await Dropdowns(dce);
                //return View(dce);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao processar o formulário. {ex.Message}";
                return _error.InternalServerError();
                //ModelState.AddModelError("", $"Ocorreu um erro ao processar sua solicitação. {ex.Message}. Tente novamente mais tarde.");
                //await Dropdowns(dce);
                //return View(dce);
            }
        }

        [HttpPost]
        public JsonResult GetDespesaEFornecedorNomes([FromBody] DCE novoItem)
        {
            if (novoItem == null)
                return Json(new { success = false, message = "Dados inválidos." });

            try
            {
                var despesa = _context.CadastroDespesas.FirstOrDefault(d => d.Id == novoItem.CadastroDespesaId);
                var fornecedor = _context.FornecedorServicos.FirstOrDefault(f => f.Id == novoItem.FornecedorServicoId);

                if (despesa == null || fornecedor == null)
                {
                    return Json(new { success = false, message = "Despesa ou fornecedor não encontrado." });
                }

                return Json(new
                {
                    success = true,
                    despesaNome = despesa?.NomeDespesa,
                    fornecedorNome = fornecedor?.Nome
                });
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = "Erro de conexão com o banco de dados ao recuperar despesa ou fornecedor.";
                return Json(new { success = false, message = $"Erro de conexão com o banco de dados. {ex.Message} Tente novamente mais tarde." });
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao recuperar despesa ou fornecedor.";
                return Json(new { success = false, message = $"Ocorreu um erro ao processar sua solicitação. {ex.Message} Tente novamente mais tarde." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirDespesaTemp([FromBody] JsonElement data)
        {
            try
            {
                //Validação do dado recebido
                if (!data.TryGetProperty("id", out JsonElement idElement) || idElement.ValueKind != JsonValueKind.Number)
                {
                    TempData["MensagemErro"] = "Dados recebidos estão nulos ou inválidos";
                    return Json(new { success = false, errors = new[] { "Dados inválidos" } });
                }

                //Recupera o ID
                int id = idElement.GetInt32();

                // Recupera o item no banco de dados
                var dceTemp = await _context.DCEs.FindAsync(id);

                if (dceTemp == null)
                {
                    TempData["MensagemErro"] = $"Item com ID {id} não encontrado no banco de dados";
                    return Json(new { success = false, errors = new[] { "Item não encontrado" } });
                }

                //Remove o item
                _context.DCEs.Remove(dceTemp);
                await _context.SaveChangesAsync();
                TempData["MensagemErro"] = $"Item com ID {id} excluído com sucesso";

                return Json(new { success = true });
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = "Erro ao atualizar o banco de dados ao excluir o item";
                return Json(new { success = false, errors = new[] { "Erro ao excluir o item.", "Erro de atualização do banco de dados.", ex.Message } });
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = "Erro de conexão com o banco de dados ao excluir o item";
                return Json(new { success = false, errors = new[] { "Erro ao excluir o item.", "Erro de conexão com o banco de dados.", ex.Message } });
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro inesperado ao excluir o item";
                return Json(new { success = false, errors = new[] { "Erro ao excluir o item.", ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarListaItens([FromBody] List<DCE> itensLista)
        {
            //if(itensLista == null || !itensLista.Any())
            //{
            //    return BadRequest("A lista de itens está vazia ou nula.");
            //}
            try
            {
                if (itensLista != null && itensLista.Any())
                {
                    foreach (var item in itensLista)
                    {
                        _context.DCEs.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }
                return Ok(new { success = true, message = "Itens cadastrados com sucesso." });
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar o banco de dados ao cadastrar itens. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao cadastrar itens. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro inesperado ao cadastrar itens. {ex.Message}";
                return _error.InternalServerError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirTodosItens(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var itensParaExcluir = await _context.DCEs.Where(d => d.ProcessoId == id).ToListAsync();

                if (itensParaExcluir == null || !itensParaExcluir.Any())
                    return _error.NotFoundError();

                _context.DCEs.RemoveRange(itensParaExcluir);

                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Itens excluídos com sucesso." });
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar o banco de dados ao excluir itens. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao excluir itens. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro inesperado ao excluir itens. {ex.Message}";
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

                //Recuperando DCE pelo id
                var dados = await _context.DCEs.FindAsync(id);

                //Retornando erro se não existir DCE com o id passado
                if (dados == null)
                    return _error.NotFoundError();

                await Dropdowns(dados);

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
        public async Task<IActionResult> Edit(int id, DCE dce)
        {
            try
            {
                //Testando se os ids são diferentes e retornando erro
                if (id != dce.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    if (Request.Form.ContainsKey("ProcessoId"))
                    {
                        dce.ProcessoId = Convert.ToInt32(Request.Form["ProcessoId"]);
                    }

                    _context.DCEs.Update(dce);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = dce.ProcessoId });
                }

                await Dropdowns(dce);

                return View(dce);
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

                //Recuperando DCE com o id passado
                var dados = await _context.DCEs
                    .Where(d => d.ProcessoId == id)
                    .FirstOrDefaultAsync();

                //Retornando erro se o id da DCE passado for nulo
                if (dados == null)
                    return _error.NotFoundError();

                ViewData["ProcessoId"] = dados.ProcessoId;

                //Recuperando Despesa com o id passado
                var despesa = _context.DCEs.FirstOrDefault(e => e.ProcessoId == dados.ProcessoId);

                //Testando se a despesa não é nula e retornando o nome da despesa
                if (despesa != null)
                    ViewData["despesa"] = GetNomeDespesa(despesa.CadastroDespesaId);

                //Recuperando Fornecedor com o id passado
                var fornecedor = _context.DCEs.FirstOrDefault(e => e.ProcessoId == dados.ProcessoId);

                //Testando se a despesa não é nula e retornando o nome da despesa
                if (fornecedor != null)
                    ViewData["fornecedor"] = GetNomeFornecedor(fornecedor.FornecedorServicoId);

                var view = new DCEView
                {
                    DCEs = _context.DCEs.Where(d => d.ProcessoId == dados.ProcessoId).ToList()
                };

                return View(view);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao buscar detalhes da DCE com ID {id}. {ex.Message}";
                return _error.InternalServerError();
            }
        }

        private string GetNomeDespesa(int? id) => id != null ? _context.CadastroDespesas.FirstOrDefault(d => d.Id == id)?.NomeDespesa : string.Empty;
        private string GetNomeFornecedor(int? id) => id != null ? _context.FornecedorServicos.FirstOrDefault(d => d.Id == id)?.Nome : string.Empty;

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                //Retornando erro se o id passado for nulo
                if (id == null)
                    return _error.NotFoundError();

                //Recuperando DCE com o id passado
                var dados = await _context.DCEs.FindAsync(id);

                //Retornando erro se o id da DCE passado for nulo
                if (dados == null)
                    return _error.NotFoundError();

                await Dropdowns(dados);

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

                //Recuperando DCE com o id passado
                var dados = await _context.DCEs.FindAsync(id);

                //Retornando erro se o id da DCE passado for nulo
                if (dados == null)
                    return _error.NotFoundError();

                await Dropdowns(dados);

                _context.DCEs.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
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
