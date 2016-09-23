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
        public string Comentario { get; set; }
        public string Usuario { get; set; }
        public string Instalador { get; set; }
        public long Length { get; set; }

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

        public string EstadoDisplay
        {
            get
            {
                return Estado == 'P' ? "Publicada" : Estado == 'N' ? "Nueva" : "Caduca";
            }
        }
    }
}
