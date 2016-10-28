using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProcessMsg.Model;
using Newtonsoft.Json;

namespace WinPerUpdateUI
{
    public partial class Ambiente : Form
    {
        public Ambiente()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

        }

        private void txtNroLicencia_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNroLicencia_Leave(object sender, EventArgs e)
        {
            string server = ConfigurationManager.AppSettings["server"];
            string port = ConfigurationManager.AppSettings["port"];

            var cliente = new ClienteBo();
            string json = Utils.StrSendMsg(server, int.Parse(port), "checklicencia#" + txtNroLicencia.Text + "#");
            cliente = JsonConvert.DeserializeObject<ClienteBo>(json);
            if (cliente == null)
            {
                MessageBox.Show("Nro de licencia no existe. Favor intente nuevamente");
                txtNroLicencia.Text = "";
                txtNroLicencia.Focus();
            }
            else
            {
                var ambientes = new List<AmbienteBo>();
                json = Utils.StrSendMsg(server, int.Parse(port), "ambientes#" + cliente.Id + "#");
                ambientes = JsonConvert.DeserializeObject<List<AmbienteBo>>(json);
                if (ambientes != null)
                {
                    clbAmbientes.Items.Clear();

                    foreach (var item in ambientes)
                    {
                        clbAmbientes.Items.Add(item.Nombre, false);
                    }
                }
            }
        }
    }
}
