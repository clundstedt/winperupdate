using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Cliente
    {
        public static List<Model.RegionBo> GetRegiones()
        {
            var lista = new List<Model.RegionBo>();
            var consulta = new CnaRegiones();
            try
            {
                var dr = consulta.Execute();
                while (dr.Read())
                {
                    var obj = new Model.RegionBo
                    {
                        idRgn = int.Parse(dr["idRgn"].ToString()),
                        NomRgn = dr["NomRgn"].ToString()
                    };

                    lista.Add(obj);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return lista;
        }

        public static List<Model.ComunaBo> GetComunas(int idRgn)
        {
            var lista = new List<Model.ComunaBo>();
            var consulta = new CnaComuna();
            try
            {
                var dr = consulta.Execute(idRgn);
                while (dr.Read())
                {
                    var obj = new Model.ComunaBo
                    {
                        idCmn = int.Parse(dr["idCmn"].ToString()),
                        NomCmn = dr["NomCmn"].ToString()
                    };

                    lista.Add(obj);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return lista;
        }

        public static Model.ComunaBo GetComunaById(int idCmn)
        {
            var lista = new List<Model.ComunaBo>();
            var consulta = new CnaComuna();
            try
            {
                var obj = new Model.ComunaBo();
                var dr = consulta.Execute(0, idCmn);
                bool existe = false;
                while (dr.Read())
                {
                    obj = new Model.ComunaBo
                    {
                        idCmn = int.Parse(dr["idCmn"].ToString()),
                        NomCmn = dr["NomCmn"].ToString(),
                        Region = GetRegiones().SingleOrDefault(x => x.idRgn == int.Parse(dr["idRgn"].ToString()))
                    };

                    existe = true;
                }
                dr.Close();

                return existe ? obj : null;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static List<Model.ClienteBo> GetClientes()
        {
            var lista = new List<Model.ClienteBo>();
            var consulta = new CnaClientes();
            try
            {
                var dr = consulta.Execute();
                while (dr.Read())
                {
                    var obj = new Model.ClienteBo
                    {
                        Id = int.Parse(dr["IdClientes"].ToString()),
                        Rut = int.Parse(dr["Rut"].ToString()),
                        Dv = dr["Dv"].ToString()[0],
                        Nombre = dr["RazonSocial"].ToString(),
                        Direccion = dr["Direccion"].ToString(),
                        NroLicencia = dr["NroLicencia"].ToString(),
                        Comuna = GetComunaById(int.Parse(dr["idCmn"].ToString()))
                    };

                    lista.Add(obj);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return lista;
        }

        public static Model.ClienteBo Add(Model.ClienteBo cliente)
        {
            var query = new AddCliente();
            try
            {
                if (query.Execute(cliente.Rut, cliente.Dv, cliente.Nombre, cliente.Direccion, cliente.Comuna.idCmn) > 0)
                {
                    return GetClientes().SingleOrDefault(x => x.Rut == cliente.Rut);
                }

            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return null;
        }

        public static Model.ClienteBo Update(int id, Model.ClienteBo cliente)
        {
            var query = new UpdCliente();
            try
            {
                if (query.Execute(id, cliente.Rut, cliente.Dv, cliente.Nombre, cliente.Direccion, cliente.Comuna.idCmn) > 0)
                {
                    return GetClientes().SingleOrDefault(x => x.Id == id);
                }

            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return null;
        }

        public static int Delete(int id)
        {
            var query = new DelCliente();
            try
            {
                return query.Execute(id);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

    }
}
