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

            cmbPerfil.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");

                txtNroLicencia.Text = key.GetValue("Licencia").ToString();
                txtDirWinper.Text = key.GetValue("DirWinPer").ToString();
                string ambientes = key.GetValue("Ambientes").ToString();
                string perfil = key.GetValue("Perfil").ToString();
                key.Close();

                string server = ConfigurationManager.AppSettings["server"];
                string port = ConfigurationManager.AppSettings["port"];

                var cliente = new ClienteBo();
                string json = Utils.StrSendMsg(server, int.Parse(port), "checklicencia#" + txtNroLicencia.Text + "#");
                cliente = JsonConvert.DeserializeObject<ClienteBo>(json);

                if (cliente != null)
                {
                    var lista = new List<AmbienteBo>();
                    json = Utils.StrSendMsg(server, int.Parse(port), "ambientes#" + cliente.Id + "#");
                    lista = JsonConvert.DeserializeObject<List<AmbienteBo>>(json);
                    if (lista != null)
                    {
                        clbAmbientes.Items.Clear();

                        foreach (var item in lista)
                        {
                            clbAmbientes.Items.Add(item.Nombre, ambientes.IndexOf(item.Nombre) >= 0);
                        }
                    }

                    int index = cmbPerfil.FindString(perfil);
                    cmbPerfil.SelectedIndex = index;
                }

            }
            catch (Exception ex) { };
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate");
            key.SetValue("Licencia", txtNroLicencia.Text);
            key.SetValue("Perfil", cmbPerfil.Items[cmbPerfil.SelectedIndex]);
            key.SetValue("DirWinper", txtDirWinper.Text);

            string ambientes = "";
            foreach (var item in clbAmbientes.CheckedItems)
            {
                ambientes += ambientes.Length == 0 ? item : "," + item;
                Microsoft.Win32.RegistryKey keya = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate\" + item);
                try
                {
                    string version = keya.GetValue("Version").ToString();
                }
                catch (Exception ex)
                {
                    keya.SetValue("Version", "");
                }
                try
                {
                    string estado = keya.GetValue("Status").ToString();
                }
                catch (Exception ex)
                {
                    keya.SetValue("Status", "");
                }
                keya.Close();
            }

            key.SetValue("Ambientes", ambientes);
            key.Close();

            this.Close();
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
