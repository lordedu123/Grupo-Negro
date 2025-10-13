
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Grupo_negro.Models;
using Grupo_negro.Services;

namespace Grupo_negro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CookieService _cookieService;

        public HomeController(ILogger<HomeController> logger, CookieService cookieService)
        {
            _logger = logger;
            _cookieService = cookieService;
        }

        public IActionResult Index()
        {
            // Obtener tema preferido del usuario desde cookies
            var temaPreferido = _cookieService.GetCookie("TemaPreferido") ?? "claro";
            ViewBag.TemaPreferido = temaPreferido;

            // Contar visitas del usuario
            var contadorVisitas = _cookieService.GetCookie("ContadorVisitas");
            int visitas = 1;
            
            if (!string.IsNullOrEmpty(contadorVisitas) && int.TryParse(contadorVisitas, out int visitasActuales))
            {
                visitas = visitasActuales + 1;
            }
            
            _cookieService.SetCookie("ContadorVisitas", visitas.ToString(), 30);
            ViewBag.ContadorVisitas = visitas;

            // Guardar última visita
            _cookieService.SetCookie("UltimaVisita", DateTime.Now.ToString("dd/MM/yyyy HH:mm"), 30);

            return View();
        }

        [HttpPost]
        public IActionResult CambiarTema(string tema)
        {
            if (!string.IsNullOrEmpty(tema) && (tema == "claro" || tema == "oscuro"))
            {
                _cookieService.SetCookie("TemaPreferido", tema, 365); // Cookie por 1 año
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
