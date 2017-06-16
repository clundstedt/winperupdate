using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class MenusBo
    {
        public int idMenus { get; set; }
        public string Descripcion { get; set; }
        public string Link { get; set; }
        public int CodPrf { get; set; }
        public string Icon { get; set; }

        public string Submenus { get; set; }

        public string[,] GetSubMenus
        {
            get
            {
                string[,] rSubMenus;
                var data = Submenus.Split('|');
                rSubMenus = new string[data.Length,2];
                for (int i = 0; i < data.Length; i++)
                {
                    var campos = data[i].Split('=');
                    rSubMenus[i, 0] = campos[0];
                    rSubMenus[i, 1] = campos[1];
                }
                
                return rSubMenus;
            }
        }





    }
}
