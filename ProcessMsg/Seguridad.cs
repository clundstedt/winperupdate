using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Seguridad
    {
        public static Model.UsuarioBo GetUsuario(string mail)
        {
            var obj = new Model.UsuarioBo();
            var consulta = new CnaUsuario();
            try
            {
                var dr = consulta.Execute(mail);
                while (dr.Read())
                {
                    obj = new Model.UsuarioBo
                    {
                        Id = int.Parse(dr["IdUsuarios"].ToString()),
                        CodPrf = int.Parse(dr["CodPrf"].ToString()),
                        Clave = dr["Clave"].ToString(),
                        EstUsr = dr["EstUsr"].ToString()[0],
                        Persona = new Model.PersonaBo
                        {
                            Id = int.Parse(dr["idPersonas"].ToString()),
                            Apellidos = dr["Apellidos"].ToString(),
                            Nombres = dr["Nombres"].ToString(),
                            Mail = dr["Mail"].ToString()
                        }
                    };
                }
                dr.Close();

                return obj.Id == 0 ? null : obj;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

        }

        public static Model.UsuarioBo GetUsuario(int id)
        {
            var obj = new Model.UsuarioBo();
            var consulta = new CnaUsuarioById();
            try
            {
                var dr = consulta.Execute(id);
                while (dr.Read())
                {
                    obj = new Model.UsuarioBo
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

                    if (dr["idClientes"] != DBNull.Value)
                    {
                        obj.Cliente = Cliente.GetClientes().SingleOrDefault(x => x.Id == int.Parse(dr["idClientes"].ToString()));
                    }
                }
                dr.Close();

                return obj.Id == 0 ? null : obj;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

        }

        public static List<Model.UsuarioBo> GetUsuarios()
        {
            var lista = new List<Model.UsuarioBo>();
            var consulta = new CnaUsuario();
            try
            {
                var dr = consulta.Execute();
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

        public static List<Model.UsuarioBo> GetUsuariosCliente()
        {
            var lista = new List<Model.UsuarioBo>();
            var consulta = new CnaUsuario();
            try
            {
                var dr = consulta.Execute(10);
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

        public static List<Model.PersonaBo> GetPersonas()
        {
            var lista = new List<Model.PersonaBo>();
            var consulta = new CnaPersona();
            try
            {
                var dr = consulta.Execute();
                while (dr.Read())
                {
                    var obj = new Model.PersonaBo
                    {
                        Id = int.Parse(dr["idPersonas"].ToString()),
                        Apellidos = dr["Apellidos"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        Mail = dr["Mail"].ToString()
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

        public static Model.PersonaBo AddPersona(Model.PersonaBo persona)
        {
            var query = new AddPersona();
            try
            {
                if (query.Execute(persona.Nombres, persona.Apellidos, persona.Mail) > 0)
                {
                    return GetPersonas().Last();
                }

            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return null;

        }

        public static Model.PersonaBo UpdPersona(Model.PersonaBo persona)
        {
            var query = new UpdPersona();
            try
            {
                if (query.Execute(persona.Id, persona.Apellidos, persona.Nombres, persona.Mail) > 0)
                {
                    return GetPersonas().SingleOrDefault(x => x.Id == persona.Id);
                }

            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return null;

        }

        public static Model.UsuarioBo AddUsuario(Model.UsuarioBo usuario)
        {
            var query = new AddUsuario();
            try
            {
                if (query.Execute(usuario.Persona.Id, usuario.CodPrf, usuario.Clave, usuario.EstUsr) > 0)
                {
                    var list = GetUsuarios().OrderBy(x => x.Id).ToList();
                    return list.ElementAt(list.Count - 1); 
                }

            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return null;

        }

        public static Model.UsuarioBo AddUsuarioCliente(Model.UsuarioBo usuario)
        {
            var query = new AddUsuario();
            try
            {
                if (query.Execute(usuario.Persona.Id, usuario.CodPrf, usuario.Clave, usuario.EstUsr) > 0)
                {
                    var list = GetUsuariosCliente().OrderBy(x => x.Id).ToList();
                    return list.ElementAt(list.Count - 1);
                }

            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return null;

        }

        public static Model.UsuarioBo UpdUsuario(Model.UsuarioBo usuario)
        {
            var query = new UpdUsuario();
            try
            {
                if (query.Execute(usuario.Id, usuario.Persona.Id, usuario.CodPrf, usuario.EstUsr) > 0)
                {
                    return GetUsuarios().SingleOrDefault(x => x.Id == usuario.Id);
                }

            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

            return null;

        }

    }
}
