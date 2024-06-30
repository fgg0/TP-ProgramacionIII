using GestorEventos.Servicios.Entidades;
using GestorEventos.Servicios.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestorEventos.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PersonaController : Controller
	{
        private IPersonaService personaService;

        public PersonaController(IPersonaService _personaService)
		{
			personaService = _personaService;
		}

		[HttpGet]
		public IActionResult Get()
		{ 

			return Ok(personaService.GetPersonas());
		}

		[HttpGet("{idPersona:int}")]
		public IActionResult GetPersonaPorId(int idPersona)
		{


			var persona = this.personaService.GetPersonaSegunId(idPersona);

			if (persona == null)
				return NotFound();
			else
				return Ok(persona);
		}


		[HttpPost]
		public IActionResult PostPersona([FromBody] Persona persona)
		{ 

			personaService.AgregarNuevaPersona(persona);

			return Ok();
		}

		[HttpPut("{idPersona:int}")]
		public IActionResult PutPersona(int idPersona, [FromBody] Persona persona)
		{ 

			personaService.ModificarPersona(idPersona, persona);

			return Ok();

		}

		[HttpPatch("borradologico/{idPersona:int}")]
		public IActionResult BorradoLogicoPersona(int idPersona)
		{ 

			personaService.BorrarLogicamentePersona(idPersona);

			return Ok();
		}

		[HttpDelete("{idPersona:int}")]
		public IActionResult BorradoFisico(int idPersona)
		{ 
			personaService.BorrarFisicamentePersona(idPersona);

			return Ok();
		}


	}
}
