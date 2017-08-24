using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Ambiente
    {
        public static List<ProcessMsg.Model.AmbienteBo> GetAmbientesNoEx(int idCliente, int idVersion, string NameFile)
        {
            try
            {
                List<ProcessMsg.Model.AmbienteBo> lista = new List<Model.AmbienteBo>();
                var reader = new CnaAmbientes().ExecuteAmbientesNoEx(idCliente, idVersion, NameFile);
                while (reader.Read())
                {
                    lista.Add(new ProcessMsg.Model.AmbienteBo
                    {
                        idAmbientes = int.Parse(reader["idAmbientes"].ToString()),
                        idClientes = int.Parse(reader["idClientes"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        Tipo = int.Parse(reader["Tipo"].ToString()),
                        ServerBd = reader["ServerBd"].ToString(),
                        Instancia = reader["Instancia"].ToString(),
                        NomBd = reader["NomBd"].ToString(),
                        UserDbo = reader["UserDbo"].ToString(),
                        PwdDbo = Utils.DesEncriptar(reader["PwdDbo"].ToString()),
                        EjecutadoOK = bool.Parse(reader["EjecutadoOK"].ToString())
                    });
                }
                reader.Close();
                return lista;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static bool AmbienteOK(int idVersion, int idAmbiente)
        {
            try
            {
                bool ok = false;
                var reader = new CnaAmbientes().ExecuteAmbienteOK(idVersion, idAmbiente);
                while (reader.Read())
                {
                    ok = bool.Parse(reader["OK"].ToString());
                }

                reader.Close();
                return ok;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static List<Model.AmbienteBo> GetAmbientesByCliente(int idCliente, EventLog log)
        {
            List<Model.AmbienteBo> lista = new List<Model.AmbienteBo>();
            try
            {
                var dr = new CnaAmbientes().Execute(idCliente);
                while (dr.Read())
                {
                    lista.Add(new Model.AmbienteBo
                    {
                        idAmbientes = int.Parse(dr["idAmbientes"].ToString()),
                        idClientes = idCliente,
                        Nombre = dr["Nombre"].ToString(),
                        Tipo = int.Parse(dr["Tipo"].ToString()),
                        ServerBd = dr["ServerBd"].ToString(),
                        Instancia = dr["Instancia"].ToString(),
                        NomBd = dr["NomBd"].ToString(),
                        UserDbo = dr["UserDbo"].ToString(),
                        PwdDbo = Utils.DesEncriptar(dr["PwdDbo"].ToString()),
                        Estado = dr["Estado"] == DBNull.Value ? ' ' : dr["Estado"].ToString()[0]
                    });
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                if (log != null) log.WriteEntry(msg, EventLogEntryType.Error);
                throw new Exception(msg, ex);
            }
            return lista;
        }
        public static List<Model.AmbienteBo> GetAmbientesByCliente(int idCliente, int idVersion, EventLog log)
        {
            List<Model.AmbienteBo> lista = new List<Model.AmbienteBo>();
            try
            {
                var dr = new CnaAmbientes().Execute(idCliente, idVersion);
                while (dr.Read())
                {
                    lista.Add(new Model.AmbienteBo
                    {
                        idAmbientes = int.Parse(dr["idAmbientes"].ToString()),
                        idClientes = idCliente,
                        Nombre = dr["Nombre"].ToString(),
                        Tipo = int.Parse(dr["Tipo"].ToString()),
                        ServerBd = dr["ServerBd"].ToString(),
                        Instancia = dr["Instancia"].ToString(),
                        NomBd = dr["NomBd"].ToString(),
                        UserDbo = dr["UserDbo"].ToString(),
                        PwdDbo = Utils.DesEncriptar(dr["PwdDbo"].ToString()),
                        Estado = dr["Estado"] == DBNull.Value ? ' ' : dr["Estado"].ToString()[0],
                        EstadoEjecucionSql = Tareas.GetScriptsOk(idVersion, idCliente, int.Parse(dr["idAmbientes"].ToString())) 
                    });
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                if (log != null) log.WriteEntry(msg, EventLogEntryType.Error);
                throw new Exception(msg, ex);
            }
            return lista;
        }
        public static List<Model.AmbienteBo> GetAmbientesByClienteEx(int idCliente, int idVersion, EventLog log)
        {
            List<Model.AmbienteBo> lista = new List<Model.AmbienteBo>();
            try
            {
                var dr = new CnaAmbientes().ExecuteEx(idCliente, idVersion);
                while (dr.Read())
                {
                    lista.Add(new Model.AmbienteBo
                    {
                        idAmbientes = int.Parse(dr["idAmbientes"].ToString()),
                        idClientes = idCliente,
                        Nombre = dr["Nombre"].ToString(),
                        Tipo = int.Parse(dr["Tipo"].ToString()),
                        ServerBd = dr["ServerBd"].ToString(),
                        Instancia = dr["Instancia"].ToString(),
                        NomBd = dr["NomBd"].ToString(),
                        UserDbo = dr["UserDbo"].ToString(),
                        PwdDbo = Utils.DesEncriptar(dr["PwdDbo"].ToString()),
                        Estado = dr["Estado"] == DBNull.Value ? ' ' : dr["Estado"].ToString()[0]
                    });
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                if (log != null) log.WriteEntry(msg, EventLogEntryType.Error);
                throw new Exception(msg, ex);
            }
            return lista;
        }
        public static Model.AmbienteBo GetAmbiente(int idAmbiente, int idCliente)
        {
            try
            {
                return GetAmbientesByCliente(idCliente, null).SingleOrDefault(x => x.idAmbientes == idAmbiente);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static bool AddAmbientesXLSX(int idCliente, string archivo, int hoja)
        {
            try
            {
                var tabla = VerificarDatosAmbientes(idCliente, archivo, hoja);
                return new AddAmbiente().ExecuteAmbientesXLSX(tabla);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        /// <summary>
        /// Verifica los AmbienteXLSX estan todos sin errores
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns>Cantidad de AmbientesXLSX sin errores</returns>
        public static int GetAmbXlSXOk(int idCliente)
        {
            try
            {
                int count = 0;
                var reader = new CnaAmbientes().ExecuteAmbXLSXOk(idCliente);
                while (reader.Read())
                {
                    count = int.Parse(reader["count"].ToString());
                }
                reader.Close();
                return count;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int AddAmbXLSXtoAmb(int idCliente)
        {
            try
            {
                return new AddAmbiente().ExecuteAmbXLSXtoAmb(idCliente);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static List<ProcessMsg.Model.AmbientesXLSXBo> GetAmbientesXLSX(int idCliente)
        {
            List<ProcessMsg.Model.AmbientesXLSXBo> lista = new List<Model.AmbientesXLSXBo>();
            try
            {
                var reader = new CnaAmbientes().ExecuteAmbientesXLSX(idCliente);
                while (reader.Read())
                {
                    lista.Add(new Model.AmbientesXLSXBo
                    {
                        idAmbientes = int.Parse(reader["idAmbientes"].ToString()),
                        idClientes = int.Parse(reader["idClientes"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        Tipo = int.Parse(reader["Tipo"].ToString()),
                        ServerBd = reader["ServerBd"].ToString(),
                        Instancia = reader["Instancia"].ToString(),
                        NomBd = reader["NomBd"].ToString(),
                        UserDbo = reader["UserDbo"].ToString(),
                        PwdDbo = Utils.DesEncriptar(reader["PwdDbo"].ToString()),
                        FechaRegistroEx = Convert.ToDateTime(reader["FechaRegistro"].ToString()),
                        EstadoRegistro = int.Parse(reader["EstadoRegistro"].ToString()),
                        ErrorRegistro = reader["ErrorRegistro"].ToString()
                    });
                }
                reader.Close();
                return lista;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        private static System.Data.DataTable VerificarDatosAmbientes(int idClientes, string Archivo, int Hoja)
        {
            System.Data.DataTable dt = new CnaAmbientes().selectExcel(Archivo, Hoja);
            System.Data.DataTable dtAmbientes = new System.Data.DataTable();
            dtAmbientes.Columns.Add("idClientes");
            dtAmbientes.Columns.Add("Nombre");
            dtAmbientes.Columns.Add("Tipo");
            dtAmbientes.Columns.Add("ServerBd");
            dtAmbientes.Columns.Add("Instancia");
            dtAmbientes.Columns.Add("NomBd");
            dtAmbientes.Columns.Add("UserDbo");
            dtAmbientes.Columns.Add("PwdDbo");
            dtAmbientes.Columns.Add("FechaRegistro");
            dtAmbientes.Columns.Add("EstadoRegistro");
            dtAmbientes.Columns.Add("ErrorRegistro");
            string ErrorRegistro = "";
            int tipo;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProcessMsg.Model.AmbientesXLSXBo ambiente;
                tipo = 0;
                ErrorRegistro = "";
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()) || dt.Rows[i][0].ToString().Length > 45)
                {
                    ErrorRegistro += "Error en la columna 'Nombre', está vacía o sobrepasa el límite de caracteres (45) || ";
                }
                if (!int.TryParse(dt.Rows[i][1].ToString(), out tipo))
                {
                    ErrorRegistro += "Error en la columna 'Tipo', debe ser numerica || ";
                }else if (!(tipo >= 1 && tipo <= 2))
                {
                    ErrorRegistro += "Error en la columna 'Tipo', debe ser 1 (Producción) o 2 (Pruebas) || ";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][2].ToString()) || dt.Rows[i][2].ToString().Length > 99)
                {
                    ErrorRegistro += "Error en la columna 'ServerBd', está vacía o sobrepasa el límite de caracteres (100) || ";
                }
                if (dt.Rows[i][3] != null)
                {
                    if (dt.Rows[i][3].ToString().Length > 99)
                    {
                        ErrorRegistro += "Error en la columna 'Instancia', sobrepasa el límite de caracteres (100) || ";
                    }
                }
                if (string.IsNullOrEmpty(dt.Rows[i][4].ToString()) || dt.Rows[i][4].ToString().Length > 99)
                {
                    ErrorRegistro += "Error en la columna 'NomBd', está vacía o sobrepasa el límite de caracteres (100) || ";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][5].ToString()) || dt.Rows[i][5].ToString().Length > 49)
                {
                    ErrorRegistro += "Error en la columna 'UserDbo', está vacía o sobrepasa el límite de caracteres (50) || ";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][6].ToString()) || dt.Rows[i][6].ToString().Length > 49)
                {
                    ErrorRegistro += "Error en la columna 'PwdDbo', está vacía o sobrepasa el límite de caracteres (50) || ";
                }
                
                ambiente = new Model.AmbientesXLSXBo
                {
                    idClientes = idClientes,
                    Nombre = dt.Rows[i][0].ToString(),
                    Tipo = (int.TryParse(dt.Rows[i][1].ToString(), out tipo) ? tipo : (int?)null),
                    ServerBd = dt.Rows[i][2].ToString(),
                    Instancia = dt.Rows[i][3] != null ? dt.Rows[i][3].ToString() : "",
                    NomBd = dt.Rows[i][4].ToString(),
                    UserDbo = dt.Rows[i][5].ToString(),
                    PwdDbo = Utils.Encriptar(dt.Rows[i][6].ToString()),
                    ErrorRegistro = ErrorRegistro,
                    EstadoRegistro = (string.IsNullOrEmpty(ErrorRegistro) ? 0 : 1)
                };
                dtAmbientes.Rows.Add(ambiente.idClientes,ambiente.Nombre, ambiente.Tipo,ambiente.ServerBd,ambiente.Instancia
                    ,ambiente.NomBd,ambiente.UserDbo, ambiente.PwdDbo,ambiente.FechaRegistro, ambiente.EstadoRegistro, ambiente.ErrorRegistro);

            }
            return dtAmbientes;
        }

        public static Model.AmbienteBo Add(int idCliente, Model.AmbienteBo ambiente)
        {
            try
            {
                if (new AddAmbiente().Execute(idCliente,ambiente.Nombre,ambiente.Tipo,ambiente.ServerBd,ambiente.Instancia,ambiente.NomBd,ambiente.UserDbo,Utils.Encriptar(ambiente.PwdDbo)) > 0)
                {
                    var list = GetAmbientesByCliente(idCliente, null).OrderBy(x => x.idAmbientes).ToList();
                    return list.ElementAt(list.Count - 1);
                }
                return null;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static Model.AmbienteBo Update(int idCliente, int idAmbiente, Model.AmbienteBo ambiente)
        {
            try
            {
                if (new UpdAmbiente().Execute(idAmbiente,idCliente,ambiente.Nombre,ambiente.Tipo,ambiente.ServerBd,ambiente.Instancia,ambiente.NomBd,ambiente.UserDbo,Utils.Encriptar(ambiente.PwdDbo)) > 0)
                {
                    return GetAmbiente(idAmbiente, idCliente);
                }
                return null;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int Delete(int idAmbiente, int idCliente)
        {
            try
            {
                return new DelAmbiente().Execute(idAmbiente, idCliente);
            }catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

    }
}
