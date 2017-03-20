using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinPerUpdateUI
{
    partial class AboutWinperUpdate : Form
    {
        public AboutWinperUpdate()
        {
            InitializeComponent();

            string dirTmp = Path.GetTempPath();

            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription + " " + dirTmp;

            this.CenterToScreen();
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            var nameIntalador = Path.Combine(Path.GetTempPath(), "SetUpdateUI.exe");
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
            string statUI = key.GetValue("statUI").ToString();
            key.Close();
            if (!statUI.Equals("updated") && System.IO.File.Exists(nameIntalador))
            {
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate");
                key.SetValue("statUI", "updated");
                key.Close();
                if (Utils.isCentralizado)
                {
                    var argument = string.Format("/DIR=\"{0}\" /VERYSILENT /SUPPRESSMSGBOXES /NORESTART /NOCANCEL", Directory.GetCurrentDirectory());
                    SvcWPUI.Start(new string[] { nameIntalador, argument });
                }
                else
                {
                    Process.Start(nameIntalador, string.Format("/DIR=\"{0}\"", Directory.GetCurrentDirectory()));
                }
            }
            else
            {
                this.Close();
            }
        }

        private void AboutWinperUpdate_Load(object sender, EventArgs e)
        {
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
            string statUI = key.GetValue("statUI").ToString();
            key.Close();
            okButton.Text = statUI.Equals("updated") ? "OK" : "Actualizar";
        }

        private void labelVersion_Click(object sender, EventArgs e)
        {

        }
    }
}
