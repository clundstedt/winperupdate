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

namespace WinPerUpdateUI
{
    public partial class FrmConfigGeneral : Form
    {
        public FrmConfigGeneral()
        {
            InitializeComponent();
        }

        private void FrmConfigGeneral_Load(object sender, EventArgs e)
        {
            TxtIpWUS.Text = ConfigurationManager.AppSettings["server"];
            TxtPuertoWUS.Text = ConfigurationManager.AppSettings["port"];
            TxtIpSFTP.Text = ConfigurationManager.AppSettings["cftp"];
            ChkEjecutarSQL.Checked = bool.Parse(ConfigurationManager.AppSettings["sql"]);
        }
        

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["server"].Value = TxtIpWUS.Text;
                config.AppSettings.Settings["port"].Value = TxtPuertoWUS.Text;
                config.AppSettings.Settings["sql"].Value = ChkEjecutarSQL.Checked.ToString();
                config.AppSettings.Settings["cftp"].Value = TxtIpSFTP.Text;
                config.Save(ConfigurationSaveMode.Modified);
                MessageBox.Show("Configuración guardada con exito!.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error durante el guardado, pruebe ejecutando WinperUpdate como Administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("ErrorConfigGen.log", ex.ToString());
            }
            
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
