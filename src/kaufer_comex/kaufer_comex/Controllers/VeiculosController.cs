using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace kaufer_comex.Controllers
{
    public class VeiculosController : Controller
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
           
                var dados = await _context.Veiculos
                        .OrderBy(a => a.Motorista)
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
        
        public async Task<IActionResult> Create(Veiculo veiculo)
        {

            if (ModelState.IsValid)
            {
				int processoId = Convert.ToInt32(Request.Form["ProcessoId"]);


				Veiculo novoVeiculo = new Veiculo
				{
					ProcessoId = processoId,
					Placa = veiculo.Placa,
                    Motorista= veiculo.Motorista,
                   

				};
				_context.Veiculos.Add(novoVeiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processos", new { id = novoVeiculo.ProcessoId });
            }

			return View(veiculo);


                       
        }

        public async Task<IActionResult> Edit(int? id)
        {
            
            
                if (id == null)
                    return NotFound();

                var dados = await _context.Veiculos.FindAsync(id);

                if (dados == null)
                    return NotFound();

                return View(dados);
                      
        }

        [HttpPost]
      
        public async Task<IActionResult> Edit(int id, Veiculo veiculo)
        {


			if (id != veiculo.Id)
				return NotFound();

			if (ModelState.IsValid)
			{
				if (Request.Form.ContainsKey("ProcessoId"))
				{
					veiculo.ProcessoId = Convert.ToInt32(Request.Form["ProcessoId"]);
				}
				_context.Veiculos.Update(veiculo);
				await _context.SaveChangesAsync();
				return RedirectToAction("Details", "Processos", new { id = veiculo.ProcessoId });
			}



			return View();

		}

        public async Task<IActionResult> Details(int? id)
        {
              if (id == null)
                    return NotFound();

                var dados = await _context.Veiculos.FindAsync(id);

                if (dados == null)
                    return NotFound();

                return View(dados);           
        }

        public async Task<IActionResult> Delete(int? id)
        {
            
                if (id == null)
                    return NotFound();

                var dados = await _context.Veiculos
                .Include(d => d.Processo)
				.FirstOrDefaultAsync(d => d.Id == id); ;

			if (dados == null)
                    return NotFound();

                return View(dados);
                       
        }

        [HttpPost, ActionName("Delete")]       
        public async Task<IActionResult> DeleteConfirmed(int? id)
        { 
                if (id == null)
                    return NotFound();

                var dados = await _context.Veiculos.FindAsync(id);

                if (dados == null)
                    return NotFound();

                _context.Veiculos.Remove(dados);
                await _context.SaveChangesAsync();
			return RedirectToAction("Details", "Processos", new { id = dados.ProcessoId });

		}
    }
}
