using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class ComponenteModuloBo
    {
        public ComponenteModuloBo()
        {
            LblEliminar = "Eliminar";
            LblModificar = "Modificar";
        }
        public int idComponentesModulos { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Modulo { get; set; }
        public TipoComponenteBo TipoComponentes { get; set; }
        public bool VerDescripcion { get; set; }
        public string LblEliminar { get; set; }
        public string LblModificar { get; set; }

        public string Directorio { get; set; }
        public string NomModulo { get; set; }

    }
}
