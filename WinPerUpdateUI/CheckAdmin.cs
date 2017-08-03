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
    public partial class CheckAdmin : Form
    {
        public CheckAdmin()
        {
            InitializeComponent();
        }

        private void BtnValidar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNombre.Text))
            {
                MessageBox.Show("El campo 'E-Mail del Administrador' está vacío.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string server = Utils.GetSetting("server");
                string port = Utils.GetSetting("port");

                string resultado = Utils.StrSendMsg(server, int.Parse(port), string.Format("checkadmin#{0}#{1}#{2}#", TxtNombre.Text, Utils.GetMd5Hash(TxtPassword.Text), licencia));
                DialogResult = resultado.Equals("1") ? DialogResult.Yes : DialogResult.No;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Excepcion CheckAdmin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("CheckAdmin.log", ex.ToString());
            }
        }

        private void CheckAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}
