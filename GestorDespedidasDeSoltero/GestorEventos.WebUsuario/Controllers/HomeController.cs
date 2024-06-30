using GestorEventos.WebUsuario.Models;
using GestorEventos.Servicios.Servicios;
using GestorEventos.Servicios.Entidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace GestorEventos.WebUsuario.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventoService _eventoService;

        public HomeController(ILogger<HomeController> logger, IEventoService eventoService)
        {
            _logger = logger;
            _eventoService = eventoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int idUsuario = int.Parse(HttpContext.User.Claims.First(x => x.Type == "usuarioSolterout").Value);
                    var eventos = _eventoService.GetMisEventos(idUsuario);
                    return View(eventos);
                }
                else
                {
                    return View(new List<EventoViewModel>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los eventos en el HomeController");
                return View(new List<EventoViewModel>());
            }
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