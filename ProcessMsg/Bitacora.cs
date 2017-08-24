using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Bitacora
    {
        public static List<Model.BitacoraBo> GetBitacoraByUsuario(int usuario)
        {
            try
            {
                List<Model.BitacoraBo> listaFmt = new List<Model.BitacoraBo>();
                List<Model.BitacoraBo> lista = new List<Model.BitacoraBo>();
                var dr = new CnaBitacora().Execute(usuario);
                while (dr.Read())
                {
                    lista.Add(new Model.BitacoraBo
                    {
                        Id = int.Parse(dr["Id"].ToString()),
                        Menu = dr["Menu"].ToString(),
                        Vant = dr["Vant"].ToString(),
                        Vnue = dr["Vnue"].ToString(),
                        Accion = char.Parse(dr["Accion"].ToString()),
                        Fecha = Convert.ToDateTime(dr["Fecha"].ToString()),
                        Usuario = int.Parse(dr["Usuario"].ToString()),
                        Registro = dr["Registro"].ToString()
                    });
                }
                dr.Close();
                foreach (var b in lista)
                {
                    if (b.Accion == 'U')
                    {
                        var splitVant = b.Vant.Split('|');
                        var splitVnue = b.Vnue.Split('|');
                        for (int i = 0; i < splitVant.Length; i++)
                        {
                            listaFmt.Add( new Model.BitacoraBo
                            {
                                Id = b.Id,
                                Menu = b.Menu,
                                Vant = splitVant[i],
                                Vnue = splitVnue[i],
                                Accion = b.Accion,
                                Fecha = b.Fecha,
                                Usuario = b.Usuario,
                                Registro = b.Registro
                            });
                        }
                    }
                    else
                    {
                        listaFmt.Add(b);
                    }
                }


                return listaFmt;
            }
            catch(Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        public static List<Model.BitacoraBo> GetBitacoraByMenu(string menu)
        {
            try
            {
                List<Model.BitacoraBo> listaFmt = new List<Model.BitacoraBo>();
                List<Model.BitacoraBo> lista = new List<Model.BitacoraBo>();
                var dr = new CnaBitacora().Execute(menu);
                while (dr.Read())
                {
                    lista.Add(new Model.BitacoraBo
                    {
                        Id = int.Parse(dr["Id"].ToString()),
                        Menu = dr["Menu"].ToString(),
                        Vant = dr["Vant"].ToString(),
                        Vnue = dr["Vnue"].ToString(),
                        Accion = char.Parse(dr["Accion"].ToString()),
                        Fecha = Convert.ToDateTime(dr["Fecha"].ToString()),
                        Usuario = int.Parse(dr["Usuario"].ToString()),
                        Registro = dr["Registro"].ToString()
                    });
                }
                dr.Close();
                foreach (var b in lista)
                {
                    if (b.Accion == 'U')
                    {
                        var splitVant = b.Vant.Split('|');
                        var splitVnue = b.Vnue.Split('|');
                        for (int i = 0, j = 0; i < splitVant.Length || j < splitVnue.Length; i++, j++)
                        {
                            listaFmt.Add(new Model.BitacoraBo
                            {
                                Id = b.Id,
                                Menu = b.Menu,
                                Vant = i < splitVant.Length ? splitVant[i] : "",
                                Vnue = j < splitVnue.Length ? splitVnue[j] : "",
                                Accion = b.Accion,
                                Fecha = b.Fecha,
                                Usuario = b.Usuario,
                                Registro = b.Registro
                            });
                        }
                    }
                    else
                    {
                        listaFmt.Add(b);
                    }
                }


                return listaFmt;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        public static int AddBitacora(string menu, string vant, string vnue, char accion, DateTime fecha, int usuario, string registro)
        {
            try
            {
                vant =  vant != null ? vant.Trim('|') : vant;
                vnue = vnue != null ?  vnue.Trim('|') : vnue;
                int res = 0;
                switch (accion)
                {
                    case 'I':
                        if (!string.IsNullOrEmpty(vnue))
                        {
                            res = new AddBitacora().Execute(menu, "", vnue, accion, fecha, usuario, registro);
                        }
                        break;
                    case 'D':
                        if (!string.IsNullOrEmpty(vant))
                        {
                            res = new AddBitacora().Execute(menu, vant, "", accion, fecha, usuario, registro);
                        }
                        break;
                    case 'U':
                        var splitAnt = vant.Split('|').ToList();
                        var splitNue = vnue.Split('|').ToList();
                        foreach (var va in splitAnt)
                        {
                            if (splitNue.Exists(x => x.Equals(va)) && !string.IsNullOrEmpty(va))
                            {
                                vant = vant.Replace(va, "");
                                vnue = vnue.Replace(va, "");
                            }
                        }
                        vant = PrepararValorBitacora(vant);
                        vnue = PrepararValorBitacora(vnue);
                        if (string.IsNullOrEmpty(vant) && string.IsNullOrEmpty(vnue)) { }
                        else
                        {
                            res = new AddBitacora().Execute(menu, vant, vnue, accion, fecha, usuario, registro);
                        }
                        break;
                    case 'N':
                        res = new AddBitacora().Execute(menu, "Estado=Vigente", "Estado=No Vigente", accion, fecha, usuario, registro);
                        break;
                    case 'V':
                        res = new AddBitacora().Execute(menu, "Estado=No Vigente", "Estado=Vigente", accion, fecha, usuario, registro);
                        break;
                    default:
                        break;
                }

                return res;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }
        /// <summary>
        /// Esta funcion solo permite bitacoras con accion 'I'
        /// </summary>
        /// <param name="bitacoras"></param>
        /// <returns></returns>
        public static bool AddBitacora(List<Model.BitacoraBo> bitacoras)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("1");
                dt.Columns.Add("2");
                dt.Columns.Add("3");
                dt.Columns.Add("4");
                dt.Columns.Add("5");
                dt.Columns.Add("6");
                dt.Columns.Add("7");
                foreach (var b in bitacoras)
                {
                    dt.Rows.Add(new object[] { b.Menu, b.Vant, b.Vnue, b.Accion, b.Fecha, b.Usuario, b.Registro });
                }

                return new AddBitacora().Execute(dt);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }
        }

        private static string PrepararValorBitacora(string valor)
        {
            string str = "";
            var splitVal = valor.Split('|');
            for (int i = 0; i < splitVal.Length; i++)
            {
                if (!string.IsNullOrEmpty(splitVal[i]))
                {
                    str += splitVal[i] + (i == splitVal.Length - 1 ? "" : "|");
                }
            }
            return str.Trim('|');
        }

    }
}
