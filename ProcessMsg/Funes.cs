using System;
using System.Collections.Generic;
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
    }
}
