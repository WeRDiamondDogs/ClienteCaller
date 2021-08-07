using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CallerPR.Common;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace CallerPR.Models
{
    public class ClienteModel
    {
        private static readonly ClienteModel _instacia = new ClienteModel();

        public static ClienteModel Instancia
        {
            get { return ClienteModel._instacia; }
        }

        public class DataCliente
        {
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public string FechaNacimiento { get; set; }
        }


        public DataTable _listarClientes()
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlConnection connection = new MySqlConnection(Configuracion.configuration["ConnectionStrings:DBCaller"]);
                MySqlDataAdapter da = new MySqlDataAdapter("sp_listarClientes", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                connection.Close();
                return dt;
            }
            catch (MySqlException ex) {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable _crearCliente(string nombres, string apellidos, string fechaNacimiento)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlConnection connection = new MySqlConnection(Configuracion.configuration["ConnectionStrings:DBCaller"]);
                MySqlDataAdapter da = new MySqlDataAdapter("sp_crearCliente", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add("p_nombre", MySqlDbType.String).Value = nombres;
                da.SelectCommand.Parameters.Add("p_apellido", MySqlDbType.String).Value = apellidos;
                da.SelectCommand.Parameters.Add("p_fechanacimiento", MySqlDbType.Date).Value = fechaNacimiento;
                da.Fill(dt);
                connection.Close();
                return dt;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable _obtenerClientePorID(int ID)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlConnection connection = new MySqlConnection(Configuracion.configuration["ConnectionStrings:DBCaller"]);
                MySqlDataAdapter da = new MySqlDataAdapter("sp_obtenerClientePorID", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add("p_id", MySqlDbType.Int32).Value = ID;
                da.Fill(dt);
                connection.Close();
                return dt;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
