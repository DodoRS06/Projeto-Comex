using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class ValoresProcessosController : Controller
    {
        private readonly AppDbContext _context;

        public ValoresProcessosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
			if (id == null)
			{
				return NotFound();
			}

			var dados = await _context.ValorProcessos
                .Where(v => v.ProcessoId == id)
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
        public async Task<IActionResult> Create(ValorProcesso valorprocesso)
        {
            if (ModelState.IsValid)
            {
				int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);


				ValorProcesso novoValor = new ValorProcesso
				{
					ProcessoId = processoId,
					ValorExw = valorprocesso.ValorExw,
                    ValorFobFca = valorprocesso.ValorFobFca,
                    ValorTotalCif = valorprocesso.ValorTotalCif,
                    FreteInternacional = valorprocesso.FreteInternacional,
                    SeguroInternaciona = valorprocesso.SeguroInternaciona,
                    Moeda = valorprocesso.Moeda,

				};
				_context.ValorProcessos.Add(novoValor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processos", new { id = novoValor.ProcessoId });
            }

			return View(valorprocesso);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ValorProcessos.FindAsync(id);
            if (dados == null)
                return NotFound();

			return View(dados);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ValorProcesso valorprocesso)
        {
            if (id != valorprocesso.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
				if (Request.Form.ContainsKey("ProcessoId"))
				{
					valorprocesso.ProcessoId = Convert.ToInt32(Request.Form["ProcessoId"]);
				}
				_context.ValorProcessos.Update(valorprocesso);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

		

			return View();
        }
       
                public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ValorProcessos
				.Include(d => d.Processo)
				.FirstOrDefaultAsync(d => d.Id == id); ;

            if (id == null)
                return NotFound();

            return View(dados);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ValorProcessos
				.Include(d => d.Processo)
				.FirstOrDefaultAsync(d => d.Id == id); ;

            if (id == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ValorProcessos.FindAsync(id);

            if (id == null)
                return NotFound();

            _context.ValorProcessos.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = dados.ProcessoId });

        }
    }
}
