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
            TxtIpWUS.Text = Utils.GetSetting("server");
            TxtPuertoWUS.Text = Utils.GetSetting("port");
            TxtIpSFTP.Text = Utils.GetSetting("cftp");
        }
        

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var dr = MessageBox.Show("¿Está seguro que desea guardar la configuración?. Esto podría causar que la aplicación no funcione correctamente. Se recomienda contactar a soporte WinPer.", "Adavertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    Utils.SetSetting("server", TxtIpWUS.Text);
                    Utils.SetSetting("port", int.Parse(TxtPuertoWUS.Text));
                    Utils.SetSetting("cftp", TxtIpSFTP.Text);
                    MessageBox.Show("Configuración guardada con exito!.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Restart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error durante el guardado, pruebe ejecutando WinAct como Administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("ErrorConfigGen.log", ex.ToString());
            }
            
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
