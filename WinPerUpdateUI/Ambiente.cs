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
                        dgAmbientes.Rows.Clear();

                        foreach (var item in lista)
                        {
                            Microsoft.Win32.RegistryKey keyv = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate\" + item.Nombre);

                            try
                            {
                                string directorio = keyv.GetValue("DirWinper").ToString();
                                dgAmbientes.Rows.Add(item.idAmbientes, item.Nombre, directorio);
                                keyv.Close();
                            }
                            catch (Exception )
                            {
                                dgAmbientes.Rows.Add(item.idAmbientes, item.Nombre, "");
                            }
                        }
                    }
                    
                    int index = cmbPerfil.FindString(perfil);
                    cmbPerfil.SelectedIndex = index;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("Ambiente.log", ex.ToString());
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var dr = new CheckAdmin().ShowDialog(this);
            if(dr == DialogResult.No)
            {
                MessageBox.Show("Administrador incorrecto.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate");
            key.SetValue("Licencia", txtNroLicencia.Text);
            key.SetValue("Perfil", cmbPerfil.Items[cmbPerfil.SelectedIndex]);

            string ambientes = "";
            foreach (DataGridViewRow item in dgAmbientes.Rows)
            {
                string nombreAmbiente = item.Cells[1].Value.ToString();
                string directorio = item.Cells[2].Value.ToString();

                ambientes += ambientes.Length == 0 ? nombreAmbiente : "," + nombreAmbiente;
                Microsoft.Win32.RegistryKey keya = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate\" + nombreAmbiente);
                try
                {
                    string version = keya.GetValue("Version").ToString();
                    keya.SetValue("DirWinper", directorio);
                }
                catch (Exception)
                {
                    keya.SetValue("Version", "");
                    keya.SetValue("DirWinper", directorio);
                    keya.SetValue("Instalacion", 0);
                }
                try

                {
                    string estado = keya.GetValue("Status").ToString();
                }
                catch (Exception)
                {
                    keya.SetValue("Status", "");
                }

                try
                {
                    var ins = keya.GetValue("Instalacion");
                }
                catch (Exception)
                {
                    keya.SetValue("Instalacion", 0);
                }
                keya.Close();
            }

            key.SetValue("Ambientes", ambientes);
            key.Close();

            if (cmbPerfil.SelectedIndex == 0)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["sql"].Value = chkPermitirSQL.Checked.ToString();
                config.Save(ConfigurationSaveMode.Modified);
            }
            
            this.Close();
            MessageBox.Show("WinperUpdate se reiniciará para conservar los cambios.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
        }

        private void txtNroLicencia_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNroLicencia_Leave(object sender, EventArgs e)
        {
            string server = ConfigurationManager.AppSettings["server"];
            string port = ConfigurationManager.AppSettings["port"];

            try
            {
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
                        dgAmbientes.Rows.Clear();

                        foreach (var item in ambientes)
                        {
                            string dir = "";
                            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate\" + item.Nombre);
                            if (key != null)
                            {
                                dir = key.GetValue("DirWinper") == null  ? "" : key.GetValue("DirWinper").ToString();
                                key.Close();
                            }
                            dgAmbientes.Rows.Add(item.idAmbientes, item.Nombre, dir);
                        }
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("CheckLicencia.log", ex.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Ambiente_Load(object sender, EventArgs e)
        {
            var chek = ConfigurationManager.AppSettings["sql"];
            if (chek != null)
            {
                chkPermitirSQL.Checked = bool.Parse(chek);
            }
        }

        private void cmbPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkPermitirSQL.Visible = (cmbPerfil.SelectedIndex == 0);
        }
    }
}
