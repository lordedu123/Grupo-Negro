using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Grupo_negro.Models;

namespace Grupo_negro.ViewComponents
{
    public class NombreUsuarioViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public NombreUsuarioViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var usuario = await _userManager.GetUserAsync(HttpContext.User);
                if (usuario != null)
                {
                    ViewBag.Saldo = usuario.Saldo;
                    return View("Default", usuario.NombreCompleto);
                }
            }
            ViewBag.Saldo = 0m;
            return View("Default", "Usuario");
        }
    }
}