using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class BitacoraBo
    {
        public int Id { get; set; }
        public string Menu { get; set; }
        public string Vant { get; set; }
        public string Vnue { get; set; }
        public char Accion { get; set; }
        public DateTime Fecha { get; set; }
        public int Usuario { get; set; }
        public string Registro { get; set; }
        public string AccionFmt
        {
            get
            {
                string str = "";
                switch (Accion)
                {
                    case 'I': str = "Insert"; break;
                    case 'U':str = "Update";break;
                    case 'D':str = "Delete";break;
                    case 'N':str = "No Vigente";break;
                    case 'V':str = "Vigente";break;
                    default: str = "Sin Accion"; break;
                }
                return str;
            }
        }
        public string FechaFmt
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", Fecha);
            }
        }

        public string Campo
        {
            get
            {
                return Accion == 'N'|| Accion == 'V'|| Accion == 'U' ? Vant.Split('=')[0] : "";
            }
        }
        public string VantFmt
        {
            get
            {
                return Accion == 'N' || Accion == 'V' || Accion == 'U' ? Vant.Split('=')[1] : Vant;
            }
        }
        public string VnueFmt
        {
            get
            {
                return Accion == 'N' || Accion == 'V' || Accion == 'U' ? Vnue.Split('=')[1] : Vnue;
            }
        }

    }
}
