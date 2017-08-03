﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessMsg.Model
{
    public class AtributosArchivoBo
    {
        public int idVersion { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime LastWrite { get; set; }
        public long Length { get; set; }
        public string Version { get; set; }
        public string Modulo { get; set; }
        public string Comentario { get; set; }
        
        public char Tipo { get; set; }

        public string TipoFmt
        {
            get
            {
                string str = "";
                switch (Tipo)
                {
                    case 'A': str = "Alter";break;
                    case 'S': str = "Procedimiento Almacenado";break;
                    case 'F': str = "Funcion";break;
                    case 'T': str = "Trigger";break;
                    case 'V': str = "Vista";break;
                    case 'Q': str = "Query";break;
                    case '*': str = "Archivo";break;
                    default:
                        str = "Tipo desconocido";
                        break;
                }
                return str;
            }
        }

        public string Extension
        {
            get
            {
                try
                {
                    var split = Name.Split('.');
                    return "." + split[split.Length - 1];
                }
                catch(Exception )
                {
                    return "otro";
                }
            }
        }

        public string MotorSql { get; set; }

        public string DateCreateFmt
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy hhhh:mm:ss}", DateCreate);
            }
        }

        public string DateCreateXml
        {
            get
            {
                return string.Format("{0:yyyy-MM-ddThhhh:mm:ss}", DateCreate);
            }
        }
        
        public string LastWriteFmt
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy hhhh:mm:ss}", LastWrite);
            }
        }

        public string MensajeToolTip { get; set; }

        public string Directorio { get; set; }

        /// <summary>
        /// Retorna información para el registro de la bitacora.
        /// </summary>
        /// <param name="accion">I: Insertado, D: Eliminado, U: Modificado y ?: Dato de Registro</param>
        /// <returns></returns>
        public string Bitacora(char accion)
        {
            string str = "";
            switch (accion)
            {
                case 'I':
                    str = string.Format("{0} Insertado", Name);
                    break;
                case 'D':
                    str = string.Format("{0} Eliminado", Name);
                    break;
                case 'U':
                    str = string.Format(@"Modulo={0}|idVersion={1}|NameFile={2}|VersionFile={3}|FechaFile={4}|Comentario={5}"
                                        , Modulo
                                        , idVersion
                                        , Name
                                        , Version
                                        , LastWrite
                                        , Comentario);
                    break;
                case '?':
                    str = string.Format("{0} (ID Versión: {1})", Name, idVersion);
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
