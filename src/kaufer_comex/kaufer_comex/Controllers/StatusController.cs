using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class StatusController : Controller
    {
		private AppDbContext _context;
        public StatusController(AppDbContext context) 
        {
            _context = context; 
        }  
        public async Task<IActionResult> Index() 
        {
            var dados = await _context.Status.ToListAsync();

            return View(dados);
        }

        public IActionResult Create() 
        { 
            return View();  
        }

        [HttpPost]  
        public async Task<IActionResult> Create(Status Status)
        {
            if(ModelState.IsValid)
            {
                _context.Status.Add(Status);
              await  _context.SaveChangesAsync();
                return RedirectToAction("Index");   

            }


            return View(Status);
        }

        public async Task<IActionResult> Edit(int? id) 
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Status.FindAsync(id);

            if (dados == null)
                return NotFound();
   
            return View(dados);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,Status Status)
        {
            if(id != Status.Id)
            return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(Status);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            return View();  
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
                return NotFound();  

            var dados = await  _context.Status.FindAsync(id);

            if (dados == null)
                return NotFound();  

            return View(dados);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Status.FindAsync(id);

            if (dados == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
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
    }
}
