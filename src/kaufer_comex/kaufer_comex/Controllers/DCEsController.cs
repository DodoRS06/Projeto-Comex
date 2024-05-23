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
			return despesa != null ? despesa.NomeDespesa : string.Empty;
		}

        //Recuperar nome dos fornecedores a partir do ID (adicionei variável temporária na model)
        private async Task<string> GetFornecedorNome(int id)
		{
			var fornecedor = await _context.FornecedorServicos.FindAsync(id);
			return fornecedor != null ? fornecedor.Nome : string.Empty;
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

            var dados = await _context.DCEs
                .Where(d => d.ProcessoId == id)
                .ToListAsync();

            //Procurar um nome com aquele id dentro da tabela de despesa e fornecedor
            foreach (var dce in dados)
            {
                dce.CadastroDespesaNome = await GetDespesaNome(dce.CadastroDespesaId);
                dce.FornecedorServicoNome = await GetFornecedorNome(dce.FornecedorServicoId);
            }

            ViewBag.DCEs = dados;

            // Disparar evento personalizado para notificar a view que os dados estão disponíveis
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
                // Recupere o ProcessoId do formulário
                int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);

                // Atribua o ProcessoId ao objeto DCE
                dce.ProcessoId = processoId;

                _context.DCEs.Add(dce);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

			await Dropdowns(dce);

			return View(dce);
        }

        //Método praa criar um id no banco para cada item da lista que foi feita na view Create
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

        //Metodo para recuperar nomes de despesa e fornecedor para ser usado na view create (na lista de itens temporária)
        //Criei outro método pois o retorno desse é um json (pra ficar mais fácil implementar)
        [HttpPost]
        public async Task<IActionResult> GetDespesaAndFornecedorNames([FromBody] DCE novoItem)
        {
            try
            {
                string despesaNome = await GetDespesaNome(novoItem.CadastroDespesaId);
                string fornecedorNome = await GetFornecedorNome(novoItem.FornecedorServicoId);

                return Json(new { despesaNome, fornecedorNome });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter nomes: {ex.Message}");
            }
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
