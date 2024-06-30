using GestorEventos.Servicios.Entidades;
using GestorEventos.Servicios.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEventos.WebAdmin.Controllers
{
    public class EventosController : Controller
    {
        private IEventoService eventoService;
        private IPersonaService personaService;
        private IServicioService servicioService;
        private IEventosServiciosService eventosServiciosService;

        public EventosController(IEventoService _eventoService, IPersonaService _personaService, IServicioService _servicioService, IEventosServiciosService _eventosServiciosService)
        {
            this.eventoService = _eventoService;
            this.personaService = _personaService;
            this.servicioService = _servicioService;
            this.eventosServiciosService = _eventosServiciosService;
        }

        // GET: EventosController
        public ActionResult Index()
        {
            var eventos = this.eventoService.GetAllEventosViewModel();
            return View(eventos);
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
            evento.ListaDeServiciosDisponibles = this.servicioService.GetServicios() ?? new List<Servicio>();

            return View(evento);
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
                eventoNuevo.IdUsuario = 0;
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

        // GET: EventosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
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

        [HttpPost("AprobarEvento")]
        public async Task<IActionResult> AprobarEvento(int idEvento, IFormCollection collection)
        {
            _ = this.eventoService.CambiarEstadoEvento(int.Parse(collection["item.idEvento"][0]), 3);

            TempData["Message"] = $"¡Evento aprobado!";
            return View("Index");
        }

        [HttpPost("RechazarEvento")]
        public async Task<IActionResult> RechazarEvento(int idEvento, IFormCollection collection)
        {
            _ = this.eventoService.CambiarEstadoEvento(int.Parse(collection["item.idEvento"][0]), 4);

            TempData["Message"] = $"¡Evento rechazado!";
            return View("Index");
        }
    }
}