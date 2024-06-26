﻿using kaufer_comex.Migrations;
using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace kaufer_comex.Controllers
{
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
        public IActionResult Create()
        {
            try
            {
                var user = _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefault();

                ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");
                ViewData["NotaItem"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");
                ViewData["EmbarqueRodoviarioId"] = new SelectList(_context.EmbarqueRodoviarios, "Id", "Transportadora");

                var view = new NovaNotaView
                {
                    Emissao = DateTime.Now,
                    BaseNota = DateTime.Now,
                    NotaItemTemps = _context.NotaItemTemps.ToList(),
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
                var user = _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefault();
                if (ModelState.IsValid)
                {
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
                        EmbarqueRodoviarioId = view.EmbarqueRodoviarioId,
                        QuantidadeTotal = view.QuantidadeTotal,
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
                    return RedirectToAction("Index");
                }

                ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");
                ViewData["NotaItem"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");
                ViewData["EmbarqueRodoviarioId"] = new SelectList(_context.EmbarqueRodoviarios, "Id", "Transportadora");
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
        public IActionResult AdicionaItem()
        {
            try
            {
                var user = _context.Usuarios.Where(u => u.NomeFuncionario == User.Identity.Name).FirstOrDefault();
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

                        };

                        _context.NotaItemTemps.Add(novoItem);
                    }

                    else
                    {
                        novoItem.Quantidade += view.Quantidade;
                        _context.Entry(novoItem).State = EntityState.Modified;
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Create");

                }

                ViewData["ItemId"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");
                return PartialView();
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        // Excluir Item
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

                return RedirectToAction("Create");
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
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

                if (ModelState.IsValid)
                {
                    _context.Notas.Update(nota);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");

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
        //public async Task<IActionResult> ExcluirItemNota(int? id)
        //{

        //    var dados = await _context.NotaItens
        //      .Include(p => p.Nota)
        //      .FirstOrDefaultAsync(p => p.ItemId == id);

        //    if (dados == null)
        //        return NotFound();

        //    var itemNota = _context.NotaItens.Where(i => i.NotaId == dados.NotaId).FirstOrDefault();
        //    _context.NotaItens.Remove(itemNota);
        //    await _context.SaveChangesAsync();


        //    return RedirectToAction("Edit", "Notas");
        //}

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

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
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
                    .Include(p => p.Veiculo)
                    .Include(p => p.NotaItem)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                    return NotFound();

                var item = _context.NotaItens.Where(i => i.NotaId == dados.Id).FirstOrDefault();
                _context.NotaItens.Remove(item);
                await _context.SaveChangesAsync();

                _context.Notas.Remove(dados);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }
    }
}
