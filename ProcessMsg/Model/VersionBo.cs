using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessMsg.Model
{
    public class VersionBo
    {
        public int IdVersion { get; set; }
        public string Release { get; set; }
        public DateTime Fecha { get; set; }
        public char Estado { get; set; }
        public string Directorio { get; set; }

        public List<AtributosArchivoBo> Componentes { get; set; }

        public string FechaFmt
        {
            get {
                return string.Format("{0:dd/MM/yyyy}", Fecha);
            }
        }

        public int TotalComponentes
        {
            get
            {
                return Componentes == null ? 0 : Componentes.Count();
            }
        }
    }
}
