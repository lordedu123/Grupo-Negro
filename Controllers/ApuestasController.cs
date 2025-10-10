using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Grupo_negro.Data;
using Grupo_negro.Models;
using Grupo_negro.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Grupo_negro.Controllers
{
    [Authorize] // Requiere autenticación para todas las acciones
    public class ApuestasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DatosSimuladosService _datosService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApuestasController(
            ApplicationDbContext context, 
            DatosSimuladosService datosService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _datosService = datosService;
            _userManager = userManager;
        }

        // GET: /Apuestas
        public async Task<IActionResult> Index()
        {
            // Inicializar datos simulados si no existen
            await _datosService.InicializarDatosAsync();

            // Obtener todas las ligas para el filtro
            var ligas = await _context.Ligas.ToListAsync();
            ViewBag.Ligas = ligas;

            // Obtener partidos próximos (sin filtro inicial)
            var partidos = await _context.Partidos
                .Include(p => p.EquipoLocal)
                .Include(p => p.EquipoVisitante)
                .Include(p => p.Liga)
                .Where(p => p.Estado == EstadoPartido.Programado && p.FechaHora > DateTime.Now)
                .OrderBy(p => p.FechaHora)
                .ToListAsync();

            return View(partidos);
        }

        // GET: /Apuestas/PorLiga/{ligaId}
        public async Task<IActionResult> PorLiga(int ligaId)
        {
            var partidos = await _context.Partidos
                .Include(p => p.EquipoLocal)
                .Include(p => p.EquipoVisitante)
                .Include(p => p.Liga)
                .Where(p => p.LigaId == ligaId && p.Estado == EstadoPartido.Programado && p.FechaHora > DateTime.Now)
                .OrderBy(p => p.FechaHora)
                .ToListAsync();

            return PartialView("_PartidosPartial", partidos);
        }

        // GET: /Apuestas/Apostar/{partidoId}
        public async Task<IActionResult> Apostar(int partidoId)
        {
            var partido = await _context.Partidos
                .Include(p => p.EquipoLocal)
                .Include(p => p.EquipoVisitante)
                .Include(p => p.Liga)
                .FirstOrDefaultAsync(p => p.Id == partidoId);

            if (partido == null || partido.Estado != EstadoPartido.Programado)
            {
                TempData["Error"] = "El partido no está disponible para apostar.";
                return RedirectToAction(nameof(Index));
            }

            // Obtener el saldo del usuario
            var usuario = await _userManager.GetUserAsync(User);
            ViewBag.SaldoUsuario = usuario?.Saldo ?? 0m;

            var viewModel = new ApuestaViewModel
            {
                PartidoId = partido.Id,
                NombreEquipoLocal = partido.EquipoLocal.Nombre,
                NombreEquipoVisitante = partido.EquipoVisitante.Nombre,
                FechaHora = partido.FechaHora,
                Liga = partido.Liga.Nombre,
                CuotaLocal = partido.CuotaLocal,
                CuotaEmpate = partido.CuotaEmpate,
                CuotaVisitante = partido.CuotaVisitante
            };

            return View(viewModel);
        }

        // POST: /Apuestas/Apostar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apostar(ApuestaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Recargar datos del partido para la vista
                var partidoData = await _context.Partidos
                    .Include(p => p.EquipoLocal)
                    .Include(p => p.EquipoVisitante)
                    .Include(p => p.Liga)
                    .FirstOrDefaultAsync(p => p.Id == model.PartidoId);

                if (partidoData != null)
                {
                    model.NombreEquipoLocal = partidoData.EquipoLocal.Nombre;
                    model.NombreEquipoVisitante = partidoData.EquipoVisitante.Nombre;
                    model.FechaHora = partidoData.FechaHora;
                    model.Liga = partidoData.Liga.Nombre;
                    model.CuotaLocal = partidoData.CuotaLocal;
                    model.CuotaEmpate = partidoData.CuotaEmpate;
                    model.CuotaVisitante = partidoData.CuotaVisitante;
                }
                return View(model);
            }

            var partido = await _context.Partidos
                .FirstOrDefaultAsync(p => p.Id == model.PartidoId && p.Estado == EstadoPartido.Programado);

            if (partido == null)
            {
                TempData["Error"] = "El partido no está disponible para apostar.";
                return RedirectToAction(nameof(Index));
            }

            // Verificar el saldo del usuario
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                TempData["Error"] = "Error al obtener datos del usuario.";
                return RedirectToAction(nameof(Index));
            }

            if (usuario.Saldo < model.MontoApostado)
            {
                TempData["Error"] = $"Saldo insuficiente. Tu saldo actual es ${usuario.Saldo:F2}. Necesitas depositar más dinero para realizar esta apuesta.";
                return RedirectToAction(nameof(Index));
            }

            // Obtener la cuota según el tipo de apuesta
            decimal cuota = model.TipoApuesta switch
            {
                TipoApuesta.GanaLocal => partido.CuotaLocal,
                TipoApuesta.Empate => partido.CuotaEmpate,
                TipoApuesta.GanaVisitante => partido.CuotaVisitante,
                _ => 1.0m
            };

            // Descontar el monto apostado del saldo del usuario
            usuario.Saldo -= model.MontoApostado;
            await _userManager.UpdateAsync(usuario);

            var apuesta = new Apuesta
            {
                UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                PartidoId = model.PartidoId,
                TipoApuesta = model.TipoApuesta,
                MontoApostado = model.MontoApostado,
                CuotaAplicada = cuota,
                PosibleGanancia = model.MontoApostado * cuota,
                FechaApuesta = DateTime.Now,
                Estado = EstadoApuesta.Activa
            };

            _context.Apuestas.Add(apuesta);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"¡Apuesta realizada exitosamente! Posible ganancia: ${apuesta.PosibleGanancia:F2}";
            return RedirectToAction(nameof(MisApuestas));
        }

        // GET: /Apuestas/MisApuestas
        public async Task<IActionResult> MisApuestas()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var apuestas = await _context.Apuestas
                .Include(a => a.Partido)
                    .ThenInclude(p => p.EquipoLocal)
                .Include(a => a.Partido)
                    .ThenInclude(p => p.EquipoVisitante)
                .Include(a => a.Partido)
                    .ThenInclude(p => p.Liga)
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.FechaApuesta)
                .ToListAsync();

            return View(apuestas);
        }
    }
}