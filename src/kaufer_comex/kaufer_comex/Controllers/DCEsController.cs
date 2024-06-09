using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class DCEsController : Controller
    {

        private readonly AppDbContext _context;

        public DCEsController(AppDbContext context)
        {
            _context = context;
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
            {
                return NotFound();
            }

            var dados = await _context.DCEs
                .Where(d => d.ProcessoId == id)
                .ToListAsync();

            if (dados == null)
            {
                return NotFound();
            }

            ViewData["ProcessoId"] = id;

            //Procurar um nome com aquele id dentro da tabela de despesa e fornecedor
            foreach (var dce in dados)
            {
                dce.CadastroDespesaNome = await GetDespesaNome(dce.CadastroDespesaId);
                dce.FornecedorServicoNome = await GetFornecedorNome(dce.FornecedorServicoId);
            }

            await Dropdowns();

            return View(dados);
        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["ProcessoId"] = id.Value;

            var processo = await _context.Processos
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            ViewData["CodProcessoExportacao"] = processo.CodProcessoExportacao;

            //Lista temporária para armazenar os dados
            var DCETemp = new List<DCE>();

            var dados = await _context.DCEs
                .Where(d => d.ProcessoId == id)
                .ToListAsync();

            //Procurar um nome com aquele id dentro da tabela de despesa e fornecedor
            foreach (var dce in dados)
            {
                dce.CadastroDespesaNome = await GetDespesaNome(dce.CadastroDespesaId);
                dce.FornecedorServicoNome = await GetFornecedorNome(dce.FornecedorServicoId);
                DCETemp.Add(dce);
            }

            decimal totalDCETemp = (decimal)DCETemp.Sum(d => d.Valor);

            ViewBag.DCEsTemp = DCETemp;
            ViewBag.ValorItens = totalDCETemp;

            Dropdowns();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DCE dce)
        {
            if (ModelState.IsValid)
            {
                //Recupera o ProcessoId do formulário
                int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);

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

        [HttpPost]
        public JsonResult GetDespesaEFornecedorNomes([FromBody] DCE novoItem)
        {
            var despesa = _context.CadastroDespesas.FirstOrDefault(d => d.Id == novoItem.CadastroDespesaId);
            var fornecedor = _context.FornecedorServicos.FirstOrDefault(f => f.Id == novoItem.FornecedorServicoId);

            return Json(new
            {
                despesaNome = despesa?.NomeDespesa,
                fornecedorNome = fornecedor?.Nome
            });
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirDespesaTemp([FromBody] JsonElement data)
        {
            try
            {
                //Validação do dado recebido
                if (!data.TryGetProperty("id", out JsonElement idElement) || idElement.ValueKind != JsonValueKind.Number)
                {
                    //Console.WriteLine("Dados recebidos estão nulos ou inválidos");
                    return Json(new { success = false, errors = new[] { "Dados inválidos" } });
                }

                //Recupera o ID
                int id = idElement.GetInt32();
                //Console.WriteLine($"ID recebido: {id}");

                // Recupera o item no banco de dados
                var dceTemp = await _context.DCEs.FindAsync(id);
                if (dceTemp == null)
                {
                    //Console.WriteLine("Item não encontrado no banco de dados");
                    return Json(new { success = false, errors = new[] { "Item não encontrado" } });
                }

                //Remove o item
                _context.DCEs.Remove(dceTemp);
                await _context.SaveChangesAsync();
                //Console.WriteLine("Item excluído com sucesso");

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Erro ao excluir o item: {ex.Message}");
                return Json(new { success = false, errors = new[] { "Erro ao excluir o item.", ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarListaItens([FromBody] List<DCE> itensLista)
        {
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
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar itens: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirTodosItens(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var itensParaExcluir = await _context.DCEs.Where(d => d.ProcessoId == id).ToListAsync();

                _context.DCEs.RemoveRange(itensParaExcluir);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir itens: {ex.Message}");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.DCEs.FindAsync(id);
            if (dados == null)
                return NotFound();

            await Dropdowns(dados);

            return View(dados);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DCE dce)
        {
            if (id != dce.Id)
                return NotFound();

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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dados = await _context.DCEs
                .Where(d => d.ProcessoId == id)
                .FirstOrDefaultAsync();

            ViewData["ProcessoId"] = dados.ProcessoId;

            if (dados == null)
            {
                return NotFound();
            }

            var despesa = _context.DCEs.FirstOrDefault(e => e.ProcessoId == dados.ProcessoId);
            if (despesa != null)
            {
                ViewData["despesa"] = GetNomeDespesa(despesa.CadastroDespesaId);
            }

            var fornecedor = _context.DCEs.FirstOrDefault(e => e.ProcessoId == dados.ProcessoId);
            if (fornecedor != null)
            {
                ViewData["fornecedor"] = GetNomeFornecedor(fornecedor.FornecedorServicoId);
            }

            var view = new DCEView
            {
                DCEs = _context.DCEs.Where(d => d.ProcessoId == dados.ProcessoId).ToList()
            };

            return View(view);
        }

        private string GetNomeDespesa(int? id) => id != null ? _context.CadastroDespesas.FirstOrDefault(d => d.Id == id)?.NomeDespesa : string.Empty;
        private string GetNomeFornecedor(int? id) => id != null ? _context.FornecedorServicos.FirstOrDefault(d => d.Id == id)?.Nome : string.Empty;

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.DCEs.FindAsync(id);

            if (id == null)
                return NotFound();

            await Dropdowns(dados);

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.DCEs.FindAsync(id);

            if (id == null)
                return NotFound();

            await Dropdowns(dados);

            _context.DCEs.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
        }
    }
}
