using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinPerUpdateUI
{
    public partial class Instalar : Form
    {
        public string ambiente = "";

        public Instalar()
        {
            InitializeComponent();
        }

        private void Instalar_Load(object sender, EventArgs e)
        {
            loginstalacion.Text = "Preparando instalación en " + ambiente;
        }

        private void timerInstalar_Tick(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
                string status = key.GetValue("Status").ToString();
                key.Close();

                if (status.Equals("Begin"))
                {
                    string dirTmpversion = Path.GetTempPath();
                    dirTmpversion += dirTmpversion.EndsWith("\\") ? "" : "\\winper";
                    DirectoryInfo di = new DirectoryInfo(dirTmpversion);
                    foreach (var fi in di.GetFiles())
                    {
                        //Console.WriteLine(fi.Name);

                    }

                }
            }
            catch (Exception )
            {

            }
        }
    }
}
