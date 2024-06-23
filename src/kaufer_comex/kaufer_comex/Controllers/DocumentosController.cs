using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class DocumentosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorService _error;

        public DocumentosController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Documentos
                    .Include(d => d.Processo)
                    .ToListAsync();
                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Documentos. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Documentos do banco de dados. {ex.Message}";
                return _error.BadRequestError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Documentos do banco de dados. {ex.Message}";
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
                return View();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Documento documento)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);

                    Documento novoDocumento = new Documento
                    {
                        ProcessoId = processoId,
                        CertificadoOrigem = documento.CertificadoOrigem,
                        CertificadoSeguro = documento.CertificadoSeguro,
                        DataEnvioOrigem = documento.DataEnvioOrigem,
                        TrackinCourier = documento.TrackinCourier,
                        Courier = documento.Courier

                    };

                    _context.Documentos.Add(novoDocumento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = novoDocumento.ProcessoId });
                }


                return View(documento);
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar Documento. {ex.Message}";
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
                if (id == null || _context.Documentos == null)
                    return _error.NotFoundError();

                var dados = await _context.Documentos
                    .Include(d => d.Processo)
                    .FirstOrDefaultAsync(d => d.Id == id);
                if (dados == null)
                {
                    return _error.NotFoundError();
                }
                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Documento documento)
        {
            try
            {
                if (id != documento.Id)
                    return _error.NotFoundError();
                if (ModelState.IsValid)
                {
                    if (Request.Form.ContainsKey("ProcessoId"))
                    {
                        documento.ProcessoId = Convert.ToInt32(Request.Form["ProcessoId"]);
                    }

                    _context.Documentos.Update(documento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Processos", new { id = documento.ProcessoId });
                }

                return View();
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao editar Documento. {ex.Message}";
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
                if (id == null || _context.Documentos == null)
                    return _error.NotFoundError();

                var dados = await _context.Documentos
                    .Include(d => d.Processo)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (SqlException ex)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Documentos. {ex.Message}";
                return _error.InternalServerError();
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Documentos do banco de dados. {ex.Message}";
                return _error.BadRequestError();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Documentos do banco de dados. {ex.Message}";
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || _context.Documentos == null)
                    return _error.NotFoundError();
                    var dados = await _context.Documentos
                    .Include(d => d.Processo)
                    .FirstOrDefaultAsync(d => d.Id == id);

                    if (dados == null)
                        return _error.NotFoundError();
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

                var dados = await _context.Documentos.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                _context.Documentos.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
            }
            catch (DbUpdateException ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir Documento. {ex.Message}";
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


