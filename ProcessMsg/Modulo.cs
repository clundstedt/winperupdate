using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Modulo
    {
        #region Gets
        public static List<Model.ModuloBo> GetModulos(EventLog log)
        {
            var lista = new List<Model.ModuloBo>();
            var consulta = new CnaModulo();
            try
            {
                var dr = consulta.Execute();
                while (dr.Read())
                {
                    var obj = new Model.ModuloBo
                    {
                        idModulo = int.Parse(dr["idModulo"].ToString()),
                        NomModulo = dr["NomModulo"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        isCore = bool.Parse(dr["isCore"].ToString()),
                        Directorio = dr["Directorio"].ToString(),
                        Estado = char.Parse(dr["Estado"].ToString())
                    };

                    lista.Add(obj);
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

        public static List<Model.ModuloXLSXBo> GetModulosXlsx(int idUsuario)
        {
            try
            {
                List<Model.ModuloXLSXBo> lista = new List<Model.ModuloXLSXBo>();
                var reader = new CnaModulo().ExecuteXlsx(idUsuario);
                while (reader.Read())
                {
                    lista.Add(new Model.ModuloXLSXBo
                    {
                         idModulo = int.Parse(reader["idModulo"].ToString()),
                         NomModulo = reader["NomModulo"].ToString(),
                         Descripcion = reader["Descripcion"].ToString(),
                         isCore = bool.Parse(reader["isCore"].ToString()),
                         Directorio = reader["Directorio"].ToString(),
                         FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"].ToString()),
                         EstadoRegistro = int.Parse(reader["EstadoRegistro"].ToString()),
                         ErrorRegistro = reader["ErrorRegistro"].ToString(),
                         idUsuario = int.Parse(reader["Usuario"].ToString())
                    });
                }
                return lista;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static Model.ModuloBo GetModulo(int idModulo)
        {
            try
            {
                Model.ModuloBo modulo = null;
                var reader = new CnaModulo().Execute(idModulo);
                while (reader.Read())
                {
                    modulo = new Model.ModuloBo
                    {
                        idModulo = int.Parse(reader["idModulo"].ToString()),
                        Descripcion = reader["Descripcion"].ToString(),
                        Directorio = reader["Directorio"].ToString(),
                        NomModulo = reader["NomModulo"].ToString(),
                        isCore = bool.Parse(reader["isCore"].ToString()),
                        Estado = char.Parse(reader["Estado"].ToString())
                    };
                }
                return modulo;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static List<Model.ModuloBo> GetModulosWithComponenteByCliente(int idCliente)
        {
            try
            {
                var modulos = ProcessMsg.Cliente.GetClientesHasModulo(idCliente);
                foreach (var m in modulos)
                {
                    m.ComponentesModulo = ProcessMsg.ComponenteModulo.GetComponentesModulos(m.idModulo);
                }
                return modulos;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Adds
        public static ProcessMsg.Model.ModuloBo AddModulo(ProcessMsg.Model.ModuloBo modulo)
        {
            try
            {
                var lastInsert = new AddModulo().Execute(modulo.NomModulo, modulo.Descripcion, modulo.isCore, modulo.Directorio);
                return GetModulo(lastInsert);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message; 
                throw new Exception(msg, ex);
            }
        }
        public static int AddModulos(int idUsuario, string rutaExcel)
        {
            try
            {
                System.Data.DataTable dt = VerificarDatosModulo(idUsuario,rutaExcel, "Modulos");
                var obj = new AddModulo();
                if (obj.ExecuteTrans(dt))
                {
                    return obj.ExecuteFromXLSX(idUsuario);
                }
                return 0;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Upds
        public static int UpdModulo(ProcessMsg.Model.ModuloBo modulo)
        {
            try
            {
                return new UpdModulo().Execute(modulo.idModulo, modulo.NomModulo, modulo.Descripcion, modulo.isCore, modulo.Directorio);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static int SetVigente(int idModulo)
        {
            try
            {
                return new UpdModulo().ExecuteVigente(idModulo);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Dels
        public static int DelModulo(int idModulo)
        {
            try
            {
                return new DelModulo().Execute(idModulo);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Otros
        private static System.Data.DataTable VerificarDatosModulo(int idUsuario, string Archivo, string Hoja)
        {
            System.Data.DataTable dt = new CnaModulo().SelectExcel(Archivo, Hoja);
            System.Data.DataTable dtModulos = new System.Data.DataTable();
            dtModulos.Columns.Add("NomModulo");
            dtModulos.Columns.Add("Descripcion");
            dtModulos.Columns.Add("isCore");
            dtModulos.Columns.Add("Directorio");
            dtModulos.Columns.Add("EstadoRegistro");
            dtModulos.Columns.Add("ErrorRegistro");
            dtModulos.Columns.Add("idUsuario");
            string ErrorRegistro = "";
            bool sisp;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProcessMsg.Model.ModuloXLSXBo modulo;
                sisp = false;
                ErrorRegistro = "";
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()) || dt.Rows[i][0].ToString().Length > 99)
                {
                    ErrorRegistro += "Error en la columna 'Nombre del Módulo', está vacía o sobre pasa el límite de caracteres (100) || ";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][1].ToString()) || dt.Rows[i][1].ToString().Length > 99)
                {
                    ErrorRegistro += "Error en la columna 'Descripción', está vacía o sobre pasa el límite de caracteres (100) || ";
                }
                if (!bool.TryParse(dt.Rows[i][2].ToString(), out sisp))
                {
                    ErrorRegistro += "Error en la columna 'Sistema Principal', verifique que sea 'True', 'False', 0 o 1 || ";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][3].ToString()) || dt.Rows[i][3].ToString().Length > 99)
                {
                    ErrorRegistro += "Error en la columna 'Directorio', está vacía o sobre pasa el límite de caracteres (100) || ";
                }

                modulo = new Model.ModuloXLSXBo
                {
                    NomModulo = dt.Rows[i][0].ToString(),
                    Descripcion = dt.Rows[i][1].ToString(),
                    isCore = sisp,
                    Directorio = dt.Rows[i][3].ToString(),
                    ErrorRegistro = ErrorRegistro,
                    EstadoRegistro = (string.IsNullOrEmpty(ErrorRegistro) ? 0 : 1),
                    idUsuario = idUsuario
                };
                dtModulos.Rows.Add(modulo.NomModulo, modulo.Descripcion, modulo.isCore, modulo.Directorio
                    , modulo.EstadoRegistro, modulo.ErrorRegistro, modulo.idUsuario);

            }
            return dtModulos;
        } 
        #endregion
    }
}
