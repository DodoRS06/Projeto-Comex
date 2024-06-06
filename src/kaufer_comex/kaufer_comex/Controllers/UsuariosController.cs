using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kaufer_comex.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace kaufer_comex.Controllers
{
    [Authorize (Roles="Admin")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {


            var dados = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (dados == null)
            {
                ViewBag.Message = "Usuário e/ou Senha inválidos!";
                return View();
            }

            bool SenhaOk = BCrypt.Net.BCrypt.Verify(usuario.Senha, dados.Senha);

            if (SenhaOk)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dados.NomeFuncionario),
                    new Claim(ClaimTypes.Email, dados.Email),
                    new Claim(ClaimTypes.NameIdentifier, dados.Id.ToString()), 
                    new Claim("CPF", dados.CPF.ToString()),
                    new Claim(ClaimTypes.Role, dados.Perfil.ToString()) 
                };

                var usuarioIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(usuarioIdentity);

                var props = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(8),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(principal, props);

                return RedirectToAction("Index", "Processos");

            }

            ViewBag.Message = "Usuário e/ou Senha inválidos!";
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Usuarios");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {

            if (User.IsInRole("Admin"))
            {
                // todos os usuarios 
                var usuarios = await _context.Usuarios
                .OrderBy(d => d.NomeFuncionario)
               .ToListAsync();
                return View(usuarios);
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    // somente usuário autenticado - User 
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
                    if (usuario != null)
                    {
                        return View(new List<Usuario> { usuario });
                    }
                }
            }

            return View(new List<Usuario>());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeFuncionario,Email,CPF,Perfil")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(usuario.Email))
                {
                    ModelState.AddModelError("Email", "O e-mail é obrigatório.");
                }

                if (string.IsNullOrEmpty(usuario.CPF))
                {
                    ModelState.AddModelError("CPF", "O CPF é obrigatório.");
                }

                if (!ModelState.IsValid)
                {
                    return View(usuario);
                }

                bool emailExiste = _context.Usuarios.Any(u => u.Email == usuario.Email);
                bool cpfExiste = _context.Usuarios.Any(u => u.CPF == usuario.CPF);

                if (emailExiste || cpfExiste)
                {
                    if (emailExiste)
                    {
                        ModelState.AddModelError("Email", "Já existe um usuário com este e-mail.");
                    }

                    if (cpfExiste)
                    {
                        ModelState.AddModelError("CPF", "Já existe um usuário com este CPF.");
                    }
                    return View(usuario);
                }

                //Gerar senha provisória
                string senhaProvisoria = GerarSenhaProvisoria();
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(senhaProvisoria);

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                //Enviar e-mail após o cadastro
                var emailService = new EmailService();
                string assunto = "Bem-vindo ao nosso sistema";
                string corpo = $"Olá {usuario.NomeFuncionario}, seja bem-vindo ao nosso sistema!<br/><br/>" +
                               $"Sua senha provisória para o primeiro acesso é: <strong>{senhaProvisoria}</strong><br/><br/>" +
                               $"Por favor, recomendamos que altere sua senha após o primeiro login.";

                try
                {
                    emailService.SendEmail(usuario.Email, assunto, corpo);
                    Console.WriteLine("Solicitação de envio de e-mail foi feita.");
                    TempData["SuccessMessage"] = "Mensagem enviada para o e-mail cadastrado com sucesso! Por favor, verifique o e-mail para realizar o primeiro acesso.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao tentar enviar o e-mail: " + ex.Message);
                    ModelState.AddModelError("Email", "Por favor, digite um e-mail válido.");
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Login");
                }
            }
            return View(usuario);
        }

        //senha provisória
        private string GerarSenhaProvisoria()
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            return new string(Enumerable.Repeat(caracteres, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeFuncionario,Email,Senha,CPF,Perfil")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'AppDbContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
