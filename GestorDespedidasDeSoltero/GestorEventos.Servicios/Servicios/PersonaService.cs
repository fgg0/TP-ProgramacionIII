using Dapper;
using GestorEventos.Servicios.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestorEventos.Servicios.Servicios
{
    public interface IPersonaService
    {
        int AgregarNuevaPersona(Persona persona);
        bool BorrarFisicamentePersona(int idPersona);
        bool BorrarLogicamentePersona(int idPersona);
        Persona? GetPersonaSegunId(int IdPersona);
        IEnumerable<Persona> GetPersonas();
        bool ModificarPersona(int idPersona, Persona persona);
    }

    public class PersonaService : IPersonaService
    {
        private string _connectionString;

        //constructor
        public PersonaService()
        {

            //Connection string 
            _connectionString = "Data Source=DESKTOP-GN0G7SH;Initial Catalog=gestioneventos;Integrated Security=True;Encrypt=False";



        }

        public IEnumerable<Persona> GetPersonas()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<Persona> personas = db.Query<Persona>("SELECT * FROM Personas WHERE Borrado = 0").ToList();

                return personas;

            }

        }

        public Persona? GetPersonaSegunId(int IdPersona)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Persona persona = db.Query<Persona>("SELECT * FROM Personas WHERE IdPersona = " + IdPersona.ToString()).FirstOrDefault();

                return persona;
            }

           

        }

        public int AgregarNuevaPersona(Persona persona)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Personas " +
                    "(Nombre, Apellido, Direccion, Telefono, Email)  " +
                    "VALUES " +
                    "( @Nombre, @Apellido, @Direccion, @Telefono, @Email); " +
                    "select  CAST(SCOPE_IDENTITY() AS INT) ";
                persona.IdPersona =  db.QuerySingle<int>(query, persona);


                return persona.IdPersona;
            }
        }

        public bool ModificarPersona(int idPersona, Persona persona)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Personas SET Nombre = @Nombre, Apellido = @Apellido, Direccion = @Direccion, Telefono = @Telefono, Email = @Email  WHERE IdPersona = " + idPersona.ToString();
                db.Execute(query, persona);

                return true;
            }

        }

        public bool BorrarLogicamentePersona(int idPersona)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Personas SET Borrado = 1 where IdPersona = " + idPersona.ToString();
                db.Execute(query);

                return true;
            }
        }

        public bool BorrarFisicamentePersona(int idPersona)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM dbo.Personas WHERE IdPersona = " + idPersona.ToString();
                db.Execute(query);

                return true;
            }
        }
    }
}
