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
                            isCompBD = bool.Parse(reader["isCompBD"].ToString())
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
                        isCompBD = bool.Parse(reader["isCompBD"].ToString())
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
        public static int AddTipoComponentes(string Nombre, bool isCompBD)
        {
            try
            {
                return new AddComponenteModulo().ExecuteTipoComponentes(Nombre, isCompBD);
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
