using Microsoft.AspNetCore.Mvc;
using Grupo_negro.Models;
using Grupo_negro.Data;

namespace ApuestasDeportivas.Controllers
{
    public class SolicitudesLocalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolicitudesLocalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult RequisitosPrevios()
        {
            return View("RequisitosPrevios");
        }

        // Vista para crear solicitud
        public IActionResult Create()
        {
            ViewBag.Locales = _context.LocalesFisicos.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SolicitudLocal solicitud)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solicitud);
                await _context.SaveChangesAsync();
                return RedirectToAction("Seguimiento", new { usuarioId = solicitud });
            }
            ViewBag.Locales = _context.LocalesFisicos.ToList();
            return View(solicitud);
        }

        public IActionResult Seguimiento()
        {
            var solicitudes = _context.SolicitudesLocales.ToList();
            return View(solicitudes);
        }

        [HttpPost]
        public IActionResult AgregarComentario(int id, string comentario)
        {
            var solicitud = _context.SolicitudesLocales.Find(id);
            if (solicitud == null) return NotFound();

            solicitud.ComentariosAdicionales  = comentario;
            _context.SaveChanges();

            return RedirectToAction("Seguimiento");
        }
    }
}
