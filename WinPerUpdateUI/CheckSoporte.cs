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
    public partial class CheckSoporte : Form
    {
        public CheckSoporte()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNombre.Text))
            {
                MessageBox.Show("El campo 'E-Mail de Soporte' está vacío.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(TxtPassword.Text))
            {
                MessageBox.Show("El campo 'Password' está vacío.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string licencia = (Owner as Ambiente).txtNroLicencia.Text;
                string server = ConfigurationManager.AppSettings["server"];
                string port = ConfigurationManager.AppSettings["port"];

                string resultado = Utils.StrSendMsg(server, int.Parse(port), string.Format("checksop#{0}#{1}#", TxtNombre.Text, Utils.GetMd5Hash(TxtPassword.Text)));
                DialogResult = resultado.Equals("1") ? DialogResult.Yes : DialogResult.No;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Excepcion CheckAdmin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("CheckAdmin.log", ex.ToString());
            }
        }
    }
}
