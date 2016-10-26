using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class ClienteBo
    {
        public int Id { get; set; }

        public int Rut { get; set; }

        public char Dv { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public ComunaBo Comuna { get; set; }

        public string NroLicencia { get; set; }

        public string RutFmt
        {
            get
            {
                return string.Format("{0}-{1}", Rut, Dv);
            }
        }

        public int NumFolio { get; set; }
        
        public int EstMtc { get; set; }
        public string Mesini { get; set; }
        public string NroTrbc { get; set; }
        public string NroTrbh { get; set; }
        public string NroUsr { get; set; }
        /*
        public string MesIniFmt
        {
            get
            {
                return string.Format("{0}-{1}-{2}",DateTime.Now.);
            }
        }*/
    }
}
