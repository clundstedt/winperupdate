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
using System.IO;
using System.Diagnostics;

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

            if (string.IsNullOrEmpty(version))
            {
                lblTitulo.Text = "Usted se encuentra Actualizado";
                lblSubtitulo.Text = "Usted ya tiene la última versión liberada de WINPER. Pronto le informaremos de nuevas actualizaciones";
                btnInstalar.Enabled = false;
                return;
            }
            if (status.ToLower().Equals("updated"))
            {
                lblTitulo.Text = "Usted se encuentra Actualizado";
                lblSubtitulo.Text = "Usted ya tiene la última versión liberada de WINPER. El detalle de esta versión se muestra a continuación:";
                btnInstalar.Enabled = false;
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

                    if (release.Componentes != null)
                    {
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

        }

        private void btnInstalar_Click(object sender, EventArgs e)
        {
            string dirTmp = Path.GetTempPath();
            dirTmp += dirTmp.EndsWith("\\") ? "" : "\\";

            string server = ConfigurationManager.AppSettings["server"];
            string port = ConfigurationManager.AppSettings["port"];


            for (int i = 0; i < treeModulos.Nodes.Count; i++)
            {
                string[] token = treeModulos.Nodes[i].Text.Split(new Char[] { ' ' });

                var version = new VersionBo();
                string json = Utils.StrSendMsg(server, int.Parse(port), "getversion#" + token[2] + "#");
                version = JsonConvert.DeserializeObject<VersionBo>(json);

                if (version != null)
                {
                    if (File.Exists(dirTmp + version.Instalador))
                    {
                        string Command = dirTmp + version.Instalador;

                        Process myProcess = new Process();
                        myProcess.StartInfo.FileName = Command;
                        myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                        myProcess.StartInfo.RedirectStandardError = true;
                        myProcess.StartInfo.UseShellExecute = false;

                        myProcess.Start();
                    }
                }
            }
        }

        private void treeModulos_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string server = ConfigurationManager.AppSettings["server"];
            string port = ConfigurationManager.AppSettings["port"];

            string modulo = e.Node.Text;

            var padre = e.Node.Parent;
            if (padre == null)
            {
                string[] token = modulo.Split(new Char[] { ' ' });

                var version = new VersionBo();
                string json = Utils.StrSendMsg(server, int.Parse(port), "getversion#" + token[2] + "#");
                version = JsonConvert.DeserializeObject<VersionBo>(json);

                listView1.Clear();

                listView1.Columns.Add("Número", 80, HorizontalAlignment.Left);
                listView1.Columns.Add("Fecha", 120, HorizontalAlignment.Left);
                listView1.Columns.Add("Comentario", 290, HorizontalAlignment.Left);

                if (version != null)
                {

                    var item = new ListViewItem(version.Release);
                    item.SubItems.Add(version.FechaFmt);
                    item.SubItems.Add(version.Comentario);

                    listView1.Items.Add(item);
                }
            }
            else
            {
                string[] token = padre.Text.Split(new Char[] { ' ' });

                var version = new VersionBo();
                string json = Utils.StrSendMsg(server, int.Parse(port), "getversion#" + token[2] + "#");
                version = JsonConvert.DeserializeObject<VersionBo>(json);

                listView1.Clear();

                listView1.Columns.Add("Nombre", 120, HorizontalAlignment.Left);
                listView1.Columns.Add("Fecha", 120, HorizontalAlignment.Left);
                listView1.Columns.Add("Versión", 70, HorizontalAlignment.Left);
                listView1.Columns.Add("Comentario", 290, HorizontalAlignment.Left);

                if (version != null)
                {

                    foreach (var componente in version.Componentes.Where(x => x.Modulo.Equals(modulo)))
                    {
                        var item = new ListViewItem(componente.Name);
                        item.SubItems.Add(componente.DateCreateFmt);
                        item.SubItems.Add(componente.Version);
                        item.SubItems.Add(componente.Comentario);

                        listView1.Items.Add(item);
                    }
                }

            }
        }

        private void treeModulos_Click(object sender, EventArgs e)
        {

        }
    }
}
