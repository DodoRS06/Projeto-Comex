using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class ItensController : Controller
    {
        private readonly AppDbContext _context;

        public ItensController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Itens
                        .OrderBy(a => a.DescricaoProduto)
                            .ToListAsync();

                return View(dados);
            }
			catch
			{
				TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
				return View();
			}
		}

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Item item)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var ItemExistente = await _context.Itens
                      .AnyAsync(a => a.DescricaoProduto == item.DescricaoProduto);
                   

					if (ItemExistente)
                    {
                        ModelState.AddModelError("DescricaoProduto", "Esse Item já está cadastrado.");
                        return View(item);
                    }
                    _context.Itens.Add(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");

                }
                return View(item);
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

                var dados = await _context.Itens.FindAsync(id);

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
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Item item)
        {
            try
            {
                if (id != item.Id)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    _context.Itens.Update(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");

                }

                return View();
            }
			catch
			{
				TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
				return View();
			}
		}

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Itens.FindAsync(id);

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

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Itens.FindAsync(id);

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

        [HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Itens.FindAsync(id);

                if (dados == null)
                    return NotFound();

                _context.Itens.Remove(dados);
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
