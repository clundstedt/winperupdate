using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Cliente
    {
        #region Class
        public class Trabs
        {
            public string idTrab { get; set; }
            public string Descripcion { get; set; }
        }
        #endregion

        #region Metodos GETs
        public static System.Data.DataTable GetVersionToClientePDF(int idCliente)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Release");
                dt.Columns.Add("Fecha de Instalación");
                dt.Columns.Add("Ambiente");
                var ins = Version.GetVersionesToCliente(idCliente);
                foreach (var i in ins)
                {
                    dt.Rows.Add(i.Version.Release,i.FechaInstalacionFmt,i.Ambiente.Nombre);
                }

                return dt;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static System.Data.DataTable GetModulosClientePDF(int idCliente)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Descripción");
                dt.Columns.Add("Estado");
                var mod = GetClientesHasModulo(idCliente);
                foreach (var m in mod)
                {
                    dt.Rows.Add(m.NomModulo,m.Descripcion,m.EstadoFmt);
                }

                return dt;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static System.Data.DataTable GetClientesPDF()
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Rut");
                dt.Columns.Add("Razón Social");
                dt.Columns.Add("Dirección");
                dt.Columns.Add("Comuna");
                dt.Columns.Add("Módulos");
                var clientes = GetClientes();
                foreach (var cl in clientes)
                {
                    var modulos = GetClientesHasModulo(cl.Id).Select(x => x.NomModulo);
                    dt.Rows.Add(cl.RutFmt
                                ,cl.Nombre
                                ,cl.Direccion
                                ,cl.Comuna.NomCmn
                                ,string.Join(" ",modulos.ToArray()));
                }

                return dt;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static List<Trabs> GetTrabPlantas()
        {
            try
            {
                List<Trabs> lista = new List<Trabs>();
                var read = new CnaClientes().GetTrabPlantas();
                while (read.Read())
                {
                    lista.Add(new Trabs
                    {
                        idTrab = read["idTrabPlantas"].ToString(),
                        Descripcion = read["Descripcion"].ToString()
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

        public static List<Trabs> GetTrabHonorarios()
        {
            try
            {
                List<Trabs> lista = new List<Trabs>();
                var read = new CnaClientes().GetTrabHonorarios();
                while (read.Read())
                {
                    lista.Add(new Trabs
                    {
                        idTrab = read["idTrabHonorarios"].ToString(),
                        Descripcion = read["Descripcion"].ToString()
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
        public static List<Model.VersionBo> GetVersiones(int idCliente, EventLog log)
        {
            var lista = new List<Model.VersionBo>();
            var consulta = new CnaVersionesCliente();
            try
            {
                var dr = consulta.Execute(idCliente);
                while (dr.Read())
                {
                    var obj = new Model.VersionBo
                    {
                        IdVersion = int.Parse(dr["idVersion"].ToString()),
                        Release = dr["NumVersion"].ToString(),
                        Fecha = DateTime.Parse(dr["FecVersion"].ToString()),
                        Estado = dr["Estado"].ToString()[0],
                        Comentario = dr["Comentario"].ToString(),
                        Usuario = dr["Usuario"].ToString(),
                        Instalador = dr["Instalador"].ToString(),
                        Componentes = new List<Model.AtributosArchivoBo>()
                    };

                    foreach (var modulo in Version.GetModulosVersiones(obj.IdVersion, null))
                    {
                        foreach (var componente in Componente.GetComponentes(obj.IdVersion, modulo, null))
                        {
                            obj.Componentes.Add(new Model.AtributosArchivoBo
                            {
                                Name = componente.Name,
                                DateCreate = componente.DateCreate,
                                Version = componente.Version,
                                Modulo = componente.Modulo
                            });
                        }
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

        public static List<Model.VersionBo> GetVersionesAmbiente(int idCliente, int idAmbiente, EventLog log)
        {
            var lista = new List<Model.VersionBo>();
            var consulta = new CnaVersionesCliente();
            try
            {
                var dr = consulta.Execute(idCliente, idAmbiente);
                while (dr.Read())
                {
                    var obj = new Model.VersionBo
                    {
                        IdVersion = int.Parse(dr["idVersion"].ToString()),
                        Release = dr["NumVersion"].ToString(),
                        Fecha = DateTime.Parse(dr["FecVersion"].ToString()),
                        Estado = dr["Estado"].ToString()[0],
                        Comentario = dr["Comentario"].ToString(),
                        Usuario = dr["Usuario"].ToString(),
                        Instalador = dr["Instalador"].ToString(),
                        Componentes = new List<Model.AtributosArchivoBo>()
                    };

                    foreach (var modulo in Version.GetModulosVersiones(obj.IdVersion, null))
                    {
                        foreach (var componente in Componente.GetComponentes(obj.IdVersion, modulo, null))
                        {
                            obj.Componentes.Add(new Model.AtributosArchivoBo
                            {
                                Name = componente.Name,
                                DateCreate = componente.DateCreate,
                                Version = componente.Version,
                                Modulo = componente.Modulo
                            });
                        }
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
                        Comuna = GetComunaById(int.Parse(dr["idCmn"].ToString())),
                        NumFolio = int.Parse(dr["Folio"].ToString()),
                        EstMtc = int.Parse(dr["EstMtc"].ToString()),
                        Mesini = dr["MesIni"].ToString(),
                        NroTrbc = dr["NroTrbc"].ToString(),
                        NroTrbh = dr["NroTrbh"].ToString(),
                        NroUsr = dr["NroUsr"].ToString()
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

        public static Model.ClienteBo GetClienteByLicencia(string nroLicencia, EventLog log)
        {
            var lista = new List<Model.ClienteBo>();
            var consulta = new CnaClienteByNroLicencia();
            try
            {
                var obj = new Model.ClienteBo();
                bool existe = false;

                var dr = consulta.Execute(nroLicencia);
                while (dr.Read())
                {
                    obj = new Model.ClienteBo
                    {
                        Id = int.Parse(dr["IdClientes"].ToString()),
                        Rut = int.Parse(dr["Rut"].ToString()),
                        Dv = dr["Dv"].ToString()[0],
                        Nombre = dr["RazonSocial"].ToString(),
                        Direccion = dr["Direccion"].ToString(),
                        NroLicencia = dr["NroLicencia"].ToString(),
                        Comuna = GetComunaById(int.Parse(dr["idCmn"].ToString()))
                    };
                    existe = true;
                }
                dr.Close();

                return existe ? obj : null;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                if (log != null) log.WriteEntry(msg, EventLogEntryType.Error);
                throw new Exception(msg, ex);
            }

        }

        public static Model.ClienteBo GetClienteUsuario(int idUsuario)
        {
            var lista = new List<Model.ClienteBo>();
            var consulta = new CnaClienteUsuario();
            try
            {
                var obj = new Model.ClienteBo();
                bool existe = false;

                var dr = consulta.Execute(idUsuario);
                while (dr.Read())
                {
                    obj = new Model.ClienteBo
                    {
                        Id = int.Parse(dr["IdClientes"].ToString()),
                        Rut = int.Parse(dr["Rut"].ToString()),
                        Dv = dr["Dv"].ToString()[0],
                        Nombre = dr["RazonSocial"].ToString(),
                        Direccion = dr["Direccion"].ToString(),
                        NroLicencia = dr["NroLicencia"].ToString(),
                        Comuna = GetComunaById(int.Parse(dr["idCmn"].ToString()))
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

        public static List<Model.ClienteBo> GetClientesVersion(int idVersion)
        {
            List<Model.ClienteBo> lista = new List<Model.ClienteBo>();

            try
            {
                var dr = new CnaClientes().Execute(idVersion);
                while (dr.Read())
                {
                    lista.Add(new Model.ClienteBo
                    {
                        Id = int.Parse(dr["IdClientes"].ToString()),
                        Rut = int.Parse(dr["Rut"].ToString()),
                        Dv = dr["Dv"].ToString()[0],
                        Nombre = dr["RazonSocial"].ToString(),
                        Direccion = dr["Direccion"].ToString(),
                        NroLicencia = dr["NroLicencia"].ToString(),
                        Comuna = GetComunaById(int.Parse(dr["idCmn"].ToString()))
                    });
                }
                dr.Close();

                return lista;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static int GetFolioLicencia()
        {
            try
            {
                int numFolio = 0;
                var dr = new CnaClientes().GetFolio("Licencia");
                while (dr.Read())
                {
                    if (int.TryParse(dr[0].ToString(), out numFolio))
                    {
                        return numFolio + 1;
                    }
                }
                return 1000;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        
        public static List<Model.UsuarioBo> GetUsuarios(int idCliente)
        {
            var lista = new List<Model.UsuarioBo>();
            var consulta = new CnaUsuariosCliente();
            try
            {
                var dr = consulta.Execute(idCliente);
                while (dr.Read())
                {
                    var obj = new Model.UsuarioBo
                    {
                        Id = int.Parse(dr["IdUsuarios"].ToString()),
                        CodPrf = int.Parse(dr["CodPrf"].ToString()),
                        EstUsr = dr["EstUsr"].ToString()[0],
                        Persona = new Model.PersonaBo
                        {
                            Id = int.Parse(dr["idPersonas"].ToString()),
                            Apellidos = dr["Apellidos"].ToString(),
                            Nombres = dr["Nombres"].ToString(),
                            Mail = dr["Mail"].ToString()
                        }
                    };

                    lista.Add(obj);
                }
                dr.Close();

                return lista;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static List<Model.ModuloBo> GetClientesHasModulo(int idCliente)
        {
            List<Model.ModuloBo> lista = new List<Model.ModuloBo>();
            try
            {
                var reader = new CnaClientes().ExecuteClientesHasModulos(idCliente);
                while (reader.Read())
                {
                    lista.Add(new Model.ModuloBo
                    {
                        idModulo = int.Parse(reader["idModulo"].ToString()),
                        NomModulo = reader["NomModulo"].ToString()
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
        public static Model.ClienteBo Add(Model.ClienteBo cliente)
        {
            int codError = 0;
            string msgError = "";
            var query = new AddCliente();
            try
            {
                var dr = query.Execute(cliente.Rut, cliente.Dv, cliente.Nombre, cliente.Direccion, cliente.Comuna.idCmn
                    , cliente.NroLicencia, cliente.NumFolio, cliente.EstMtc,cliente.Mesini,cliente.NroTrbc,cliente.NroTrbh,cliente.NroUsr);
                while (dr.Read())
                {
                    codError = int.Parse(dr["codErr"].ToString());
                    msgError = dr["msgErr"].ToString();
                    if (codError == 0)
                    {
                        return GetClientes().SingleOrDefault(x => x.Rut == cliente.Rut);
                    }else
                    {
                        var msg = "Excepcion Controlada: " + msgError;
                        throw new Exception(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return null;
        }
        public static bool AddClientesHasModulos(int idCliente, int[] idModulos)
        {
            try
            {
                return new AddCliente().ExecuteClientesHasModulos(idCliente, idModulos);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static int AddUsuario(int idCliente, int idUsuario)
        {
            var query = new AddUsuarioCliente();
            try
            {
                return query.Execute(idUsuario, idCliente);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        #endregion

        #region Upds
        public static Model.ClienteBo Update(int id, Model.ClienteBo cliente)
        {
            var query = new UpdCliente();
            try
            {
                if (query.Execute(id, cliente.Rut, cliente.Dv, cliente.Nombre, cliente.Direccion, cliente.Comuna.idCmn
                                 ,cliente.NroLicencia, cliente.EstMtc, cliente.Mesini, cliente.NroTrbc, cliente.NroTrbh, cliente.NroUsr) > 0)
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
        #endregion

        #region Dels
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
        #endregion




    }
}
