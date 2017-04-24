using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Tareas
    {
    
        public static List<ProcessMsg.Model.TareaBo> GetTareas(int idClientes, int idVersion)
        {
            List<ProcessMsg.Model.TareaBo> lista = new List<ProcessMsg.Model.TareaBo>();
            try
            {
                var reader = new CnaTareas().ExecuteTareas(idClientes, idVersion);
                while (reader.Read())
                {
                    lista.Add(new ProcessMsg.Model.TareaBo
                    {
                        idTareas = int.Parse(reader["idTareas"].ToString()),
                        idClientes = int.Parse(reader["CltTarea"].ToString()),
                        Ambientes = new Model.AmbienteBo
                        {
                            idAmbientes = int.Parse(reader["idAmbientes"].ToString()),
                            idClientes = int.Parse(reader["idClientes"].ToString()),
                            Nombre = reader["Nombre"].ToString(),
                            Tipo = int.Parse(reader["Tipo"].ToString()),
                            ServerBd = reader["ServerBd"].ToString(),
                            Instancia = reader["Instancia"].ToString(),
                            NomBd = reader["NomBd"].ToString(),
                            UserDbo = reader["UserDbo"].ToString(),
                            PwdDbo = ProcessMsg.Utils.DesEncriptar(reader["PwdDbo"].ToString())
                        },
                        CodPrf = int.Parse(reader["CodPrf"].ToString()),
                        Estado = int.Parse(reader["Estado"].ToString()),
                        Modulo = reader["Modulo"].ToString(),
                        idVersion = int.Parse(reader["idVersion"].ToString()),
                        NameFile = reader["NameFile"].ToString(),
                        Error = reader["Error"].ToString(),
                        FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"].ToString()),
                        Reportado = bool.Parse(reader["Reportado"].ToString())
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
        public static List<ProcessMsg.Model.TareaBo> GetTareasPendientes(int idClientes, int CodPrf) 
        {
            List<ProcessMsg.Model.TareaBo> lista = new List<ProcessMsg.Model.TareaBo>();
            try
            {
                var reader = new CnaTareas().Execute(idClientes, CodPrf);
                while (reader.Read())
                {
                    lista.Add(new ProcessMsg.Model.TareaBo
                    {
                        idTareas = int.Parse(reader["idTareas"].ToString()),
                        idClientes = int.Parse(reader["CltTarea"].ToString()),
                        Ambientes = new Model.AmbienteBo
                        {
                            idAmbientes = int.Parse(reader["idAmbientes"].ToString()),
                            idClientes = int.Parse(reader["idClientes"].ToString()),
                            Nombre = reader["Nombre"].ToString(),
                            Tipo = int.Parse(reader["Tipo"].ToString()),
                            ServerBd = reader["ServerBd"].ToString(),
                            Instancia = reader["Instancia"].ToString(),
                            NomBd = reader["NomBd"].ToString(),
                            UserDbo = reader["UserDbo"].ToString(),
                            PwdDbo = ProcessMsg.Utils.DesEncriptar(reader["PwdDbo"].ToString())
                        },
                        CodPrf = int.Parse(reader["CodPrf"].ToString()),
                        Estado = int.Parse(reader["Estado"].ToString()),
                        Modulo = reader["Modulo"].ToString(),
                        idVersion = int.Parse(reader["idVersion"].ToString()),
                        NameFile = reader["NameFile"].ToString(),
                        Error=reader["Error"].ToString(),
                        FechaRegistro=Convert.ToDateTime(reader["FechaRegistro"].ToString()),
                        Reportado = bool.Parse(reader["Reportado"].ToString())
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

        public static bool ExisteTarea(int idCliente, int idAmbiente, int idVersion, string nameFile)
        {
            try
            {
                return new CnaTareas().Execute(idCliente, idAmbiente, idVersion, nameFile).Read();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int Add(ProcessMsg.Model.TareaBo tarea)
        {
            try
            {
                return new AddTareas().Execute(tarea.idTareas,tarea.idClientes,tarea.Ambientes.idAmbientes
                                              ,tarea.CodPrf,tarea.Estado,tarea.Modulo,tarea.idVersion
                                              ,tarea.NameFile, tarea.Error);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int SetEstadoTarea(int idTareas)
        {
            try
            {
                return new UpdTareas().Execute(idTareas);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int SetEstadoTarea(int idTareas, int estado, string msgErr)
        {
            try
            {
                return new UpdTareas().Execute(idTareas, estado, msgErr);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int ReportarTarea(int idTareas)
        {
            try
            {
                return new UpdTareas().ExecuteTareaReportada(idTareas);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int ReportarTodasTareas(int idCliente, int idVersion)
        {
            try
            {
                return new UpdTareas().ExecuteTodasTareas(idCliente, idVersion);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
    }
}
