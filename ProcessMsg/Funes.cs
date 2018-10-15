using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinperUpdateDAO;

namespace ProcessMsg
{
    public class Funes
    {
        public static int Agregar(Model.FunesNotificacionBo funes)
        {
            var query = new AddFunes();
            try
            {
                int nRow = query.Execute(funes.IdSolicitud, funes.FechaSolicitud, funes.Estado, funes.Comentario, funes.Token);
                if (nRow > 0)
                {
                    foreach (var empresa in funes.Funes)
                    {
                        if (query.Execute(funes.IdSolicitud, funes.FechaSolicitud, empresa.rutEmpresa, empresa.unidadGestion.codigo, empresa.unidadGestion.glosa) > 0)
                        {
                            foreach (var trabajador in empresa.trabajadores)
                            {
                                string[] mes = trabajador.mesPrimerDescuento.Split(new Char[] { '-' });
                                string tipoNotificacion = "";
                                for (int i=0; i < trabajador.tipoNotificacion.Count;i++)
                                {
                                    if (i == 0)
                                    {
                                        tipoNotificacion = trabajador.tipoNotificacion[i].ToString();
                                    }
                                    else
                                    {
                                        tipoNotificacion += "," + trabajador.tipoNotificacion[i].ToString();
                                    }
                                }

                                query.Execute(funes.IdSolicitud, funes.FechaSolicitud, empresa.rutEmpresa, trabajador.rut,
                                    trabajador.folioFUN, tipoNotificacion, trabajador.codigoIsapre, trabajador.ppPeso,
                                    trabajador.ppUF, trabajador.ppPorcentaje, trabajador.estadoFUN, trabajador.motivoRechazo,
                                    trabajador.fechaMotivo, trabajador.observacionRechazo, int.Parse(mes[0]),
                                    int.Parse(mes[1]), trabajador.enviadoFun ? 1 : 0);
                            }
                        }
                    }
                }

                return nRow;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                throw new Exception(msg, ex);
            }

        }

        public static List<Model.FunesTrabajadorBo> GetFunes(int idCliente, EventLog log)
        {
            try
            {
                var lista = new List<Model.FunesTrabajadorBo>();
                var r = new CnaFunes().Execute(idCliente);

                while (r.Read())
                {
                    var listaTipos = new List<int>();
                    foreach (var valor in r["tipo_modificacion"].ToString().Split(new Char[] { ',' }))
                    {
                        listaTipos.Add(int.Parse(valor));
                    }

                    lista.Add(new Model.FunesTrabajadorBo
                    {
                        rut = r["rutEmpresa"].ToString(),
                        folioFUN = r["folio_fun"].ToString(),
                        codigoIsapre = r["codigoIsapre"].ToString(),
                        ppPeso = decimal.Parse(r["ppPeso"].ToString()),
                        ppUF = decimal.Parse(r["ppUF"].ToString()),
                        ppPorcentaje = decimal.Parse(r["ppPorcentaje"].ToString()),
                        estadoFUN = int.Parse(r["estadoFUN"].ToString()),
                        motivoRechazo = int.Parse(r["motivoRechazo"].ToString()),
                        fechaMotivo = r["fechaMotivo"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(r["fechaMotivo"].ToString()),
                        observacionRechazo = r["observacionRechazo"].ToString(),
                        mesPrimerDescuento = string.Format("{0}-{1}", r["mesPrimerDescuento"].ToString(), r["mesPrimerDescuento"].ToString()),
                        enviadoFun = int.Parse(r["enviadoFun"].ToString()) == 1,
                        tipoNotificacion = listaTipos
                    });
                }

                r.Close();
                return lista;
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                if (log != null) log.WriteEntry(msg, EventLogEntryType.Error);
                throw new Exception(msg, ex);
            }
        }

        public static int Actualizar(int idCliente, EventLog log)
        {
            var query = new UpdFunes();
            try
            {
                return query.Execute(idCliente);
            }
            catch (Exception ex)
            {
                var msg = "Excepcion Controlada: " + ex.Message;
                if (log != null) log.WriteEntry(msg, EventLogEntryType.Error);
                throw new Exception(msg, ex);
            }
        }
    }
}
