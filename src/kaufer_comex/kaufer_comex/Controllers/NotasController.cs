﻿using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using kaufer_comex.Migrations;
using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
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

        private readonly ErrorService _error;
        public NotasController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error; 
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
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Notas. {ex.Message}";
                return _error.InternalServerError();
            }          
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Notas do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
        }


        //GET: Notas/Create
        public async Task<IActionResult> Create(int? id)
        {
            try
            {
                if (id == null)
                {
                    return _error.NotFoundError();
                }

                ViewData["EmbarqueRodoviarioId"] = id.Value;


                var user = await _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefaultAsync();
                if (user == null) { return NotFound(); }

                var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == id);
                if (embarque == null) { return NotFound(); }

                var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);
                if (processoEmbarque == null) { return NotFound(); }

                var processoId = processoEmbarque.Id;

                ViewData["ProcessoId"] = processoEmbarque.Id;
                InfoViewData(processoEmbarque,processoId);
                ViewData["VeiculoId"] = new SelectList(_context.Veiculos.Where(v =>v.ProcessoId == processoEmbarque.Id), "Id", "Motorista");
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
            catch (Exception)
            {
                return _error.InternalServerError();
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

                var notaItemTemps = _context.NotaItemTemps.Where(u => u.NomeUsuario == User.Identity.Name).ToList();

               
                if (!notaItemTemps.Any())
                {
                    ModelState.AddModelError("", "Você deve adicionar pelo menos um item antes de criar a nota.");
                    int embarqueId_ = Convert.ToInt32(Request.Form["EmbarqueRodoviarioId"]);
                    ViewData["EmbarqueRodoviarioId"] = embarqueId_;
                    var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == embarqueId);
                    if (embarque == null) { return NotFound(); }

                    var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);
                    if (processoEmbarque == null) { return NotFound(); }

                    var processoId = processoEmbarque.Id;
                    InfoViewData(processoEmbarque, processoId);

                    view.NotaItemTemps = notaItemTemps;

                    return View(view);
                }
                else
                {
                 
                    if (ModelState.IsValid)
                    {
                        if (await NotaJaExiste(view.NumeroNf))
                        {
                            ModelState.AddModelError("NumeroNf", "Esse número de nota já está cadastrado.");
                            int embarqueId_ = Convert.ToInt32(Request.Form["EmbarqueRodoviarioId"]);
                            ViewData["EmbarqueRodoviarioId"] = embarqueId_;
                            var embarque_ = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == embarqueId);
                            if (embarque_ == null) { return NotFound(); }

                            var processoEmbarque_ = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque_.ProcessoId);
                            if (processoEmbarque_ == null) { return NotFound(); }

                            
                            InfoViewData(processoEmbarque_, processoEmbarque_.Id);
                            var usuario = _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefault();
                            view.NotaItemTemps = notaItemTemps;
                            return View(view);
                        }

                        var novaNota = CriarNovaNota(view, embarqueId);
                        _context.Notas.Add(novaNota);
                        await _context.SaveChangesAsync();

                        await CriarItensNota(novaNota.Id, User.Identity.Name);
                        var processoId = await GetProcessoId(novaNota.EmbarqueRodoviarioId);
                        return RedirectToAction("Details", "Processos", new { id = processoId });

                    }
                    var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == embarqueId);
                    if (embarque == null) { return NotFound(); }

                    var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);
                    if (processoEmbarque == null) { return NotFound(); }


                    InfoViewData(processoEmbarque, processoEmbarque.Id);
                    view.NotaItemTemps = notaItemTemps; 

                    return View(view);
                }
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar Despacho. {ex.Message}";
                return _error.ConflictError();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        //Verificar se já existe o Numero de NF cadastrado
        private async Task<bool> NotaJaExiste(int? numeroNf)
        {
            return await _context.Notas.AnyAsync(a => a.NumeroNf == numeroNf);
        }

        //Criar uma nova nota
        private Nota CriarNovaNota(NovaNotaView view, int embarqueId)
        {
            return new Nota
            {
                NumeroNf = view.NumeroNf,
                Emissao = view.Emissao,
                BaseNota = view.BaseNota,
                ValorFob = view.ValorFob,
                ValorCif = view.ValorFob + view.ValorSeguro + view.ValorFrete,
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
        }

        //Relacionar itens e nota na tabela NotaItens
        private async Task CriarItensNota(int notaId, string nomeUsuario)
        {
            var itens = await _context.NotaItemTemps.Where(u => u.NomeUsuario == nomeUsuario).ToListAsync();

            foreach (var item in itens)
            {
                var notaItem = new NotaItem
                {
                    ItemId = item.ItemId,
                    NotaId = notaId,
                    Quantidade = item.Quantidade,
                    Valor = item.Valor,
                };

                _context.NotaItens.Add(notaItem);
                _context.NotaItemTemps.Remove(item);
            }
            await _context.SaveChangesAsync();
        }

        
        private async Task<int> GetProcessoId(int embarqueRodoviarioId)
        {
            var embarqueProcesso = await _context.EmbarqueRodoviarios.FindAsync(embarqueRodoviarioId);
  
            var processo = await _context.Processos.FirstOrDefaultAsync(p => p.Id == embarqueProcesso.ProcessoId);
            return processo.Id;
        }

        private void InfoViewData(Processo processo,int processoId)
        {
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos.Where(v => v.ProcessoId == processo.Id), "Id", "Motorista");
            ViewData["NotaItem"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");
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
                    decimal itemValor = (decimal)(novoItem.Preco * Convert.ToDecimal(view.Quantidade));

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
                InfoViewData(processoEmbarque, processoEmbarque.Id);

                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        //POST: Notas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Nota nota)
        {
            if (id != nota.Id)
                return NotFound();

            var dados = await GetDadosNota(id);
            if (dados == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    EditarDadosNota(dados, nota);
                    await _context.SaveChangesAsync();
                    var processoId = await GetProcessoId(dados.EmbarqueRodoviarioId);
                 
                    return RedirectToAction("Details", "Processos", new { id = processoId });
                }
                catch (DbUpdateException ex)
                {
                    TempData["MensagemErro"] = $"Erro ao editar a nota: {ex.Message}";
                    return View();
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Ocorreu um erro inesperado: {ex.Message}";
                    return View();
                }
            }
            var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == dados.EmbarqueRodoviarioId);

            var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);

            InfoViewData(processoEmbarque, processoEmbarque.Id);

            return View();
        }

        //GET: Dados de nota
        private async Task<Nota> GetDadosNota(int id)
        {
            return await _context.Notas
                .Include(p => p.Veiculo)
                .Include(p => p.EmbarqueRodoviario)
                .Include(p => p.NotaItem)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        //Editar dados de nota
        private void EditarDadosNota(Nota dados, Nota novaNota)
        {

            dados.NumeroNf = novaNota.NumeroNf;
            dados.Emissao = novaNota.Emissao;
            dados.BaseNota = novaNota.BaseNota;
            dados.ValorFob = novaNota.ValorFob;
            dados.ValorCif = novaNota.ValorFob + novaNota.ValorFrete + novaNota.ValorSeguro;
            dados.ValorFrete = novaNota.ValorFrete;
            dados.ValorSeguro = novaNota.ValorSeguro;
            dados.VeiculoId = novaNota.VeiculoId;
            dados.PesoBruto = novaNota.PesoBruto;
            dados.PesoLiq = novaNota.PesoLiq;
            dados.TaxaCambial = novaNota.TaxaCambial;
            dados.CertificadoQualidade = novaNota.CertificadoQualidade;
            dados.EmbarqueRodoviarioId = novaNota.EmbarqueRodoviarioId;
            dados.QuantidadeTotal = novaNota.QuantidadeTotal;
            dados.ValorTotalNota = novaNota.ValorTotalNota;
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
                    return _error.NotFoundError();

                var dados = await _context.Notas
                    .Include(p => p.Veiculo)
                    .Include(p => p.EmbarqueRodoviario)
                    .Include(p => p.NotaItem)
                    .ThenInclude(ni => ni.Item)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                    return _error.NotFoundError();

                var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == dados.EmbarqueRodoviarioId);

                var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);

                ViewData["ProcessoId"] = processoEmbarque.Id;

                return View(dados);
            }
            catch { return _error.InternalServerError(); }
        }


        //GET: Notas/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Notas
                    .Include(p => p.Veiculo)
                    .Include(p => p.NotaItem)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                    return _error.NotFoundError();

                if (User.IsInRole("Admin"))
                {
                    var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == dados.EmbarqueRodoviarioId);

                    var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);

                    ViewData["ProcessoId"] = processoEmbarque.Id;

                    return View(dados);
                }

                return _error.UnauthorizedError();
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
                    return _error.NotFoundError();

                var dados = await _context.Notas
                    .Include(n => n.Veiculo)
                    .Include(n => n.NotaItem)
                    .Include(n => n.EmbarqueRodoviario)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (User.IsInRole("Admin"))
                {
                    var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.Id == dados.EmbarqueRodoviarioId);

                    var processoEmbarque = await _context.Processos.FirstOrDefaultAsync(e => e.Id == embarque.ProcessoId);

                    if (dados == null)
                        return _error.NotFoundError();

                    var item = _context.NotaItens.Where(i => i.NotaId == dados.Id).FirstOrDefault();
                    _context.NotaItens.Remove(item);
                    await _context.SaveChangesAsync();

                    _context.Notas.Remove(dados);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "Processos", new { id = processoEmbarque.Id });
                }
                return _error.UnauthorizedError();
            }
            catch (DbUpdateException)
            {
                TempData["MensagemErro"] = $"Essa Nota está vinculada a um processo e não pode ser excluída ";
                return View();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
    }
}
