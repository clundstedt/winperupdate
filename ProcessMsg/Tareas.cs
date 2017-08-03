using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Tareas
    {
        public static int GetScriptsOk(int idVersion, int idCliente, int idAmbiente)
        {
            try
            {
                var tareas = ProcessMsg.Tareas.GetTareas(idCliente, idVersion).Where(x => x.Ambientes.idAmbientes == idAmbiente).ToList();
                if (tareas.Count == 0)
                {
                    return 2;
                }
                var scriptsPendientes = tareas.Where(x => x.Ambientes.idAmbientes == idAmbiente && x.Estado == 0).ToList();
                if (scriptsPendientes.Count > 0)
                {
                    return 1;
                }
                var scriptsEjecutados = tareas.Where(x => x.Ambientes.idAmbientes == idAmbiente && x.Estado == 1).ToList().Count;
                var scripts = ProcessMsg.Componente.GetComponentes(idVersion, null).Where(x => x.Tipo != '*').ToList().Count;
                if (scripts != scriptsEjecutados)
                {
                    return  3;
                }
                return 0;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
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
        public static bool Add(List<ProcessMsg.Model.TareaBo> tareas)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("0");
                dt.Columns.Add("1");
                dt.Columns.Add("2");
                dt.Columns.Add("3");
                dt.Columns.Add("4");
                dt.Columns.Add("5");
                dt.Columns.Add("6");
                dt.Columns.Add("7");
                dt.Columns.Add("8");
                foreach (var i in tareas)
                {
                    dt.Rows.Add(new object[] { i.idTareas, i.idClientes, i.Ambientes.idAmbientes, i.CodPrf, i.Estado, i.Modulo, i.idVersion, i.NameFile, i.Error });
                }
                return new AddTareas().Execute(dt);
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
