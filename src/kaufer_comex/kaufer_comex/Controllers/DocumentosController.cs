using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class DocumentosController : Controller
    {
        private readonly AppDbContext _context;
        public DocumentosController(AppDbContext context)
        {
            _context = context;
        }
		public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dados = await _context.Documentos
                .Where(d => d.ProcessoId == id)
                .ToListAsync();

            return View(dados);
        }
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["ProcessoId"] = id.Value;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Documento documento)
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
                    DataEnvioSeguro = documento.DataEnvioSeguro,
                    TrackinCourier = documento.TrackinCourier,
					
				};

				_context.Documentos.Add(novoDocumento);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processos", new { id = novoDocumento.ProcessoId });
            }


            return View(documento);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Documentos == null)
                return NotFound();

            var dados = await _context.Documentos
                .Include(d=>d.Processo)
				.FirstOrDefaultAsync(d => d.Id == id);
			if (dados == null)

				return NotFound();
			
			return View(dados);
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Documento documento)
        {
            if (id != documento.Id)
                return NotFound();
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

        public async Task<ActionResult> Details(int? id)
        {
			if (id == null || _context.Documentos == null)

				return NotFound();
            var dados = await _context.Documentos
				.Include(e => e.Processo)
				.FirstOrDefaultAsync(d => d.Id == id);

			if (dados == null)

                return NotFound();

            return View(dados);
        }

        public async Task<ActionResult> Delete(int? id)
        {
			if (id == null || _context.Documentos == null)

				return NotFound();
            var dados = await _context.Documentos
                .Include(d => d.Processo)
				.FirstOrDefaultAsync(d => d.Id == id);
			if (dados == null)

                return NotFound();
			ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");
			return View(dados);
        }

        [HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int? id)
        {
			if (id == null)

				return NotFound();

            var dados = await _context.Documentos.FindAsync(id);

            if (dados == null)

                return NotFound();
            _context.Documentos.Remove(dados);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
        }
    }
}


