using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Grupo_negro.Models;

namespace Grupo_negro.ViewComponents
{
    public class SaldoUsuarioViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SaldoUsuarioViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var usuario = await _userManager.GetUserAsync(HttpContext.User);
                return View("Default", usuario?.Saldo ?? 0m);
            }
            return View("Default", 0m);
        }
    }
}