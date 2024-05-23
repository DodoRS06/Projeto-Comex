using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
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

			// Lista temporária para armazenar os dados
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

			ViewBag.DCEsTemp = DCETemp;

            // Disparar evento personalizado para notificar a view que os dados estão disponíveis (teste)
            Response.Headers.Add("X-Dados-Disponiveis", "true");

            Dropdowns();

			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DCE dce)
        {
            if (ModelState.IsValid)
            {
                // Recupera o ProcessoId do formulário
                int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);

                // Atribui o ProcessoId ao objeto DCE
                dce.ProcessoId = processoId;

                _context.DCEs.Add(dce);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

			await Dropdowns(dce);

			return View(dce);
        }

        [HttpPost]
        public async Task<IActionResult> AddDespesaTemp([FromBody] DCETemp dceTemp)
        {
            if (ModelState.IsValid)
            {
                // Recuperar os nomes de CadastroDespesa e FornecedorServico
                dceTemp.CadastroDespesaNome = await GetDespesaNome(dceTemp.CadastroDespesaId);
                dceTemp.FornecedorServicoNome = await GetFornecedorNome(dceTemp.FornecedorServicoId);

                _context.DCEsTemp.Add(dceTemp);
                await _context.SaveChangesAsync();

                return Json(new { success = true, despesa = dceTemp });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirDespesaTemp(int id)
        {
            var dceTemp = await _context.DCEsTemp.FindAsync(id);
            if (dceTemp == null)
            {
                return Json(new { success = false, errors = new[] { "Item não encontrado" } });
            }

            _context.DCEsTemp.Remove(dceTemp);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarTodosOsItens()
        {
            var dcesTemp = await _context.DCEsTemp.ToListAsync();

            foreach (var temp in dcesTemp)
            {
                var dce = new DCE
                {
                    CadastroDespesaId = temp.CadastroDespesaId,
                    FornecedorServicoId = temp.FornecedorServicoId,
                    Valor = temp.Valor,
                    Observacao = temp.Observacao,
                    ProcessoId = temp.ProcessoId
                };

                _context.DCEs.Add(dce);
            }

            // Remover itens temporários após a transferência
            _context.DCEsTemp.RemoveRange(dcesTemp);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirTodosItens(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                // Encontrar todos os itens relacionados ao ProcessoId
                var itensParaExcluir = await _context.DCEs.Where(d => d.ProcessoId == id).ToListAsync();

                // Remover os itens do contexto
                _context.DCEs.RemoveRange(itensParaExcluir);

                // Salvar as mudanças no banco de dados
                await _context.SaveChangesAsync();

                // Retornar uma resposta de sucesso
                return Ok();
            }
            catch (Exception ex)
            {
                // Retornar um código de erro com uma mensagem de erro
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
                return RedirectToAction("Index", new { id = dce.ProcessoId });
            }

			await Dropdowns(dce);

            return View(dce);
        }

   //     public async Task<IActionResult> Details(int? id)
   //     {
   //         if (id == null)
   //             return NotFound();

   //         var dados = await _context.DCEs.FindAsync(id);

   //         if (id == null)
   //             return NotFound();

			//await Dropdowns(dados);

			//return View(dados);
   //     }

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
			return RedirectToAction("Index", new { id = dados.ProcessoId });
		}
    }
}
