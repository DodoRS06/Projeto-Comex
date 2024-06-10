using DocumentFormat.OpenXml.Wordprocessing;
using kaufer_comex.Migrations;
using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Text.Json;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class NotasController : Controller
    {
        private AppDbContext _context;

        public NotasController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Notas/Index
        public async Task<IActionResult> Index()
        {
            try
            {

                var dados = await _context.Notas
                    .Include(p => p.Veiculo)
                    .Include(p => p.EmbarqueRodoviario)
                    .ToListAsync();


                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }


        //GET: Notas/Create
        public async Task<IActionResult> Create(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ViewData["EmbarqueRodoviarioId"] = id.Value;


                var user = await _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefaultAsync();
                if (user == null) { return NotFound(); }

                var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == id);
                if (embarque == null) { return NotFound(); }

                var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);
                if (processoEmbarque == null) { return NotFound(); }

                ViewData["ProcessoId"] = processoEmbarque.Id;
                ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");
                ViewData["NotaItem"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");

                var itens = _context.NotaItemTemps.Where(u => u.NomeUsuario == User.Identity.Name).ToList();

                var view = new NovaNotaView
                {
                    Emissao = DateTime.Now,
                    BaseNota = DateTime.Now,
                    NotaItemTemps = itens,
                    NotaItens = _context.NotaItens.ToList(),

                };
                return View(view);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        //POST: Notas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NovaNotaView view)
        {
            try
            {
                int embarqueId = Convert.ToInt32(Request.Form["EmbarqueRodoviarioId"]);
                var user = _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefault();

                if (ModelState.IsValid)
                {
                    var notaExistente = await _context.Notas
                        .AnyAsync(a => a.NumeroNf == view.NumeroNf);

                    if (notaExistente)
                    {
                        ModelState.AddModelError("NumeroNf", "Esse número de nota já está cadastrado.");
                        return View(view);
                    }

                    var novaNota = new Nota
                    {
                        NumeroNf = view.NumeroNf,
                        Emissao = view.Emissao,
                        BaseNota = view.BaseNota,
                        ValorFob = view.ValorFob,
                        ValorCif = view.ValorCif,
                        ValorFrete = view.ValorFrete,
                        ValorSeguro = view.ValorSeguro,
                        VeiculoId = view.VeiculoId,
                        PesoBruto = view.PesoBruto,
                        PesoLiq = view.PesoLiq,
                        TaxaCambial = view.TaxaCambial,
                        CertificadoQualidade = view.CertificadoQualidade,
                        EmbarqueRodoviarioId = embarqueId,
                        QuantidadeTotal = view.QuantidadeTotalNota,
                        ValorTotalNota = view.ValorTotalNota,
                    };
                    _context.Notas.Add(novaNota);
                    await _context.SaveChangesAsync();

                    var itens = _context.NotaItemTemps.Where(u => u.NomeUsuario == User.Identity.Name).ToList();

                    foreach (var item in itens)
                    {
                        var notaItem = new NotaItem
                        {
                            ItemId = item.ItemId,
                            NotaId = novaNota.Id,
                            Quantidade = item.Quantidade,
                            Valor = item.Valor,
                        };

                        _context.NotaItens.Add(notaItem);
                        _context.NotaItemTemps.Remove(item);
                        await _context.SaveChangesAsync();

                    }

                    var embarque = novaNota.EmbarqueRodoviarioId;
                    var embarqueProcesso = await _context.EmbarqueRodoviarios.FindAsync(embarque);
                    var processo = await _context.Processos.FirstOrDefaultAsync(p => p.Id == embarqueProcesso.ProcessoId);



                    return RedirectToAction("Details", "Processos", new { id = processo.Id });
                }

                ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");
                ViewData["NotaItem"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");

                var notaItemTemps = _context.NotaItemTemps.Where(u => u.NomeUsuario == User.Identity.Name).ToList();

                return View(view);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        // GET: ADD ITEM
        public async Task<IActionResult> AdicionaItem(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ViewData["EmbarqueRodoviarioId"] = id.Value;

                var user = await _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefaultAsync();
                ViewData["ItemId"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");
                return PartialView();
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        // POST: ADD ITEM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionaItem(AdicionaItemView view)
        {
            try
            {
                int embarqueId = Convert.ToInt32(Request.Form["EmbarqueRodoviarioId"]);
                ViewData["EmbarqueRodoviarioId"] = embarqueId;

                var user = _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefault();

                if (ModelState.IsValid)
                {
                    var novoItem = _context.NotaItemTemps.Where(u => u.NomeUsuario == User.Identity.Name && u.ItemId == view.ItemId).FirstOrDefault();
                    if (novoItem == null)
                    {

                        var item = _context.Itens.Find(view.ItemId);

                        novoItem = new Models.NotaItemTemp
                        {
                            ItemId = item.Id,
                            Quantidade = view.Quantidade,
                            Descricao = item.DescricaoProduto,
                            Preco = item.Preco,
                            NomeUsuario = User.Identity.Name,
                            PesoLiquido = item.PesoLiquido,
                            PesoBruto = item.PesoBruto,
                            Item = item,
                            EmbarqueId = embarqueId,

                        };

                        _context.NotaItemTemps.Add(novoItem);
                    }

                    else
                    {
                        novoItem.Quantidade += view.Quantidade;
                        _context.Entry(novoItem).State = EntityState.Modified;
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Create", new { id = embarqueId });

                }

                ViewData["ItemId"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");
                return PartialView();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar itens: {ex.Message}");
            }
        }

        // Excluir Item antes de cadastrar nota
        public async Task<IActionResult> ExcluirItem(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }

                var item = _context.NotaItemTemps.Where(u => u.NomeUsuario == User.Identity.Name && u.ItemId == id).FirstOrDefault();

                if (item == null)
                {
                    return NotFound();
                }
                _context.NotaItemTemps.Remove(item);
                await _context.SaveChangesAsync();


                return RedirectToAction("Create", new { id = item.EmbarqueId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir itens: {ex.Message}");
            }
        }

        // GET: Notas/EditItem/5
        public async Task<IActionResult> EditItem(int id)
        {
            var item = await _context.NotaItens
                .Include(ni => ni.Item)
                .FirstOrDefaultAsync(ni => ni.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            ViewData["ItemId"] = new SelectList(_context.Itens, "Id", "DescricaoProduto", item.ItemId);
            return PartialView(item);
        }

        // POST: ADD ITEM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(NotaItem view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Buscando item na tabela NotaItens
                    var existingItem = await _context.NotaItens
                        .FirstOrDefaultAsync(ni => ni.ItemId == view.ItemId && ni.NotaId == view.NotaId);

                    if (existingItem == null)
                    {
                        return NotFound();
                    }

                    var item = await _context.Itens.FirstOrDefaultAsync(i => i.Id == view.ItemId);

                    // Atualiza os campos da view Edit (Total e etc)
                    existingItem.Quantidade = view.Quantidade;
                    decimal quantidadeDecimal = (decimal)view.Quantidade;
                    existingItem.Valor = quantidadeDecimal * item.Preco;

                    _context.Entry(existingItem).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Edit", new { id = view.NotaId });
                }

                ViewData["ItemId"] = new SelectList(_context.Itens, "Id", "DescricaoProduto", view.ItemId);
                return PartialView(view);
            }
            catch
            {
                TempData["MensagemErro"] = "Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        // GET: Adicionar item na nota já criada
        public async Task<IActionResult> AdicionaItemNota(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ViewData["NotaId"] = id.Value;

                var user = await _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefaultAsync();
                ViewData["ItemId"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");
                return PartialView();
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        //POST: Adicionar item na nota já criada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionaItemNota(NotaItem view)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = view.ItemId;

            var itemExistente = await _context.NotaItens
                        .FirstOrDefaultAsync(ni => ni.ItemId == view.ItemId && ni.NotaId == view.NotaId);

            if (itemExistente == null)
            {
                var novoItem = await _context.Itens.FirstOrDefaultAsync(p => p.Id == item);
                if (novoItem != null && view != null)
                {
                    decimal itemValor = novoItem.Preco * Convert.ToDecimal(view.Quantidade);

                    var novaNotaItem = new NotaItem
                    {
                        NotaId = view.NotaId,
                        ItemId = view.ItemId,
                        Quantidade = view.Quantidade,
                        Valor = itemValor,
                    };
                    _context.NotaItens.Add(novaNotaItem);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                TempData["MensagemErro"] = $"Item já cadastrado.";
                return View();
            }

            return RedirectToAction("Edit", "Notas", new { id = view.NotaId });
        }

        //GET: Notas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Notas
                    .Include(p => p.Veiculo)
                    .Include(p => p.EmbarqueRodoviario)
                    .Include(p => p.NotaItem)
                    .ThenInclude(ni => ni.Item)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                    return NotFound();
                var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == dados.EmbarqueRodoviarioId);

                var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);

                ViewData["ProcessoId"] = processoEmbarque.Id;
                ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        //POST: Notas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Nota nota)
        {
            try
            {
                if (id != nota.Id)
                    return NotFound();

                var dados = await _context.Notas
                   .Include(p => p.Veiculo)
                   .Include(p => p.EmbarqueRodoviario)
                   .Include(p => p.NotaItem)
                   .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    dados.NumeroNf = nota.NumeroNf;
                    dados.Emissao = nota.Emissao;
                    dados.BaseNota = nota.BaseNota;
                    dados.ValorFob = nota.ValorFob;
                    dados.ValorCif = nota.ValorCif;
                    dados.ValorFrete = nota.ValorFrete;
                    dados.ValorSeguro = nota.ValorSeguro;
                    dados.VeiculoId = nota.VeiculoId;
                    dados.PesoBruto = nota.PesoBruto;
                    dados.PesoLiq = nota.PesoLiq;
                    dados.TaxaCambial = nota.TaxaCambial;
                    dados.CertificadoQualidade = nota.CertificadoQualidade;
                    dados.EmbarqueRodoviarioId = nota.EmbarqueRodoviarioId;
                    dados.QuantidadeTotal = nota.QuantidadeTotal;
                    dados.ValorTotalNota = nota.ValorTotalNota;
                    await _context.SaveChangesAsync();
                    
                    var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == dados.EmbarqueRodoviarioId);
                    
                    var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);

                    return RedirectToAction("Details", "Processos", new { id = processoEmbarque.Id });

                }

                ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");

                return View();
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        //Excluir item da nota já criada
        [HttpPost]
        public async Task<IActionResult> ExcluirItemNota([FromBody] ExcluirItemNotaModel model)
        {
            try
            {
                var notaItem = await _context.NotaItens
                    .FirstOrDefaultAsync(e => e.ItemId == model.IdItem && e.NotaId == model.IdNota);

                if (notaItem == null)
                {
                    return Json(new { success = false, errors = new[] { "Item não encontrado" } });
                }

                _context.NotaItens.Remove(notaItem);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro ao excluir o item. {ex.Message}";
                return Json(new { success = false, errors = new[] { ex.Message } });
            }
        }

        //GET: Notas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Notas
                    .Include(p => p.Veiculo)
                    .Include(p => p.EmbarqueRodoviario)
                    .Include(p => p.NotaItem)
                    .ThenInclude(ni => ni.Item)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                    return NotFound();

                var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == dados.EmbarqueRodoviarioId);

                var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);

                ViewData["ProcessoId"] = processoEmbarque.Id;

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }


        //GET: Notas/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Notas
                    .Include(p => p.Veiculo)
                    .Include(p => p.NotaItem)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                    return NotFound();

                var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == dados.EmbarqueRodoviarioId);

                var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);

                ViewData["ProcessoId"] = processoEmbarque.Id;

                return View(dados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir nota: {ex.Message}");
            }
        }

        //POST : Excluir Nota
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Notas
                    .Include(n => n.Veiculo)
                    .Include(n => n.NotaItem)
                    .Include(n => n.EmbarqueRodoviario)
                    .FirstOrDefaultAsync(p => p.Id == id);

                var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == dados.EmbarqueRodoviarioId);

                var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);

                if (dados == null)
                    return NotFound();

                var item = _context.NotaItens.Where(i => i.NotaId == dados.Id).FirstOrDefault();
                _context.NotaItens.Remove(item);
                await _context.SaveChangesAsync();

                _context.Notas.Remove(dados);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Processos", new { id = processoEmbarque.Id });
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }
    }
}
