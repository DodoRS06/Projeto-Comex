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

        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.ValorProcessos
                    .ToListAsync();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Erro ao carregar os dados. Tente novamente";
                return View();
            }
        }
        public IActionResult Create(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ViewData["ProcessoId"] = id.Value;
                return View();
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ValorProcesso valorprocesso)
        {
            try
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
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.ValorProcessos.FindAsync(id);
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
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ValorProcesso valorprocesso)
        {
            try
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
                    return RedirectToAction("Details", "Processos", new { id = valorprocesso.ProcessoId });
                }



                return View();
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }


        public async Task<IActionResult> Delete(int? id)
        {
            try
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
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.ValorProcessos.FindAsync(id);

                if (id == null)
                    return NotFound();

                _context.ValorProcessos.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }

        }
    }
}
