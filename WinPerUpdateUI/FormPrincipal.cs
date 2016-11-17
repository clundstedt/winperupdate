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
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WinPerUpdateUI
{
    public partial class FormPrincipal : Form
    {
        const int SIZEBUFFER = 16384;
        ContextMenu ContextMenu1 = new ContextMenu();
        private bool ServerInAccept = true;
        private ClienteBo cliente = new ClienteBo();
        private List<AmbienteBo> ambientes = new List<AmbienteBo>();

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
            //this.Close();
            Application.Exit();
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
            //ShowInTaskbar = true;
            //WindowState = FormWindowState.Normal;
            var form = new frmVersiones();
            form.ShowDialog();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
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

            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinperUpdate");
                nroLicencia = key.GetValue("Licencia").ToString();
                ambientecfg = key.GetValue("Ambientes").ToString();
                key.Close();
            }
            catch (Exception ex)
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate");
                key.SetValue("Licencia", "");
                key.SetValue("Ambientes", "");
                key.Close();
            }

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
                        foreach (var ambiente in JsonConvert.DeserializeObject<List<AmbienteBo>>(json))
                        {
                            addDevice.MenuItems.Add(new MenuItem(ambiente.Nombre, new EventHandler(this.Restaurar_Click)));
                        }
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
                    }
                }
                catch (Exception ex)
                {
                    ContextMenu1.MenuItems[0].Enabled = false;
                    ContextMenu1.MenuItems[1].Enabled = false;
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
                    catch (Exception ex)
                    {
                        ContextMenu1.MenuItems[0].Enabled = false;
                        ContextMenu1.MenuItems[1].Enabled = false;
                        MessageBox.Show("Winper Update no tiene conexión con el servidor central");
                    }
                }

            }
            catch (Exception ex) { };
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ServerInAccept)
            {
                // TODO: Insert monitoring activities here.

                string server = ConfigurationManager.AppSettings["server"];
                string port = ConfigurationManager.AppSettings["port"];
                string dirTmp = Path.GetTempPath();
                dirTmp += dirTmp.EndsWith("\\") ? "" : "\\";

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
                            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinperUpdate");
                            string nroVersion = key.GetValue("Version").ToString();
                            if (nroVersion.Equals(release.Release)) return;

                            if (!File.Exists(dirTmp + release.Instalador))
                            {
                                ServerInAccept = false;

                                FileStream stream = new FileStream(dirTmp + release.Instalador, FileMode.CreateNew, FileAccess.Write);
                                BinaryWriter writer = new BinaryWriter(stream);

                                int nPosIni = 0;
                                while (nPosIni < release.Length)
                                {
                                    long largoMax = release.Length - nPosIni;
                                    if (largoMax > SIZEBUFFER) largoMax = SIZEBUFFER;
                                    string newmsg = string.Format("getfile#{0}\\Output\\{1}#{2}#{3}#", release.Release, release.Instalador, nPosIni, largoMax);
                                    var buffer = Utils.SendMsg(server, int.Parse(port), newmsg);
                                    writer.Write(buffer, 0, buffer.Length);

                                    nPosIni += SIZEBUFFER;
                                }

                                writer.Close();
                                stream.Close();

                                // Avisamos llegada de nueva versión
                                notifyIcon2.BalloonTipIcon = ToolTipIcon.Info;
                                notifyIcon2.BalloonTipText = "Existen una nueva versión de winper";
                                notifyIcon2.BalloonTipTitle = "Winper Update";

                                notifyIcon2.ShowBalloonTip(1000);

                                key.SetValue("Version", release.Release);
                                key.SetValue("Status", "");

                                ServerInAccept = true;
                            }
                            else
                            {
                                if (!nroVersion.Equals(release.Release))
                                {
                                    // Actualizamos la versión en la registry
                                    key.SetValue("Version", release.Release);
                                    key.SetValue("Status", "");
                                }
                            }

                            key.Close();
                        }
                    }
                }

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
                                var buffer = Utils.SendMsg(server, int.Parse(port), newmsg);
                                writer.Write(buffer, 0, buffer.Length);

                                nPosIni += SIZEBUFFER;
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

                //throw new Exception(msg, ex);
            }

            return false;
        }
    }
}
