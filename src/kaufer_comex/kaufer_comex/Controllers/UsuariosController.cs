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
using Microsoft.Data.SqlClient;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorService _error;
        public UsuariosController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
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
            try
            {
                if (string.IsNullOrEmpty(usuario.Senha))
                {
                    ViewBag.Message = "Senha não pode estar vazia!";
                    return View();
                }

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
            catch (Exception)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro ao tentar fazer login. Por favor, tente novamente mais tarde.";
                return _error.InternalServerError();
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Usuarios");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro ao tentar fazer Logout.";
                return _error.InternalServerError();
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            try
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
            catch (SqlException)
            {
                TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Usuário.";
                return _error.InternalServerError();
            }
            catch (InvalidOperationException)
            {
                TempData["MensagemErro"] = $"Erro ao recuperar Usuário do banco de dados.";
                return _error.BadRequestError();
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro ao tentar visualizar dados de Usuário.";
                return _error.InternalServerError();
            }
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Usuarios.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch { return _error.InternalServerError(); }
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

                try
                {
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

                    // Gerar senha provisória
                    string senhaProvisoria = GerarSenhaProvisoria();
                    usuario.Senha = BCrypt.Net.BCrypt.HashPassword(senhaProvisoria);

                    _context.Add(usuario);
                    await _context.SaveChangesAsync();

                    // Enviar e-mail após o cadastro
                    var emailService = new EmailService();
                    string assunto = "Bem-vindo ao nosso sistema";
                    string corpo = $"Olá {usuario.NomeFuncionario}, seja bem-vindo ao nosso sistema!<br/><br/>" +
                                   $"Sua senha é: <strong>{senhaProvisoria}</strong><br/><br/>";

                    try
                    {
                        emailService.SendEmail(usuario.Email, assunto, corpo);
                        Console.WriteLine("Solicitação de envio de e-mail foi feita.");
                        ViewBag.SuccessMessage = "Mensagem enviada para o e-mail cadastrado com sucesso! Por favor, verifique o e-mail para realizar o primeiro acesso.";
                        return View(usuario);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Erro ao tentar enviar o e-mail: ");
                        ModelState.AddModelError("Email", "Por favor, digite um e-mail válido.");
                    }

                    if (ModelState.IsValid)
                    {
                        return RedirectToAction("Login");
                    }
                }
                catch (Exception)
                {
                    TempData["MensagemErro"] = $"Ocorreu um erro ao tentar Cadastrar um Usuário.";
                    return _error.InternalServerError();
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
            try
            {
                if (id == null || _context.Usuarios == null)
                {
                    return _error.NotFoundError();
                }

                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return _error.NotFoundError();
                }
                return View(usuario);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeFuncionario,Email,Senha,CPF,Perfil")] Usuario usuario)
        {
            try
            {
                if (id != usuario.Id)
                {
                    return _error.NotFoundError();
                }

                if (ModelState.IsValid)
                {
                    var userInDb = await _context.Usuarios.FindAsync(id);
                    if (userInDb == null)
                    {
                        return _error.NotFoundError();
                    }

                    // Verificar se o e-mail ou CPF já existem no banco de dados para outro usuário
                    bool emailExiste = _context.Usuarios.Any(u => u.Email == usuario.Email && u.Id != usuario.Id);
                    bool cpfExiste = _context.Usuarios.Any(u => u.CPF == usuario.CPF && u.Id != usuario.Id);

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

                    if (!string.IsNullOrEmpty(usuario.Senha))
                    {
                        userInDb.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                    }
                    else
                    {
                        // Se a senha nao foi fornecida, mantenha a senha original no banco de dados
                        userInDb.Senha = userInDb.Senha;
                    }

                    // Aplicar as alterações na entidade carregada do banco de dados
                    userInDb.NomeFuncionario = usuario.NomeFuncionario;
                    userInDb.Email = usuario.Email;
                    userInDb.CPF = usuario.CPF;
                    userInDb.Perfil = usuario.Perfil;

                    _context.Update(userInDb);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                return View(usuario);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || _context.Usuarios == null)
                {
                    return _error.NotFoundError();
                }

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
                if (usuario == null)
                {
                    return _error.NotFoundError();
                }

                return View(usuario);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Usuarios == null)
                {
                    return Problem("Entity set 'AppDbContext.Usuarios' is null.");
                }

                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario != null)
                {
                    _context.Usuarios.Remove(usuario);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        // GET: Usuarios/ForgotPassword
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Usuarios/ForgotPassword
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email, string cpf)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    ViewBag.ErrorMessage = "O e-mail é obrigatório.";
                    return View();
                }

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.CPF == cpf);
                if (usuario == null)
                {
                    ViewBag.ErrorMessage = "E-mail ou CPF não encontrados.";
                    return View();
                }

                string novaSenha = GerarSenhaProvisoria();
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);
                _context.Update(usuario);
                await _context.SaveChangesAsync();

                var emailService = new EmailService();
                string assunto = "Recuperação de Senha";
                string corpo = $"Olá {usuario.NomeFuncionario},<br/><br/>" +
                               $"Sua nova senha é: <strong>{novaSenha}</strong><br/><br/>";

                try
                {
                    emailService.SendEmail(usuario.Email, assunto, corpo);
                    TempData["SuccessMessage"] = "A nova senha foi enviada para o seu e-mail.";
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "Erro ao enviar e-mail: ";
                }

                return View();
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ocorreu um erro ao tentar recuperar a senha. Por favor, tente novamente mais tarde.";
                return _error.NotFoundError();
            }
        }

    }
}
