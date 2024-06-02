using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class EmbarqueRodoviariosController : Controller
    {
        private readonly AppDbContext _context;
        public EmbarqueRodoviariosController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
        
            var dados = await _context.EmbarqueRodoviarios
                .Include(e => e.AgenteDeCarga)
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

            ViewData["AgenteDeCargaId"] = new SelectList(_context.AgenteDeCargas, "Id", "NomeAgenteCarga");
           
            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmbarqueRodoviario embarqueRodoviario)
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

				};

				_context.EmbarqueRodoviarios.Add(novoEmbarque);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processos", new { id = novoEmbarque.ProcessoId });
            }


            return View(embarqueRodoviario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmbarqueRodoviarios == null)
                return NotFound();

            var dados = await _context.EmbarqueRodoviarios
                   .Include(e => e.AgenteDeCarga)
                   .Include(e => e.Processo)
                 .FirstOrDefaultAsync(e => e.Id == id);

            if (dados == null)

                return NotFound();
            ViewData["AgenteDeCargaId"] = new SelectList(_context.AgenteDeCargas, "Id", "NomeAgenteCarga");
            
            return View(dados);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmbarqueRodoviario embarqueRodoviario)
        {
            if (id != embarqueRodoviario.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                if (Request.Form.ContainsKey("ProcessoId"))
                {
                    embarqueRodoviario.ProcessoId = Convert.ToInt32(Request.Form["ProcessoId"]);
                }

                _context.EmbarqueRodoviarios.Update(embarqueRodoviario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Processos", new { id = embarqueRodoviario.ProcessoId });
            }


            return View();
        }
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null || _context.EmbarqueRodoviarios == null)

                return NotFound();

            var dados = await _context.EmbarqueRodoviarios
                    .Include(e => e.AgenteDeCarga)
                    .Include(e => e.Processo)
                  .FirstOrDefaultAsync(e => e.Id == id);

            if (dados == null)

                return NotFound();

            return View(dados);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || _context.EmbarqueRodoviarios == null)
                return NotFound();

            var dados = await _context.EmbarqueRodoviarios
                   .Include(e => e.AgenteDeCarga)
                   .Include(e => e.Processo)
                 .FirstOrDefaultAsync(e => e.Id == id);

            if (dados == null)

                return NotFound();
            ViewData["AgenteDeCargaId"] = new SelectList(_context.AgenteDeCargas, "Id", "NomeAgenteCarga");
            ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");
            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)

                return NotFound();

            var dados = await _context.EmbarqueRodoviarios.FindAsync(id);

            if (dados == null)

                return NotFound();
            _context.EmbarqueRodoviarios.Remove(dados);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
        }
    }
}
