using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class ComponenteModulo
    {
        #region Gets
        public static Model.ComponenteModuloBo GetComponenteModuloByName(string nombre)
        {
            try
            {
                Model.ComponenteModuloBo comp = null;
                var reader = new CnaComponenteModulo().ExecuteComponentesModulos(nombre);
                while (reader.Read())
                {
                    comp = new Model.ComponenteModuloBo
                    {
                        idComponentesModulos = int.Parse(reader["idComponentesModulos"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Modulo = int.Parse(reader["Modulos"].ToString())
                    };
                }
                return comp;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static bool ExisteComponentesModulos(string nombre)
        {
            try
            {
                var reader = new CnaComponenteModulo().ExecuteComponentesModulos(nombre);
                return reader.Read();
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static List<Model.ComponenteModuloBo> GetComponentesModulos(int idModulo)
        {
            try
            {
                List<Model.ComponenteModuloBo> lista = new List<Model.ComponenteModuloBo>();
                var reader = new CnaComponenteModulo().ExecuteComponentesModulos(idModulo);
                while (reader.Read())
                {
                    lista.Add(new Model.ComponenteModuloBo
                    {
                        idComponentesModulos = int.Parse(reader["idComponentesModulos"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Modulo = int.Parse(reader["Modulos"].ToString()),
                        TipoComponentes = new Model.TipoComponenteBo
                        {
                            idTipoComponentes = int.Parse(reader["TipoComponentes"].ToString()),
                            Nombre = reader["NombreTipo"].ToString(),
                            isCompBD = bool.Parse(reader["isCompBD"].ToString()),
                            isCompDLL = bool.Parse(reader["isCompDLL"].ToString()),
                            Extensiones = reader["Extensiones"].ToString()
                        }
                    });
                }
                return lista;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static List<Model.TipoComponenteBo> GetTipoComponentes()
        {
            try
            {
                List<Model.TipoComponenteBo> lista = new List<Model.TipoComponenteBo>();
                var reader = new CnaComponenteModulo().ExecuteTipoComponente();
                while (reader.Read())
                {
                    lista.Add(new Model.TipoComponenteBo
                    {
                        idTipoComponentes = int.Parse(reader["idTipoComponentes"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        isCompBD = bool.Parse(reader["isCompBD"].ToString()),
                        isCompDLL = bool.Parse(reader["isCompDLL"].ToString()),
                        Extensiones = reader["Extensiones"].ToString()
                    });
                }
                return lista;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Adds

        public static int AddComponentesModulos(string Nombre, string Descripcion, int Modulos, int TipoComponentes)
        {
            try
            {
                return new AddComponenteModulo().ExecuteComponenteModulo(Nombre, Descripcion, Modulos, TipoComponentes);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static object[] AddComponentesModulos(List<ProcessMsg.Model.ComponenteModuloBo> componentes)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Descripcion");
                dt.Columns.Add("Modulos");
                dt.Columns.Add("TipoComponentes");
                componentes.ForEach(x =>
                {
                    dt.Rows.Add(x.Nombre, x.Descripcion, x.Modulo, x.TipoComponentes.idTipoComponentes);
                });
                object[] respuesta = new object[2];
                var reader = new AddComponenteModulo().ExecuteComponenteModulo(dt);
                while (reader.Read())
                {
                    respuesta[0] = reader["coderr"].ToString();
                    respuesta[1] = reader["msgerror"].ToString();
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static int AddTipoComponentes(string Nombre, bool isCompBD, bool isCompDLL, string Extensiones)
        {
            try
            {
                return new AddComponenteModulo().ExecuteTipoComponentes(Nombre, isCompBD, isCompDLL, Extensiones);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Upds

        public static int UpdComponentesModulos(int idComponentesModulos, string Nombre, string Descripcion, int TipoComponentes)
        {
            try
            {
                return new UpdComponenteModulo().Execute(idComponentesModulos, Nombre, Descripcion, TipoComponentes);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        #endregion

        #region Dels

        public static int DelComponentesModulos(int idComponentesModulos)
        {
            try
            {
                return new DelComponenteModulo().ExecuteComponenteModulo(idComponentesModulos);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static int DelTipoComponentes(int idTipoComponentes)
        {
            try
            {
                return new DelComponenteModulo().ExecuteTipoComponentes(idTipoComponentes);
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        #endregion
    }
}
