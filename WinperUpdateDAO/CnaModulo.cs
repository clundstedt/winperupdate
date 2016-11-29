using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO
{
    public class CnaModulo : SpDao
    {
        public new SqlDataReader Execute()
        {
            SpName = @" select * from Modulos";
            try
            {
                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
        public SqlDataReader ExecuteXlsx(int idUsuario)
        {
            SpName = @"SELECT * FROM modulosxlsx WHERE Usuario = @idUsuario";
            try
            {
                ParmsDictionary.Add("@idUsuario",idUsuario);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "ExecuteXlsx", ex.Message);
                throw new Exception(msg, ex);
            }
        }
        public DataTable selectExcel(string Arch, string Hoja)
        {

            OleDbConnection Conex = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Arch + ";Extended Properties=Excel 12.0;");

            OleDbCommand CmdOle = new OleDbCommand();

            CmdOle.Connection = Conex;
            CmdOle.CommandType = CommandType.Text;
            CmdOle.CommandText = "SELECT * FROM [" + Hoja + "$]";

            OleDbDataAdapter AdaptadorOle = new OleDbDataAdapter(CmdOle.CommandText, Conex);

            DataTable dt = new DataTable();

            AdaptadorOle.Fill(dt);

            return dt;
        }
        public SqlDataReader Execute(int idModulo)
        {
            SpName = @"SELECT * FROM modulos WHERE idModulo = @idModulo";
            try
            {
                ParmsDictionary.Add("@idModulo", idModulo);

                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch(Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

    }
}
