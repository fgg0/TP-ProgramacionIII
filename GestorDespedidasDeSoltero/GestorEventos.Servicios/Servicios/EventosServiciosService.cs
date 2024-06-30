using GestorEventos.Servicios.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GestorEventos.Servicios.Servicios
{
    public interface IEventosServiciosService
    {
        IEnumerable<EventosServicios> GetServiciosPorEvento(int IdEvento);
        int PostNuevoEventoServicio(EventosServicios relacionEventoServicio);
    }

    public class EventosServiciosService : IEventosServiciosService
    {


        private string _connectionString;



        public EventosServiciosService()
        {

            //Connection string 
            _connectionString = "Data Source=DESKTOP-GN0G7SH;Initial Catalog=gestioneventos;Integrated Security=True;Encrypt=False";


        }

        public int PostNuevoEventoServicio(EventosServicios relacionEventoServicio)
        {

            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO dbo.EventosServicios(IdEvento, IdServicio, Borrado)" +
                                    "VALUES(   @IdEvento,  @IdServicio,   0)";
                    db.Execute(query, relacionEventoServicio);


                    return relacionEventoServicio.IdEventoServicio;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IEnumerable<EventosServicios> GetServiciosPorEvento(int IdEvento)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<EventosServicios> eventos = db.Query<EventosServicios>("select * from eventosServicios WHERE IdEvento =" + IdEvento.ToString()).ToList();

                return eventos;

            }
        }

    }
}
