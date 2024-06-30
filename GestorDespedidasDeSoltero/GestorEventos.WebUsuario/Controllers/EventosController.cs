// Controlador eventos de la web usuario
using GestorEventos.Servicios.Entidades;
using GestorEventos.Servicios.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorEventos.WebUsuario.Controllers
{
    [Authorize]
    public class EventosController : Controller
    {
        private IEventoService eventoService;
        private IPersonaService personaService;
        private IServicioService servicioService;
        private IEventosServiciosService eventosServiciosService;

        public EventosController(IEventoService _eventoService, IPersonaService _personaService, IServicioService _servicioService, IEventosServiciosService _eventosServiciosService )
        {
            this.eventoService = _eventoService;
            this.personaService = _personaService;
            this.servicioService = _servicioService;
            this.eventosServiciosService = _eventosServiciosService;
        }

        // GET: EventosController
        public ActionResult Index()
        {

            int idUsuario = int.Parse(
                    HttpContext.User.Claims.First(x => x.Type == "usuarioSolterout").Value);

            var eventos = this.eventoService.GetMisEventos(idUsuario);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult GetEventDetails(int id)
        {
            var evento = this.eventoService.GetEventoPorId(id);
            if (evento == null)
            {
                return NotFound();
            }

            var listaServiciosDisponibles = this.servicioService.GetServicios();
            var listaIdServiciosContratados = this.eventosServiciosService.GetServiciosPorEvento(evento.IdEvento);
            List<Servicio> listaServicios = new List<Servicio>();

            foreach (var servicio in listaIdServiciosContratados)
            {
                var servicioContratado = listaServiciosDisponibles.FirstOrDefault(x => x.IdServicio == servicio.IdServicio);
                if (servicioContratado != null)
                {
                    listaServicios.Add(servicioContratado);
                }
            }

            var result = new
            {
                evento.NombreEvento,
                evento.FechaEvento,
                evento.CantidadPersonas,
                Servicios = listaServicios.Select(s => new { s.Descripcion, s.PrecioServicio })
            };

            return Json(result);
        }

        // GET: EventosController/Create
        public ActionResult Create()
        {
            var evento = new EventoModel();
            evento.ListaDeServiciosDisponibles = this.servicioService.GetServicios();

            return View(evento);
        }

        // GET: EventosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        } 

            // POST: EventosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {

                Persona personaAgasajada = new Persona();
                personaAgasajada.Nombre = collection["Nombre"].ToString();
                personaAgasajada.Apellido = collection["Apellido"].ToString();
                personaAgasajada.Email = collection["Email"].ToString();
                personaAgasajada.Telefono = collection["Telefono"].ToString();
                personaAgasajada.Borrado = false;
                personaAgasajada.Direccion = collection["Direccion"].ToString();

                int IdPersonaAgasajada = personaService.AgregarNuevaPersona(personaAgasajada);





                Evento eventoNuevo = new Evento();
                eventoNuevo.IdPersonaAgasajada = IdPersonaAgasajada;

                eventoNuevo.CantidadPersonas = int.Parse(collection["CantidadPersonas"].ToString());
                eventoNuevo.Visible = true;
                eventoNuevo.IdUsuario = int.Parse(HttpContext.User.Claims.First(x => x.Type == "usuarioSolterout").Value); // HttpContext.User.Identity.Id;
                eventoNuevo.FechaEvento = DateTime.Parse(collection["FechaEvento"].ToString());
                eventoNuevo.IdTipoEvento = int.Parse(collection["IdTipoEvento"].ToString());
                eventoNuevo.NombreEvento = collection["NombreEvento"].ToString();
                eventoNuevo.IdEstadoEvento = 2; //Pendiente de Aprobacion


                int idEventoNuevo = this.eventoService.PostNuevoEvento(eventoNuevo);

                foreach (var idServicio in (collection["Servicio"]))
                {
                    EventosServicios relacionEventoServicio = new EventosServicios();

                    relacionEventoServicio.IdEvento = idEventoNuevo;
                    relacionEventoServicio.IdServicio = int.Parse(idServicio.ToString());
                    relacionEventoServicio.Borrado = false;


                    this.eventosServiciosService.PostNuevoEventoServicio(relacionEventoServicio);
                }



                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }


        // POST: EventosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        

        // POST: EventosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
