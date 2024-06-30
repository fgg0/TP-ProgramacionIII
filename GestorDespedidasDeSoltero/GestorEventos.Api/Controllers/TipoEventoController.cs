using GestorEventos.Servicios.Entidades;
using GestorEventos.Servicios.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestorEventos.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TipoEventoController : Controller
	{

		[HttpGet]
		public IActionResult Get()
		{
			TipoEventoService tipoEventoService = new TipoEventoService();

			return Ok(tipoEventoService.GetTipoEventos());
		}


		[HttpGet("{idTipoEvento:int}")]
		public IActionResult Get(int idTipoEvento)
		{
			TipoEventoService tipoEventoService = new TipoEventoService();
			TipoEvento tipoEvento = null;

			tipoEvento = tipoEventoService.GetTipoEventoPorId(idTipoEvento);

			if (tipoEvento == null)
				return NotFound();
			else
				return Ok(tipoEvento);
		}


	}
}
