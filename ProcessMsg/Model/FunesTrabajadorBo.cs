using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class FunesTrabajadorBo
    {
        public string rut { get; set; }

        public string folioFUN { get; set; }

        public List<int> tipoNotificacion { get; set; }

        public string codigoIsapre { get; set; }

        public decimal ppPeso { get; set; }

        public decimal ppUF { get; set; }

        public decimal ppPorcentaje { get; set; }

        public int estadoFUN { get; set; }

        public int motivoRechazo { get; set; }

        public DateTime? fechaMotivo { get; set; }

        public string observacionRechazo { get; set; }

        public string mesPrimerDescuento { get; set; }

        public bool enviadoFun { get; set; }
    }
}
