using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Grupo_negro.Data;
using Grupo_negro.Models;

namespace Grupo_negro.Controllers
{
    [Authorize]
    public class SaldoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SaldoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Saldo
        public async Task<IActionResult> Index()
        {
            var usuario = await _userManager.GetUserAsync(User);
            
            // Log de acceso para auditoría
            if (usuario != null)
            {
                Console.WriteLine($"Usuario {usuario.Email} accedió al panel de saldo");
            }
            
            return View(usuario);
        }

        // GET: Saldo/Depositar
        public IActionResult Depositar()
        {
            return View();
        }

        // POST: Saldo/Depositar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Depositar(DepositoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validación adicional de monto
            if (model.Monto <= 0)
            {
                ModelState.AddModelError("Monto", "El monto debe ser mayor a cero");
                return View(model);
            }

            // Validación de monto máximo
            if (model.Monto > 10000)
            {
                ModelState.AddModelError("Monto", "El monto no puede exceder $10,000");
                return View(model);
            }

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Aquí iría la integración con PayPal/Yape
            // Por ahora simulamos un depósito exitoso
            var montoAnterior = usuario.Saldo;
            usuario.Saldo += model.Monto;
            await _userManager.UpdateAsync(usuario);

            TempData["SuccessMessage"] = $"Depósito de ${model.Monto:N2} realizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Saldo/Retirar
        public async Task<IActionResult> Retirar()
        {
            var usuario = await _userManager.GetUserAsync(User);
            ViewBag.SaldoDisponible = usuario?.Saldo ?? 0;
            return View();
        }

        // POST: Saldo/Retirar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Retirar(RetiroViewModel model)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SaldoDisponible = usuario.Saldo;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Monto > usuario.Saldo)
            {
                ModelState.AddModelError("Monto", "No tienes suficiente saldo para realizar este retiro.");
                return View(model);
            }

            // Aquí iría la integración con PayPal/Yape
            // Por ahora simulamos un retiro exitoso
            usuario.Saldo -= model.Monto;
            await _userManager.UpdateAsync(usuario);

            TempData["SuccessMessage"] = $"Retiro de ${model.Monto:N2} procesado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Saldo/Historial
        public IActionResult Historial()
        {
            // Por ahora retornamos una vista vacía
            // En el futuro se implementará con una tabla de transacciones
            return View();
        }
    }
}