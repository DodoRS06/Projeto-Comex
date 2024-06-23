using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class EmbarqueRodoviariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorService _error;

        public EmbarqueRodoviariosController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.EmbarqueRodoviarios
                    .Include(e => e.AgenteDeCarga)
                    .ToListAsync();


                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Embarques. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Embarques do banco de dados. {ex.Message}";
                return _error.BadRequestError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Embarques do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
        }
        public IActionResult Create(int? id)
        {
            try
            {
                if (id == null)
                {
                    return _error.NotFoundError();
                }

                ViewData["ProcessoId"] = id.Value;

                ViewData["AgenteDeCargaId"] = new SelectList(_context.AgenteDeCargas, "Id", "NomeAgenteCarga");

                return View();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmbarqueRodoviario embarqueRodoviario)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);


                    EmbarqueRodoviario novoEmbarque = new EmbarqueRodoviario
                    {
                        ProcessoId = processoId,
                        Transportadora = embarqueRodoviario.Transportadora,
                        TransitTime = embarqueRodoviario.TransitTime,
                        DataEmbarque = embarqueRodoviario.DataEmbarque,
                        ChegadaDestino = embarqueRodoviario.ChegadaDestino,
                        AgenteDeCargaId = embarqueRodoviario.AgenteDeCargaId,
                        DeadlineCarga = embarqueRodoviario.DeadlineCarga,
                        DeadlineVgm = embarqueRodoviario.DeadlineVgm,
                        Booking = embarqueRodoviario.Booking,
                        DeadlineDraft = embarqueRodoviario.DeadlineDraft
                    };

                    _context.EmbarqueRodoviarios.Add(novoEmbarque);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = novoEmbarque.ProcessoId });
                }


                return View(embarqueRodoviario);
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar Embarque. {ex.Message}";
                return _error.ConflictError();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || _context.EmbarqueRodoviarios == null)
                    return _error.NotFoundError();

                var dados = await _context.EmbarqueRodoviarios
                       .Include(e => e.AgenteDeCarga)
                       .Include(e => e.Processo)
                     .FirstOrDefaultAsync(e => e.Id == id);

                if (dados == null)

                    return _error.NotFoundError();
                ViewData["AgenteDeCargaId"] = new SelectList(_context.AgenteDeCargas, "Id", "NomeAgenteCarga");

                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmbarqueRodoviario embarqueRodoviario)
        {
            try
            {
                if (id != embarqueRodoviario.Id)
                    return _error.NotFoundError();
                if (ModelState.IsValid)
                {
                    if (Request.Form.ContainsKey("ProcessoId"))
                    {
                        embarqueRodoviario.ProcessoId = Convert.ToInt32(Request.Form["ProcessoId"]);
                    }

                    _context.EmbarqueRodoviarios.Update(embarqueRodoviario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = embarqueRodoviario.ProcessoId });
                }
                return View();
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao editar Embarque. {ex.Message}";
                return _error.ConflictError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado: {ex.Message} Por favor, tente novamente.";
                return _error.InternalServerError();
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null || _context.EmbarqueRodoviarios == null)
                    return _error.NotFoundError();

                var dados = await _context.EmbarqueRodoviarios
                     .Include(e => e.AgenteDeCarga)
                     .Include(e => e.Processo)
                     .FirstOrDefaultAsync(e => e.Id == id);

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Embarques. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Embarques do banco de dados. {ex.Message}";
                return _error.BadRequestError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Embarques do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || _context.EmbarqueRodoviarios == null)
                    return _error.NotFoundError();
                
                    var dados = await _context.EmbarqueRodoviarios
                        .Include(e => e.AgenteDeCarga)
                        .Include(e => e.Processo)
                        .FirstOrDefaultAsync(e => e.Id == id);

                    if (dados == null)
                        return _error.NotFoundError();
                    ViewData["AgenteDeCargaId"] = new SelectList(_context.AgenteDeCargas, "Id", "NomeAgenteCarga");
                    ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");
                    return View(dados);
                
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.EmbarqueRodoviarios.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                    _context.EmbarqueRodoviarios.Remove(dados);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
                
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir Despacho. {ex.Message}";
                return _error.ConflictError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado: {ex.Message}. Por favor, tente novamente.";
                return _error.InternalServerError();
            }
        }
    }
}

