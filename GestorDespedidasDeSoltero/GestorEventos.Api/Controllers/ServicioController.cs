using GestorEventos.Servicios.Entidades;
using GestorEventos.Servicios.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorEventos.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
	[ApiController]
	public class ServicioController : ControllerBase
	{
        private IServicioService serviciosService;

        public ServicioController(IServicioService _serviciosService)
		{
			serviciosService = _serviciosService;
		}


		[HttpGet]
		public IActionResult GetServicios()
		{
			 

			return Ok(serviciosService.GetServicios());
		}

		[HttpGet("{idServicio:int}")]
		public IActionResult GetServicioPorId(int idServicio)
		{ 

			var servicio = serviciosService.GetServiciosPorId(idServicio);

			if (servicio == null)
				return NotFound();
			else
				return Ok(servicio);
		}

		[HttpPost("nuevo")]
		public IActionResult PostNuevoServicio([FromBody] Servicio servicionuevo)
		{
			 
			serviciosService.AgregarNuevoServicio(servicionuevo);

			return Ok();
		}



	}
}
