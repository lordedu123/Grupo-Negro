using Microsoft.AspNetCore.Identity;
using Grupo_negro.Models;
using System.Security.Claims;

namespace Grupo_negro.Services
{
    public class UsuarioService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> ObtenerUsuarioActualAsync(ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated == true)
            {
                return await _userManager.GetUserAsync(user);
            }
            return null;
        }

        public async Task<string> ObtenerNombreCompletoAsync(ClaimsPrincipal user)
        {
            var usuario = await ObtenerUsuarioActualAsync(user);
            return usuario?.NombreCompleto ?? user?.Identity?.Name ?? "Usuario";
        }
    }
}