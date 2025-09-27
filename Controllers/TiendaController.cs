using Microsoft.AspNetCore.Mvc;

namespace Grupo_negro.Controllers
{
    public class TiendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
