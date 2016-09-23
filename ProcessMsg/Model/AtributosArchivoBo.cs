using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessMsg.Model
{
    public class AtributosArchivoBo
    {
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime LastWrite { get; set; }
        public long Length { get; set; }
        public string Version { get; set; }
        public string Modulo { get; set; }
        public string Comentario { get; set; }

        public string Tipo
        {
            get
            {
                if (Name.EndsWith("exe"))
                {
                    return "exe";
                }
                else if (Name.EndsWith("qrp"))
                {
                    return "qrp";
                }
                return "otro";
            }
        }

        public string DateCreateFmt
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy hhhh:mm:ss}", DateCreate);
            }
        }

        public string LastWriteFmt
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy hhhh:mm:ss}", LastWrite);
            }
        }
    }
}
