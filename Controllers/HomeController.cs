
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Grupo_negro.Models;

namespace Grupo_negro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Almacenamiento en memoria de usuarios registrados
        private static List<Usuario> usuarios = new List<Usuario>();
        private static Usuario usuarioActual = null;

        public IActionResult Index()
        {
            ViewBag.UsuarioActual = usuarioActual;
            return View("Inicio");
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario model)
        {
            if (ModelState.IsValid)
            {
                usuarios.Add(model);
                TempData["RegistroExitoso"] = "¡Registro exitoso! Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            var user = usuarios.FirstOrDefault(u => u.Correo == correo && u.Password == password);
            if (user != null)
            {
                usuarioActual = user;
                return RedirectToAction("Bienvenido");
            }
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        public IActionResult Bienvenido()
        {
            if (usuarioActual == null)
                return RedirectToAction("Login");
            ViewBag.UsuarioActual = usuarioActual;
            return View();
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
