
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
    public interface IServicioService
    {
        bool AgregarNuevoServicio(Servicio servicio);
        bool BorrarFisicamenteServicio(int idServicio);
        bool BorrarLogicamenteServicio(int idServicio);
        IEnumerable<Servicio> GetServicios();
        Servicio GetServiciosPorId(int IdServicio);
        bool ModificarServicio(int idServicio, Servicio servicio);
    }

    public class ServicioService : IServicioService
    {

        private string _connectionString;

        public ServicioService()
        {

            _connectionString = "Data Source=DESKTOP-GN0G7SH;Initial Catalog=gestioneventos;Integrated Security=True;Encrypt=False";


        }

        public IEnumerable<Servicio> GetServicios()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<Servicio> servicios = db.Query<Servicio>("SELECT * FROM Servicios WHERE Borrado = 0").ToList();

                return servicios;

            }
        }

        public Servicio GetServiciosPorId(int IdServicio)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Servicio servicio = db.Query<Servicio>("SELECT * FROM Servicios WHERE IdServicio = " + IdServicio.ToString()).FirstOrDefault();

                return servicio;
            }
        }

        public bool AgregarNuevoServicio(Servicio servicio)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Servicios (Descripcion, PrecioServicio, Borrado) VALUES ( @Descripcion, @PrecioServicio, 0)";
                db.Execute(query, servicio);
                return true;
            }
        }

        public bool ModificarServicio(int idServicio, Servicio servicio)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Servicios SET Descripcion = @Descripcion, PrecioServicio = @PrecioServicio  WHERE IdServicio = " + idServicio.ToString();
                db.Execute(query, servicio);

                return true;
            }

        }

        public bool BorrarLogicamenteServicio(int idServicio)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Servicios SET Borrado = 1 where IdServicio = " + idServicio.ToString();
                db.Execute(query);

                return true;
            }
        }

        public bool BorrarFisicamenteServicio(int idServicio)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM dbo.Servicios WHERE IdServicio = " + idServicio.ToString();
                db.Execute(query);

                return true;
            }
        }

    }
}
