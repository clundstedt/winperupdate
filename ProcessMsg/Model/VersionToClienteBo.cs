using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class VersionToClienteBo
    {
        public ClienteBo Cliente { get; set; }
        public VersionBo Version { get; set; }
        public AmbienteBo Ambiente { get; set; }
        public char Estado { get; set; }
        public DateTime FechaInstalacion { get; set; }

        public string FechaInstalacionFmt
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}",FechaInstalacion);
            }
        }

    }
}
