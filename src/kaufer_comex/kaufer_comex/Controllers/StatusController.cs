using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class StatusController : Controller
    {
		private readonly AppDbContext _context;
        public StatusController(AppDbContext context) 
        {
            _context = context; 
        }  
        public async Task<IActionResult> Index() 
        {
            try
            {
                var dados = await _context.Status
                    .OrderBy(a => a.StatusAtual)
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
		public async Task<IActionResult> Create(Status Status)
        {
            try
            {
                if (ModelState.IsValid)
                {
					var StatusExistente = await _context.Status
				  .AnyAsync(a => a.StatusAtual == Status.StatusAtual);

					if (StatusExistente)
					{
						ModelState.AddModelError("StatusAtual", "Esse Status já está cadastrado.");
						return View(Status);
					}
					_context.Status.Add(Status);
					await _context.SaveChangesAsync();
					return RedirectToAction("Index");

				}
                return View(Status);
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

                var dados = await _context.Status.FindAsync(id);

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
		public async Task<IActionResult> Edit(int id,Status Status)
        {
            try
            {
                if (id != Status.Id)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    _context.Update(Status);
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

                var dados = await _context.Status.FindAsync(id);

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

                var dados = await _context.Status.FindAsync(id);

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

                var dados = await _context.Status.FindAsync(id);

                if (dados == null)
                    return NotFound();

                _context.Status.Remove(dados);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
			catch
			{
				TempData["MensagemErro"] = $"Esse status está vinculado a um processo. Não pode ser excluído.";
				return View();
			}
		}
    }
}
