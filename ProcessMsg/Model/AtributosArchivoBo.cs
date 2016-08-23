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
    }
}
