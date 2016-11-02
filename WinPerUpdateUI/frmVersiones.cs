using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProcessMsg.Model;
using System.Windows.Forms;
using System.Configuration;

namespace WinPerUpdateUI
{
    public partial class frmVersiones : Form
    {
        public frmVersiones()
        {
            InitializeComponent();

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
            string nroLicencia = key.GetValue("Licencia").ToString();
            string version = key.GetValue("Version").ToString();
            string status = key.GetValue("Status").ToString();
            key.Close();

            if (status.ToLower().Equals("updated"))
            {
                lblTitulo.Text = "Usted se encuentra Actualizado";
                lblSubtitulo.Text = "Usted ya tiene la última versión liberada de WINPER. El detalle de esta versión se muestra a continuación:";
            }
            else
            {
                lblTitulo.ForeColor = Color.Red;
                lblTitulo.Text = "Actualización requerida";
                lblSubtitulo.Text = "Usted no tiene la última versión liberada de WINPER (" + version + "). Para actualizarla favor dar click en el botón Instalar ...";
            }

            treeModulos.Nodes.Clear();
            treeModulos.Nodes.Add("Winper V " + version);

            string server = ConfigurationManager.AppSettings["server"];
            string port = ConfigurationManager.AppSettings["port"];

            key.Close();

            if (!string.IsNullOrEmpty(nroLicencia))
            {
                string json = Utils.StrSendMsg(server, int.Parse(port), "checklicencia#" + nroLicencia + "#");
                var cliente = JsonConvert.DeserializeObject<ClienteBo>(json);
                if (cliente != null)
                {
                    var release = new VersionBo();
                    json = Utils.StrSendMsg(server, int.Parse(port), "getversion#" + version + "#");
                    release = JsonConvert.DeserializeObject<VersionBo>(json);

                    string modulo = "";
                    foreach (var componente in release.Componentes)
                    {
                        if (!modulo.Equals(componente.Modulo))
                        {
                            modulo = componente.Modulo;
                            treeModulos.Nodes[0].Nodes.Add(modulo);
                        }
                    }
                }
            }

        }

        private void btnInstalar_Click(object sender, EventArgs e)
        {

        }
    }
}
