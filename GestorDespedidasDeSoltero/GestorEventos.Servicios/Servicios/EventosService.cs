using GestorEventos.Servicios.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dapper;

namespace GestorEventos.Servicios.Servicios
{
    public interface IEventoService
    {
        bool DeleteEvento(int idEvento);
        IEnumerable<Evento> GetAllEventos();
        IEnumerable<EventoViewModel> GetAllEventosViewModel();
        IEnumerable<EventoViewModel> GetMisEventos(int IdUsuario);
        Evento GetEventoPorId(int IdEvento);
        int PostNuevoEvento(Evento evento);
        bool PutNuevoEvento(int idEvento, Evento evento);
        bool CambiarEstadoEvento(int idEvento, int idEstado);
    }

    public class EventoService : IEventoService
    {
        private string _connectionString;



        public EventoService()
        {

            //Connection string 
            _connectionString = "Data Source=DESKTOP-GN0G7SH;Initial Catalog=gestioneventos;Integrated Security=True;Encrypt=False";


        }


        public IEnumerable<Evento> GetAllEventos()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<Evento> eventos = db.Query<Evento>("SELECT * FROM Eventos WHERE Borrado = 0").ToList();

                return eventos;

            }
        }

        public IEnumerable<EventoViewModel> GetMisEventos(int idUsuario)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<EventoViewModel> eventos = db.Query<EventoViewModel>("select eventos.*, EstadosEventos.Descripcion EstadoEvento from eventos left join EstadosEventos on EstadosEventos.IdEstadoEvento = eventos.idEstadoEvento WHERE Eventos.IdUsuario =" + idUsuario.ToString()).ToList();

                return eventos;

            } 
        }


        public IEnumerable<EventoViewModel> GetAllEventosViewModel()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<EventoViewModel> eventos = db.Query<EventoViewModel>("select eventos.*, EstadosEventos.Descripcion EstadoEvento from eventos left join EstadosEventos on EstadosEventos.IdEstadoEvento = eventos.idEstadoEvento").ToList();

                return eventos;

            }
        }

        public Evento GetEventoPorId(int IdEvento)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Eventos WHERE IdEvento = @IdEvento AND Borrado = 0";
                Evento evento = db.QueryFirstOrDefault<Evento>(query, new { IdEvento });

                if (evento == null)
                {
                    Console.WriteLine($"No se encontró ningún evento con ID: {IdEvento}");
                }
                else
                {
                    Console.WriteLine($"Evento encontrado - ID: {evento.IdEvento}, Nombre: {evento.NombreEvento}");
                }

                return evento;
            }
        }

        public int PostNuevoEvento(Evento evento)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            // Verificar si el IdEstadoEvento existe
                            string checkQuery = "SELECT COUNT(1) FROM EstadosEventos WHERE IdEstadoEvento = @IdEstadoEvento";
                            int count = db.ExecuteScalar<int>(checkQuery, new { IdEstadoEvento = evento.IdEstadoEvento }, transaction);

                            if (count == 0)
                            {
                                throw new Exception($"El IdEstadoEvento {evento.IdEstadoEvento} no existe en la tabla EstadosEventos");
                            }

                            // Si el IdEstadoEvento es válido, proceder con la inserción
                            string query = "INSERT INTO dbo.Eventos (NombreEvento, FechaEvento, CantidadPersonas, IdPersonaAgasajada, IdTipoEvento, Visible, Borrado, IdUsuario, IdEstadoEvento) " +
                                           "VALUES (@NombreEvento, @FechaEvento, @CantidadPersonas, @IdPersonaAgasajada, @IdTipoEvento, @Visible, @Borrado, @IdUsuario, @IdEstadoEvento);" +
                                           "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                            evento.IdEvento = db.QuerySingle<int>(query, evento, transaction);

                            transaction.Commit();
                            return evento.IdEvento;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Error inserting event: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostNuevoEvento: {ex.Message}");
                return 0;
            }
        }

        public bool CambiarEstadoEvento(int idEvento, int idEstado)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE Eventos SET IdEstadoEvento = " + idEstado.ToString() + "WHERE IdEvento = " + idEvento.ToString();
                    db.Execute(query);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        

        public bool PutNuevoEvento(int idEvento, Evento evento)
        {

            try
            {

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
 

        public bool DeleteEvento(int idEvento)
        {

            try
            {
                return true;
            }

            catch (Exception ex)
            {

                return false;

            }
        }
    }
}
