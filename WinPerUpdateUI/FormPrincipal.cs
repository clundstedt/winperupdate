using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProcessMsg.Model;
using Newtonsoft.Json;
using System.Diagnostics;

using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WinPerUpdateUI
{
    public partial class FormPrincipal : Form
    {
        const int SIZEBUFFER = 65700;
        ContextMenu ContextMenu1 = new ContextMenu();
        private bool ServerInAccept = true;
        private ClienteBo cliente = new ClienteBo();
        private List<AmbienteBo> ambientes = new List<AmbienteBo>();
        public string ambienteUpdate = "";

        public class DllFileUI
        {
            public string Nombre { get; set; }
            public string VersionArchivo { get; set; }
        }

        public class UpdateUI
        {
            public long SetupLength { get; set; }
            public List<DllFileUI> Lista { get; set; }
        }
        public FormPrincipal()
        {
            InitializeComponent();
            ServerInAccept = true;
            this.CenterToScreen();
        }

        private void Salir_Click(object sender, System.EventArgs e)
        {
            //' Este procedimiento se usa para cerrar el formulario,
            //' se usará como procedimiento de eventos, en principio usado por el botón Salir
            this.Close();
            //Application.Exit();
        }

        private void Restaurar_Click(object sender, System.EventArgs e)
        {
            //' Restaurar por si se minimizó
            //' Este evento manejará tanto los menús Restaurar como el NotifyIcon.DoubleClick
            //ShowInTaskbar = true;
            //WindowState = FormWindowState.Normal;

            var menu = (MenuItem)sender;

            var form = new frmVersiones();
            form.ambiente = menu.Text;
            form.Show();
        }

        private void AcercaDe_Click(object sender, System.EventArgs e)
        {
            var form = new AboutWinperUpdate();
            form.Show();
        }

        private void FormPrincipal_Resize(object sender, EventArgs e)
        {
            //' Cuando se minimice, ocultarla, se quedará disponible en la barra de tareas
            if (this.WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
            }
        }

        private void FormPrincipal_Activated(object sender, EventArgs e)
        {
        }

        private void notifyIcon2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*
            //ShowInTaskbar = true;
            //WindowState = FormWindowState.Normal;
            var form = new frmVersiones();
            form.ambiente = ambienteUpdate;
            form.ShowDialog();
            */

        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            Utils.isCentralizado = File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ProcessWPUI.exe"));
            try
            {
                if (SvcWPUI.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    SvcWPUI.Stop();
                }
            }
            catch (Exception) { }
            
            Utils.RegistrarLog("Load.log", "UI Iniciado");
            Utils.RegistrarLog("Load.log", "-----");
            ContextMenu1.MenuItems.Add("Configurar Ambiente y Licencia", new EventHandler(this.Ambiente_Click));
            ContextMenu1.MenuItems[0].Enabled = true;

            ContextMenu1.MenuItems.Add("-");
            ContextMenu1.MenuItems.Add("&Acerca de...", new EventHandler(this.AcercaDe_Click));
            ContextMenu1.MenuItems[1].DefaultItem = true;

            ContextMenu1.MenuItems.Add("-");
            ContextMenu1.MenuItems.Add("&Salir", new EventHandler(this.Salir_Click));

            notifyIcon2.ContextMenu = ContextMenu1;

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            timer1.Stop();

            string nroLicencia = "";
            string ambientecfg = "";
            string perfil = "";
            string inRun = "No";
            string installDir = Directory.GetCurrentDirectory();

            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
                nroLicencia = key.GetValue("Licencia").ToString();
                ambientecfg = key.GetValue("Ambientes").ToString();
                inRun = key.GetValue("InRun").ToString();
                perfil = key.GetValue("Perfil").ToString();
                installDir = key.GetValue("InstallDir").ToString();
                key.Close();
            }
            catch (Exception )
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate");
                key.SetValue("Licencia", nroLicencia);
                key.SetValue("Ambientes", ambientecfg);
                key.SetValue("InRun", inRun);
                key.SetValue("Perfil", perfil);
                key.SetValue("InstallDir", installDir);
                key.Close();
            }

            string regUI = Path.Combine(Path.GetTempPath(), "regUI.bat");
            string exe = Path.Combine(installDir, "WinPerUpdateUI.exe");
            
            int intentos = 0;
            var keyRun = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate");
            while (intentos < 3 && inRun.Equals("No"))
            {
                try
                {
                    if (File.Exists(regUI)) File.Delete(regUI);
                    if (!File.Exists(regUI))
                    {
                        File.WriteAllLines(regUI, new string[] {
                        "@echo off",
                        "cd %windir%\\system32",
                        "REG ADD \"HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\" /v  WinperUpdate /t REG_SZ /d \"" +exe+"\"  /f >> regUI.log"
                        });
                    }
                    var pas = Utils.ShowDialogInput(string.Format("Se procederá a configurar WinperUpdate en el arranque de Windows.\nEscriba la clave para el usuario {0}", Environment.UserName), "Clave de Usuario", true);

                    if (string.IsNullOrEmpty(pas))
                    {
                        intentos++;
                        MessageBox.Show(string.Format("No ha escrito ninguna clave, vuelva a intentarlo (Intento {0}/3)", intentos), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        var sec = new System.Security.SecureString();
                        for (int i = 0; i < pas.Length; i++)
                        {
                            sec.AppendChar(pas.ElementAt(i));
                        }

                        Process.Start(regUI, Environment.UserName, sec, Environment.UserDomainName);
                        inRun = "Si";
                        keyRun.SetValue("InRun", inRun);
                        var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                        key.SetValue("WinperUpdate", exe);
                        key.Close();
                        MessageBox.Show("WinperUpdate fue configurado al inicio de Windows correctamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    intentos++;
                    MessageBox.Show(string.Format("{0} (Intento {1}/3)", ex.Message, intentos), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (intentos >= 3 && inRun.Equals("No"))
                {
                    MessageBox.Show("El numero de intentos sobrepaso el límite.\n\nWinperUpdate no se pudo configurar al inicio de Windows. Esto NO afectará el funcionamiento de WinperUpdate", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            } 
            keyRun.Close();

            if (!string.IsNullOrEmpty(nroLicencia))
            {
                string server = ConfigurationManager.AppSettings["server"];
                string port = ConfigurationManager.AppSettings["port"];

                try
                {
                    string json = Utils.StrSendMsg(server, int.Parse(port), "checklicencia#" + nroLicencia + "#");
                    cliente = JsonConvert.DeserializeObject<ClienteBo>(json);
                    if (cliente != null)
                    {
                        json = Utils.StrSendMsg(server, int.Parse(port), "ambientes#" + cliente.Id.ToString() + "#");

                        ContextMenu1.MenuItems.Clear();

                        MenuItem addDevice = new MenuItem("&Estado de la Versión");
                        addDevice.Enabled = true;
                        ambientes = JsonConvert.DeserializeObject<List<AmbienteBo>>(json);
                        foreach (var ambiente in ambientes)
                        {
                            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate\"+ambiente.Nombre);
                            string dirwp = key.GetValue("DirWinper") == null ? "" : key.GetValue("DirWinper").ToString();
                            if (Directory.Exists(dirwp))
                            {
                                addDevice.MenuItems.Add(new MenuItem(ambiente.Nombre, new EventHandler(this.Restaurar_Click)));
                            }
                            key.Close();
                        }
                        

                        /*Obtiene los modulos contratados del cliente con sus respectivos componentes*/
                        json = Utils.StrSendMsg(server, int.Parse(port), "modulos#" + cliente.Id + "#");
                        Utils.ModulosContratados = JsonConvert.DeserializeObject<List<ModuloBo>>(json);

                        ContextMenu1.MenuItems.Add(addDevice);

                        ContextMenu1.MenuItems.Add("Configurar Ambiente y Licencia", new EventHandler(this.Ambiente_Click));
                        ContextMenu1.MenuItems[1].Enabled = true;

                        ContextMenu1.MenuItems.Add("-");
                        ContextMenu1.MenuItems.Add("&Acerca de...", new EventHandler(this.AcercaDe_Click));
                        ContextMenu1.MenuItems[2].DefaultItem = true;

                        ContextMenu1.MenuItems.Add("-");
                        ContextMenu1.MenuItems.Add("&Salir", new EventHandler(this.Salir_Click));

                        notifyIcon2.ContextMenu = ContextMenu1;
                        timer1.Start();
                        timerUI.Start();
                    }
                }
                catch (Exception ex)
                {
                    if (ContextMenu1.MenuItems.Count > 0)
                    {
                        ContextMenu1.MenuItems[0].Enabled = false;
                        ContextMenu1.MenuItems[1].Enabled = false;
                    }
                    else
                    {
                        ContextMenu1.MenuItems.Add("Configurar Ambiente y Licencia", new EventHandler(this.Ambiente_Click));
                        ContextMenu1.MenuItems[0].Enabled = false;

                        ContextMenu1.MenuItems.Add("-");
                        ContextMenu1.MenuItems.Add("&Acerca de...", new EventHandler(this.AcercaDe_Click));
                        ContextMenu1.MenuItems[1].DefaultItem = true;

                        ContextMenu1.MenuItems.Add("-");
                        ContextMenu1.MenuItems.Add("&Salir", new EventHandler(this.Salir_Click));
                    }
                    MessageBox.Show("Winper Update no tiene conexión con el servidor central");
                }
            }            
        }

        private void Ambiente_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            var form = new Ambiente();
            form.ShowDialog();

            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
                string nroLicencia = key.GetValue("Licencia").ToString();
                string ambientecfg = key.GetValue("Ambientes").ToString();
                key.Close();

                if (!string.IsNullOrEmpty(nroLicencia))
                {
                    string server = ConfigurationManager.AppSettings["server"];
                    string port = ConfigurationManager.AppSettings["port"];

                    try
                    {
                        string json = Utils.StrSendMsg(server, int.Parse(port), "checklicencia#" + nroLicencia + "#");
                        cliente = JsonConvert.DeserializeObject<ClienteBo>(json);
                        if (cliente != null)
                        {
                            json = Utils.StrSendMsg(server, int.Parse(port), "ambientes#" + cliente.Id.ToString() + "#");
                            foreach (var ambiente in JsonConvert.DeserializeObject<List<AmbienteBo>>(json))
                            {
                                if (ambientecfg.Contains(ambiente.Nombre))
                                {
                                    ambientes.Add(ambiente);
                                }
                            }
                            ContextMenu1.MenuItems[0].Enabled = true;
                            ContextMenu1.MenuItems[2].DefaultItem = false;
                            ContextMenu1.MenuItems[0].DefaultItem = true;
                            timer1.Start();
                        }
                    }
                    catch (Exception )
                    {
                        ContextMenu1.MenuItems[0].Enabled = false;
                        ContextMenu1.MenuItems[1].Enabled = false;
                        MessageBox.Show("Winper Update no tiene conexión con el servidor central");
                    }
                }

            }
            catch (Exception ) { };
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown)
            {
                var res = MessageBox.Show("¿Está seguro que desea cerrar WinperUpdate?", "CONFIRME", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ServerInAccept)
                {
                    // TODO: Insert monitoring activities here.

                    string server = ConfigurationManager.AppSettings["server"];
                    string port = ConfigurationManager.AppSettings["port"];
                    string dirTmp = Path.GetTempPath();

                    // 1.- Verificamos versiones
                    foreach (var item in ambientes)
                    {
                        var versiones = new List<VersionBo>();
                        string json = Utils.StrSendMsg(server, int.Parse(port), "getversiones#" + cliente.Id.ToString() + "#" + item.idAmbientes.ToString() + "#");
                        versiones = JsonConvert.DeserializeObject<List<VersionBo>>(json);
                        if (versiones != null)
                        {
                            var release = versiones.SingleOrDefault(x => x.Estado == 'P');
                            if (release != null)
                            {
                                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate\" + item.Nombre);
                                string nroVersion = key.GetValue("Version") == null ? "" : key.GetValue("Version").ToString();
                                var DirWinper = key.GetValue("DirWinper") == null ? "" : key.GetValue("DirWinper").ToString();
                                if (string.IsNullOrEmpty(DirWinper)) continue;
                                if (!System.IO.Directory.Exists(DirWinper)) continue;
                                if (nroVersion.Equals(release.Release)) continue;  // se da cuenta de que ya tengo la version

                                string dirTmpversion = dirTmp + (dirTmp.EndsWith("\\") ? "" : "\\");
                                dirTmpversion += "WinPer";
                                if (!Directory.Exists(dirTmpversion))
                                {
                                    Directory.CreateDirectory(dirTmpversion);
                                }

                                dirTmpversion += ("\\" + item.Nombre);
                                if (!Directory.Exists(dirTmpversion))
                                {
                                    Directory.CreateDirectory(dirTmpversion);
                                }
                                dirTmpversion += ("\\" + release.Release);
                                if (!Directory.Exists(dirTmpversion))
                                {
                                    Directory.CreateDirectory(dirTmpversion);
                                }

                                string nameIntalador = dirTmpversion + "\\" + release.Instalador;


                                worker.RunWorkerAsync(new object[]
                                {
                                nameIntalador,
                                release,
                                server,
                                port,
                                item,
                                nroVersion
                                });

                                key.Close();
                            }
                        }
                    }

                    #region 2.- Verificamos script que se deben ejecutar
                    // 2.- Verificamos script que se deben ejecutar
                    Microsoft.Win32.RegistryKey key2 = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
                    string perfil = key2.GetValue("Perfil").ToString();
                    string ambiente = key2.GetValue("Ambientes").ToString();
                    key2.Close();

                    if (perfil.Equals("Administrador") || perfil.Equals("DBA"))
                    {
                        int idPerfil = perfil.Equals("Administrador") ? 11 : 12;
                        var tareas = new List<TareaBo>();
                        string mensaje = "tareas#" + cliente.Id.ToString() + "#" + idPerfil.ToString() + "#";
                        string jsont = Utils.StrSendMsg(server, int.Parse(port), mensaje);
                        tareas = JsonConvert.DeserializeObject<List<TareaBo>>(jsont);

                        foreach (var tarea in tareas.Where(x => x.Estado == 0 && x.LengthFile > 0))
                        {
                            if (ambiente.Contains(tarea.Ambientes.Nombre))
                            {
                                ServerInAccept = false;

                                jsont = Utils.StrSendMsg(server, int.Parse(port), "getversionbyid#" + tarea.idVersion.ToString() + "#");
                                var release = JsonConvert.DeserializeObject<VersionBo>(jsont);

                                if (File.Exists(dirTmp + tarea.NameFile))
                                {
                                    File.Delete(dirTmp + tarea.NameFile);
                                }
                                FileStream stream = new FileStream(dirTmp + tarea.NameFile, FileMode.CreateNew, FileAccess.Write);
                                BinaryWriter writer = new BinaryWriter(stream);

                                int nPosIni = 0;
                                while (nPosIni < tarea.LengthFile)
                                {
                                    long largoMax = tarea.LengthFile - nPosIni;
                                    if (largoMax > SIZEBUFFER) largoMax = SIZEBUFFER;
                                    string newmsg = string.Format("getfile#{0}\\{1}#{2}#{3}#", release.Release, tarea.NameFile, nPosIni, largoMax);
                                    var buffer = Utils.SendMsg(server, int.Parse(port), newmsg, SIZEBUFFER);
                                    writer.Write(buffer, 0, buffer.Length);

                                    nPosIni += buffer.Length;
                                }

                                writer.Close();
                                stream.Close();

                                // ejecutamos archivo en base e datos
                                EjecutarQuery(tarea.idTareas,
                                              dirTmp + tarea.NameFile,
                                              tarea.Ambientes.Instancia + "\\" + tarea.Ambientes.ServerBd,
                                              tarea.Ambientes.NomBd,
                                              tarea.Ambientes.UserDbo,
                                              tarea.Ambientes.PwdDbo);

                                ServerInAccept = true;
                            }
                        }
                    }
                    #endregion

                }
            
            }
            catch (Exception ex)
            {
                Utils.RegistrarLog("TimerTick.log", ex.ToString());
            }
        }

        private Boolean EjecutarQuery (int idTarea, string nameFile, string Server, string DataBase, string User, string Password)
        {
            string ConnectionStr = string.Format("Database={0};Server={1};User={2};Password={3};Connect Timeout=200;Integrated Security=;", DataBase, Server, User, Password);
            string server = ConfigurationManager.AppSettings["server"];
            string port = ConfigurationManager.AppSettings["port"];
            SqlConnection conn = new SqlConnection();
            try
            {
                var sr = File.OpenText(nameFile);
                string query = sr.ReadToEnd();
                sr.Close();

                // split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(query, @"^\s*GO\s*$",
                                         RegexOptions.Multiline | RegexOptions.IgnoreCase);

                conn = new SqlConnection(ConnectionStr);
                conn.Open();

                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, conn))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }


                //var comm = conn.CreateCommand();
                //comm.CommandType = CommandType.Text;
                //comm.CommandText = query;
                //comm.ExecuteNonQuery();

                conn.Close();

                //ProcessMsg.Tareas.SetEstadoTarea(idTarea, 1, "");
                Utils.StrSendMsg(server, int.Parse(port), "settarea#" + idTarea.ToString() + "#1#OK#");

                return true;
            }
            catch (Exception ex)
            {
                conn.Close();

                var msg = "Error al abrir la conexion" + ex.Message + ".";
                //ProcessMsg.Tareas.SetEstadoTarea(idTarea, 2, ex.Message);
                Utils.StrSendMsg(server, int.Parse(port), "settarea#" + idTarea.ToString() + "#2#" + ex.Message + "#");
                Utils.RegistrarLog("EjecutarQuery.log", ex.ToString());
                //throw new Exception(msg, ex);
            }

            return false;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var obj = (object[])e.Argument;
            var nameIntalador = (String)obj[0];
            var release = (VersionBo)obj[1];
            var server = (String)obj[2];
            var port = (String)obj[3];
            var item = (AmbienteBo)obj[4];
            var nroVersion = (String)obj[5];
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate\" + item.Nombre);
            var ins = new FileInfo(nameIntalador);
            long lengthInstalador = ins.Exists ? ins.Length : 0;

            if (!File.Exists(nameIntalador) || (File.Exists(nameIntalador) && lengthInstalador < release.Length)) //new
            {
                ServerInAccept = false;

                FileStream stream = null;
                if (!File.Exists(nameIntalador))
                {
                    stream = new FileStream(nameIntalador, FileMode.CreateNew, FileAccess.Write);
                }
                else
                {
                    stream = new FileStream(nameIntalador, FileMode.Append, FileAccess.Write);
                }

                BinaryWriter writer = new BinaryWriter(stream);

                long nPosIni = lengthInstalador;//new
                while (nPosIni < release.Length)
                {
                    long largoMax = release.Length - nPosIni;
                    if (largoMax > SIZEBUFFER) largoMax = SIZEBUFFER;
                    string newmsg = string.Format("getfile#{0}\\Output\\{1}#{2}#{3}#", release.Release, release.Instalador, nPosIni, largoMax);
                    var buffer = Utils.SendMsg(server, int.Parse(port), newmsg, SIZEBUFFER);
                    //writer.Write(buffer);
                    writer.Write(buffer, 0, buffer.Length);
                    worker.ReportProgress(int.Parse(((nPosIni * 100) / release.Length).ToString()));
                    nPosIni += buffer.Length;
                }

                writer.Close();
                stream.Close();
                ServerInAccept = true;

            }

            // Caso en que existe instalador
            // 1.- Sacar largo del archivo
            // 2.- Comparar con release.Length
            // 3 id son igrales entonces hacer lo que hace hoy
            if (lengthInstalador == release.Length)
            {
                if (!nroVersion.Equals(release.Release))
                {
                    // Avisamos llegada de nueva versión
                    ambienteUpdate = item.Nombre;
                    notifyIcon2.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon2.BalloonTipText = "Existe una nueva versión de winper para el ambiente " + item.Nombre;
                    notifyIcon2.BalloonTipTitle = "Winper Update";

                    notifyIcon2.ShowBalloonTip(5000);
                    // Actualizamos la versión en la registry
                    key.SetValue("Version", release.Release);
                    key.SetValue("Status", "");
                }
            }

            key.Close();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(string.Format("Ocurrio un error durante la recepción del archivo.\n\n{0}", e.Error.Message), "ERROR WinperUpdate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Utils.RegistrarLog("WorkerDoWork.log", e.Error.ToString());
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Se ha cancelado la recepcion del archivo", "CANCELADO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                notifyIcon2.Text = "Winper Update";
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            notifyIcon2.Text = string.Format("Winper Update {0}%",e.ProgressPercentage);
            
        }

        private void timerUI_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ServerInAccept)
                {
                    string nroLicencia = "";
                    string Perfil = "";
                    Microsoft.Win32.RegistryKey key = null;
                    try
                    {
                        key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
                        nroLicencia = key.GetValue("Licencia").ToString();
                        Perfil = key.GetValue("Perfil").ToString();
                    }
                    catch (Exception)
                    {
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate");
                        key.SetValue("Licencia", nroLicencia);
                        key.SetValue("Perfil", Perfil);
                    }
                    
                    key.Close();
                    if (!string.IsNullOrEmpty(nroLicencia) && Perfil.Equals("Administrador"))
                    {
                        var nameIntalador = Path.Combine(Path.GetTempPath(), "SetUpdateUI.exe");
                       
                        string server = ConfigurationManager.AppSettings["server"];
                        string port = ConfigurationManager.AppSettings["port"];

                        string json = Utils.StrSendMsg(server, int.Parse(port), "checkupui#" + nroLicencia + "#");
                        var uui = JsonConvert.DeserializeObject<UpdateUI>(json);
                        bool FileOk = true;

                        if (uui != null && uui.Lista.Count > 0)
                        {
                            foreach(FileInfo fl in new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles().ToList())
                            {
                                var exist = uui.Lista.Exists(f => f.Nombre.Equals(fl.Name));
                                if (exist)
                                {
                                    var fil = uui.Lista.SingleOrDefault(x => x.Nombre.Equals(fl.Name));
                                    var a = FileVersionInfo.GetVersionInfo(fl.FullName);
                                    if (FileVersionInfo.GetVersionInfo(fl.FullName).FileVersion != null)
                                    {
                                        if (!fil.VersionArchivo.Equals(FileVersionInfo.GetVersionInfo(fl.FullName).FileVersion))
                                        {
                                            FileOk = false;
                                        }
                                    }
                                }
                            }
                            if (!FileOk)
                            {
                                ServerInAccept = false;
                                
                                long lengthInstalador = 0;
                                FileStream stream = null;
                                if (!File.Exists(nameIntalador))
                                {
                                    stream = new FileStream(nameIntalador, FileMode.CreateNew, FileAccess.Write);
                                }
                                else
                                {
                                    lengthInstalador = new FileInfo(nameIntalador).Length;
                                    stream = new FileStream(nameIntalador, FileMode.Append, FileAccess.Write);
                                }

                                BinaryWriter writer = new BinaryWriter(stream);

                                long nPosIni = lengthInstalador;//new
                                while (nPosIni < uui.SetupLength)
                                {
                                    long largoMax = uui.SetupLength - nPosIni;
                                    if (largoMax > SIZEBUFFER) largoMax = SIZEBUFFER;
                                    string newmsg = string.Format("downsetup#{0}#{1}#", nPosIni, largoMax);
                                    var buffer = Utils.SendMsg(server, int.Parse(port), newmsg, SIZEBUFFER);
                                    writer.Write(buffer, 0, buffer.Length);
                                    nPosIni += buffer.Length;
                                }
                                
                                writer.Close();
                                stream.Close();

                                lengthInstalador = new FileInfo(nameIntalador).Length;
                                if (uui.SetupLength == lengthInstalador)
                                {
                                    if (Utils.isCentralizado)
                                    {
                                        var argument = string.Format("/DIR=\"{0}\" /VERYSILENT /SUPPRESSMSGBOXES /NORESTART /NOCANCEL",Directory.GetCurrentDirectory());
                                        SvcWPUI.Start(new string[] { nameIntalador, argument });
                                    }
                                    else
                                    {
                                        Process.Start(nameIntalador);
                                    }
                                }

                            }
                            else
                            {
                                if (File.Exists(nameIntalador))
                                {
                                    File.Delete(nameIntalador);
                                }
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ServerInAccept = true;
                Utils.RegistrarLog("UPUI_InvalidOperationException.log", ex.ToString());
            }
            catch(Exception ex)
            {
                ServerInAccept = true;
                Utils.RegistrarLog("UPUI.log", ex.ToString());
                MessageBox.Show("Ocurrió un error en timerUI_Tick, revise el log 'UPUI.log'", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
