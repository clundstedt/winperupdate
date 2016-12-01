﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMsg.Model
{
    public class TipoComponenteBo
    {
        public TipoComponenteBo()
        {
            LblEliminarTipo = "Eliminar";
        }
        public int idTipoComponentes { get; set; }
        public string Nombre { get; set; }
        public bool isCompBD { get; set; }
        public string IsCompBDFmt
        {
            get
            {
                return isCompBD ? "Componente parte de Base de Datos" : "Componente NO es parte de Base de Datos";
            }
        }

        public string LblEliminarTipo { get; set; }
    }
}
