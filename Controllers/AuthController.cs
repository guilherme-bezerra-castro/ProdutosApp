using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosApp.Data;
using ProdutosApp.Models;
using ProdutosApp.Models.Auth;

namespace ProdutosApp.Controllers;

public class AuthController : Controller
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AuthController(AppDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return View(model);
        }

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha);
        if (resultado == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Produtos");
    }

    [HttpGet]
    public IActionResult Registrar() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(RegistroViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var emailJaExiste = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
        if (emailJaExiste)
        {
            ModelState.AddModelError(nameof(model.Email), "Este e-mail já está cadastrado.");
            return View(model);
        }

        var usuario = new Usuario { Nome = model.Nome, Email = model.Email };
        usuario.SenhaHash = _passwordHasher.HashPassword(usuario, model.Senha);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        TempData["Sucesso"] = "Cadastro realizado com sucesso! Faça login para continuar.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}