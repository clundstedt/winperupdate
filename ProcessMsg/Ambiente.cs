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
                        Estado = dr["Estado"] == DBNull.Value ? ' ' : dr["Estado"].ToString()[0]
                    });
                }
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
