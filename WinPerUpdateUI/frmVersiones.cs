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
using System.Management;

namespace WinPerUpdateUI
{
    public partial class frmVersiones : Form
    {
        public string ambiente = String.Empty;
        private Microsoft.Win32.RegistryKey keyv = null;
        private string directorio = String.Empty;
        private Form1 Progreso;//ventana de progreso
        private const int MAX_INTENTOS = 3;//maximo de intentos para instalar la actualizacion
        public frmVersiones()
        {
            InitializeComponent();
            Progreso = new Form1();
        }

        private void btnInstalar_Click(object sender, EventArgs e)
        {
            
            try
            {
                string dirTmp = Path.GetTempPath();
                dirTmp += dirTmp.EndsWith("\\") ? "" : "\\";
                dirTmp += "WinPer\\" + ambiente + "\\";

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
                        string ProcRun = CheckProcessRun(version);
                        if (!string.IsNullOrEmpty(ProcRun))
                        {
                            MessageBox.Show(string.Format("Los siguientes procesos se deben cerrar para proceder con la actualización de Winper:\n\n{0}", ProcRun), "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        var form = new Instalar();
                        string fileInstalador = dirTmp + version.Release + "\\" + version.Instalador;
                        if (File.Exists(fileInstalador))
                        {
                            this.Close();
                            form.ambiente = ambiente;
                            form.Show();

                            string Command = fileInstalador;

                            Process myProcess = new Process();
                            myProcess.StartInfo.FileName = Command;
                            myProcess.StartInfo.Arguments = "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /NOCANCEL";
                            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            myProcess.StartInfo.RedirectStandardError = true;
                            myProcess.StartInfo.UseShellExecute = false;

                            myProcess.Start();
                            myProcess.WaitForExit();

                        }
                        form.Close();
                        string dirComponentes = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\WinperSetupUI\\" + version.Release;
                        var comps = new DirectoryInfo(dirComponentes).GetFiles().ToList();
                        int cont = 0;
                        foreach (var x in comps)
                        {
                            if (version.Componentes.Exists(y => y.Name.Equals(x.Name)))
                            {
                                cont++;
                            }
                        }
                        if (version.TotalComponentes == cont)
                        {
                            Progreso.Show();
                            Progreso.Text = "Instalando";
                            bwCopia.RunWorkerAsync(version);
                        }
                        else
                        {
                            MessageBox.Show("Se produjo un error durante la instalación.\nWinPer Update procederá a preparar nuevamente la actualización. Se le avisará cuando este lista para instalar.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Microsoft.Win32.RegistryKey keya = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate\" + ambiente);
                            var intentos = int.Parse(keya.GetValue("Instalacion").ToString());
                            intentos++;
                            keya.SetValue("Instalacion", intentos);
                            keya.SetValue("Version", "");
                            keya.Close();
                            if (intentos > MAX_INTENTOS)
                            {
                                MessageBox.Show("El N° de intentos de actualización ha llegado a su límite, comuníquese con soporte.", "ERROR", MessageBoxButtons.OK,MessageBoxIcon.Error);
                            }
                            else
                            {
                                System.IO.File.Delete(fileInstalador);
                            }
                        }
                    }

                }
            }catch(Exception ex)
            {
                MessageBox.Show(string.Format("Ocurrió un error inesperado.\n\n{0}. Revise Instalar_ERROR.log", ex.Message), "EXCEPCION CONTROLADA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("Instalar_ERROR.log", ex.ToString());
            }
        }

        private string CheckProcessRun(VersionBo version)
        {
            string NomProc = "";
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate\" + ambiente);
            var DirWinper = key.GetValue("DirWinper") == null ? "" : key.GetValue("DirWinper").ToString();
            key.Close();
            string query = "SELECT ExecutablePath, Name FROM Win32_Process";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject item in searcher.Get())
            {
                object path = item["ExecutablePath"];
                object processname = item["Name"];
                if (path != null && processname != null)
                {
                    Utils.RegistrarLog("CheckProcessRun.log", processname + " : "+path + " : DirWinper "+ DirWinper + " : ("+ new FileInfo(path.ToString()).Directory.FullName+")");
                    Utils.RegistrarLog("CheckProcessRun.log", (version.Componentes.Exists(vercomp => vercomp.Name.Equals(processname.ToString(), StringComparison.OrdinalIgnoreCase))).ToString());
                    Utils.RegistrarLog("CheckProcessRun.log", (path.ToString().Replace(processname.ToString(), "").Equals(DirWinper, StringComparison.OrdinalIgnoreCase)).ToString());
                    
                    if (version.Componentes.Exists(vercomp => vercomp.Name.Equals(processname.ToString(), StringComparison.OrdinalIgnoreCase))
                && new FileInfo(path.ToString()).Directory.FullName.Equals(DirWinper))
                    {
                        NomProc += processname + " \n";
                    }
                }
            }

            return NomProc;
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

        private void frmVersiones_Load(object sender, EventArgs e)
        {

            this.Text = this.Text + " Ambiente " + ambiente;

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
            string nroLicencia = key.GetValue("Licencia").ToString();
            key.Close();

            Microsoft.Win32.RegistryKey keya = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate\" + ambiente);
            string version = keya.GetValue("Version") == null ? "" : keya.GetValue("Version").ToString();
            string status = keya.GetValue("Status") == null ? "": keya.GetValue("Status").ToString();
            keya.Close();

            keyv = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate\" + ambiente);
            directorio = keyv.GetValue("DirWinper").ToString(); 
            keyv.Close();

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

            try
            {
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
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrio un error durante la conexión con el servidor central.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("FrmVersionesLoad.log", ex.ToString());
            }
        }

        private void bwCopia_DoWork(object sender, DoWorkEventArgs e)
        {
            var version = (VersionBo)e.Argument;
            Utils.RegistrarLog("InstallFile.log", "INICIO PROCESO DE COPIA");
            string dirComponentes = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\WinperSetupUI\\" + version.Release;
            if (System.IO.Directory.Exists(dirComponentes))
            {
                Utils.RegistrarLog("InstallFile.log", dirComponentes);
                if (System.IO.Directory.Exists(directorio))
                {
                    Utils.RegistrarLog("InstallFile.log", directorio);
                    var files = new System.IO.DirectoryInfo(dirComponentes).GetFiles().ToList();
                    var cont = files.Count;
                    int progress = 0;
                    Utils.RegistrarLog("InstallFile.log", files.Count.ToString());
                    files.ForEach(x =>
                    {
                        Utils.RegistrarLog("InstallFile.log", x.Name);
                        if (!x.Name.StartsWith("unins000") && !x.Extension.ToUpper().Equals(".SQL"))
                        {
                            foreach (var mc in Utils.ModulosContratados)
                            {
                                if (mc.ComponentesModulo.Exists(cm => cm.Nombre.Equals(x.Name)))
                                {
                                    x.CopyTo(Path.Combine(directorio, x.Name), true);
                                    Utils.RegistrarLog("InstallFile.log", x.Name + " COPIADO A " + Path.Combine(directorio, x.Name) + " Y ELIMINADO");
                                    break;
                                }
                            }
                            x.Delete();
                        }
                        else
                        {
                            Utils.RegistrarLog("InstallFile.log", x.Name + " ES UN ARCHIVO UNINS000 O .SQL, FUE ELIMINADO");
                            x.Delete();
                        }
                        progress++;
                        bwCopia.ReportProgress((progress*100)/cont);
                    });
                    System.IO.Directory.Delete(dirComponentes, true);
                }
            }
        }

        private void bwCopia_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progreso.LblPorcentaje.Text = string.Format("{0}%",e.ProgressPercentage);
            Progreso.PbProgreso.Value = e.ProgressPercentage;
            
        }

        private void bwCopia_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Microsoft.Win32.RegistryKey keya = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate\" + ambiente);
            if (e.Error != null)
            {
                MessageBox.Show(string.Format("Ocurrió un error durante el proceso de instalación, revise log (InstallFile_ERROR) para más detalles.\n\n{0}", e.Error.Message), "ERROR WinperUpdate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("InstallFile_ERROR.log", e.Error.ToString());
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("El proceso de instalación fue cancelado!.");
                Utils.RegistrarLog("InstallFile.log", "El proceso de instalación fue cancelado!");
            }
            else
            {
                Progreso.Close();
                MessageBox.Show("Instalación finalizada con exito.", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                keya.SetValue("Status", "updated");
                Utils.RegistrarLog("InstallFile.log", "El proceso de instalación finalizó con exito!.");
                Application.Restart();
            }
            keya.Close();
        }

     
    }
}
