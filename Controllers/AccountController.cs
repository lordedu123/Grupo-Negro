using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Grupo_negro.Models;
using Grupo_negro.Data;
using Microsoft.EntityFrameworkCore;

namespace Grupo_negro.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, 
                    model.Password, 
                    model.RememberMe, 
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Cuenta bloqueada temporalmente por múltiples intentos fallidos.");
                    }
                    else if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError(string.Empty, "Cuenta no verificada. Por favor confirma tu email.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos. Verifica tus datos.");
                    }
                }
            }

            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Crear registro adicional en tabla Usuarios
                    var usuario = new Usuario
                    {
                        Nombres = model.Nombres,
                        Apellidos = model.Apellidos,
                        DNI = model.DNI,
                        Celular = model.Celular,
                        Correo = model.Email,
                        Negocio = model.Negocio,
                        Password = model.Password // Nota: En producción, no guardes la contraseña en texto plano
                    };

                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    string errorMessage = error.Code switch
                    {
                        "DuplicateUserName" => "Este correo electrónico ya está registrado.",
                        "DuplicateEmail" => "Este correo electrónico ya está registrado.",
                        "PasswordTooShort" => "La contraseña debe tener al menos 6 caracteres.",
                        "PasswordRequiresDigit" => "La contraseña debe contener al menos un número.",
                        "PasswordRequiresLower" => "La contraseña debe contener al menos una letra minúscula.",
                        "PasswordRequiresUpper" => "La contraseña debe contener al menos una letra mayúscula.",
                        "InvalidEmail" => "El formato del correo electrónico no es válido.",
                        _ => error.Description
                    };
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #region Helpers

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}