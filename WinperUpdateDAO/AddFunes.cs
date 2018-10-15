using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateDAO 
{
    public class AddFunes : SpDao
    {
        public int Execute(string IdSolicitud, DateTime FechaSolicitud, int Estado, string Comentario, string Token)
        {
            SpName = @"INSERT INTO Funes (  idSolicitud
                                           ,fecha
                                           ,estado
                                           ,comentario
                                           ,token)
                                    VALUES (@IdSolicitud
                                           ,@FechaSolicitud
                                           ,@Estado
                                           ,@Comentario
                                           ,@Token)";
            try
            {
                ParmsDictionary.Clear();
                ParmsDictionary.Add("@IdSolicitud", IdSolicitud);
                ParmsDictionary.Add("@FechaSolicitud", FechaSolicitud);
                ParmsDictionary.Add("@Estado", Estado);
                ParmsDictionary.Add("@Comentario", Comentario);
                ParmsDictionary.Add("@Token", Token);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int Execute(string IdSolicitud, DateTime FechaSolicitud, string rutEmpresa, string codGestion, string glosaGestion)
        {
            SpName = @"INSERT INTO EmpresaFunes (   idSolicitud
                                                   ,fecha
                                                   ,rutEmpresa
                                                   ,codGestion
                                                   ,glosaGestion)
                                    VALUES (@IdSolicitudE
                                           ,@FechaSolicitudE
                                           ,@rutEmpresa
                                           ,@codGestion
                                           ,@glosaGestion)";
            try
            {
                ParmsDictionary.Clear();
                ParmsDictionary.Add("@IdSolicitudE", IdSolicitud);
                ParmsDictionary.Add("@FechaSolicitudE", FechaSolicitud);
                ParmsDictionary.Add("@rutEmpresa", rutEmpresa);
                ParmsDictionary.Add("@codGestion", codGestion);
                ParmsDictionary.Add("@glosaGestion", glosaGestion);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);

            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public int Execute(string IdSolicitud, DateTime FechaSolicitud, string rutEmpresa, string rut_trabajador,
                           string folio_fun, string tipo_modificacion, string codigoIsapre,
                           decimal ppPeso, decimal ppUF, decimal ppPorcentaje, int estadoFUN, int motivoRechazo,
                           DateTime? fechaMotivo, string observacionRechazo, int mesPrimerDescuento, int añoPrimerDescuento,
                           int enviadoFun)
        {
            SpName = @"INSERT INTO TrabajadorEmpresaFunes (   
                                                    idSolicitud
                                                   ,fecha
                                                   ,rutEmpresa
                                                   ,rut_trabajador
                                                   ,folio_fun
                                                   ,tipo_modificacion
                                                   ,codigoIsapre
                                                   ,ppPeso
                                                   ,ppUF
                                                   ,ppPorcentaje
                                                   ,estadoFUN
                                                   ,motivoRechazo
                                                   ,fechaMotivo
                                                   ,observacionRechazo
                                                   ,mesPrimerDescuento
                                                   ,añoPrimerDescuento
                                                   ,enviadoFun)
                                    VALUES (@IdSolicitudT
                                           ,@FechaSolicitudT
                                           ,@rutEmpresaT
                                           ,@rut_trabajador
                                           ,@folio_fun
                                           ,@tipo_modificacion
                                           ,@codigoIsapre
                                           ,@ppPeso
                                           ,@ppUF
                                           ,@ppPorcentaje
                                           ,@estadoFUN
                                           ,@motivoRechazo
                                           ,@fechaMotivo
                                           ,@observacionRechazo
                                           ,@mesPrimerDescuento
                                           ,@añoPrimerDescuento
                                           ,@enviadoFun)";
            try
            {
                ParmsDictionary.Clear();
                ParmsDictionary.Add("@IdSolicitudT", IdSolicitud);
                ParmsDictionary.Add("@FechaSolicitudT", FechaSolicitud);
                ParmsDictionary.Add("@rutEmpresaT", rutEmpresa);
                ParmsDictionary.Add("@rut_trabajador", rut_trabajador);
                ParmsDictionary.Add("@folio_fun", folio_fun);
                ParmsDictionary.Add("@tipo_modificacion", tipo_modificacion);
                ParmsDictionary.Add("@codigoIsapre", codigoIsapre);
                ParmsDictionary.Add("@ppPeso", ppPeso);
                ParmsDictionary.Add("@ppUF", ppUF);
                ParmsDictionary.Add("@ppPorcentaje", ppPorcentaje);
                ParmsDictionary.Add("@estadoFUN", estadoFUN);
                ParmsDictionary.Add("@motivoRechazo", motivoRechazo);
                ParmsDictionary.Add("@fechaMotivo", fechaMotivo);
                ParmsDictionary.Add("@observacionRechazo", observacionRechazo);
                ParmsDictionary.Add("@mesPrimerDescuento", mesPrimerDescuento);
                ParmsDictionary.Add("@añoPrimerDescuento", añoPrimerDescuento);
                ParmsDictionary.Add("@enviadoFun", enviadoFun);

                return Connector.ExecuteQueryNoResult(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Execute", ex.Message);
                throw new Exception(msg, ex);
            }

        }
    }
}
