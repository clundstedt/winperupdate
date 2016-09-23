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

namespace WinPerUpdateUI
{
    public partial class FormPrincipal : Form
    {
        const int SIZEBUFFER = 16384;
        ContextMenu ContextMenu1 = new ContextMenu();
        private bool ServerInAccept = true;
        private bool TreePoblado = false;

        static byte[] SendMsg(string ipServer, int port, string message)
        {
            string output = "";

            try
            {
                // Create a TcpClient.
                // The client requires a TcpServer that is connected
                // to the same address specified by the server and port
                // combination.
                TcpClient client = new TcpClient(ipServer, port);
                //eventLog1.WriteEntry("Servidor acepta coneccion ...");

                // Get a client stream for reading and writing.
                // Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Translate the passed message into ASCII and store it as a byte array.
                Byte[] data = new Byte[SIZEBUFFER];
                data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                output = "Sent: " + message;
                //eventLog1.WriteEntry(output);

                // Buffer to store the response bytes.
                data = new Byte[SIZEBUFFER];

                // String to store the response ASCII representation.
                //String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                Byte[] responseData = new Byte[bytes];

                for (int i = 0; i < bytes; i++) responseData[i] = data[i];

                //output = string.Format("Received ({0}): {1} ", bytes, responseData);
                output = string.Format("Byte Received : {0}", bytes);
                //eventLog1.WriteEntry(output);

                // Close everything.
                stream.Close();
                client.Close();

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                output = "ArgumentNullException: " + e;
                //eventLog1.WriteEntry(output);
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                //eventLog1.WriteEntry(output);
            }

            return null;
        }

        static string StrSendMsg(string ipServer, int port, string message)
        {
            string output = "";

            try
            {
                // Create a TcpClient.
                // The client requires a TcpServer that is connected
                // to the same address specified by the server and port
                // combination.
                TcpClient client = new TcpClient(ipServer, port);
                //eventLog1.WriteEntry("Servidor acepta coneccion ...");

                // Get a client stream for reading and writing.
                // Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Translate the passed message into ASCII and store it as a byte array.
                Byte[] data = new Byte[1024];
                data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                output = "Sent: " + message;
                //eventLog1.WriteEntry(output);

                // Buffer to store the response bytes.
                data = new Byte[SIZEBUFFER];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                output = string.Format("Received ({0}): {1} ", bytes, responseData);
                //eventLog1.WriteEntry(output);

                // Close everything.
                stream.Close();
                client.Close();

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                output = "ArgumentNullException: " + e;
                //eventLog1.WriteEntry(output);
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                //eventLog1.WriteEntry(output);
            }

            return null;
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
            //this.Close();
            Application.Exit();
        }

        private void Restaurar_Click(object sender, System.EventArgs e)
        {
            //' Restaurar por si se minimizó
            //' Este evento manejará tanto los menús Restaurar como el NotifyIcon.DoubleClick
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        private void AcercaDe_Click(object sender, System.EventArgs e)
        {
            //' Mostrar la información del autor, versión, etc.
            //MessageBox.Show(Application.ProductName + " v" + Application.ProductVersion, "Prueba 2 de NotifyIcon en C#");
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
            string server = ConfigurationManager.AppSettings["server"];
            string port = ConfigurationManager.AppSettings["port"];
            string dirTmp = Path.GetTempPath();
            dirTmp += dirTmp.EndsWith("\\") ? "" : "\\";

            var versiones = new List<VersionBo>();
            string json = StrSendMsg(server, int.Parse(port), "getversiones#");
            versiones = JsonConvert.DeserializeObject<List<VersionBo>>(json);
            if (versiones != null)
            {
                var release = versiones.SingleOrDefault(x => x.Estado == 'P');
                if (release != null)
                {
                    if (File.Exists(dirTmp + release.Instalador)) {
                        label2.Text = string.Format("Ya se encuentra disponible la versión {0} de Winper. Esta versión contiene las siguientes actualizaciones:", release.Release);
                        treeModulos.Nodes.Clear();

                        treeModulos.Nodes.Add("Winper V " + release.Release);
                        string modulo = "";
                        foreach (var componente in release.Componentes)
                        {
                            if (!modulo.Equals(componente.Modulo))
                            {
                                modulo = componente.Modulo;
                                treeModulos.Nodes[0].Nodes.Add(modulo);
                            }
                        }

                        TreePoblado = true;
                    }

                    listView1.Clear();

                    listView1.Columns.Add("Número", 80, HorizontalAlignment.Left);
                    listView1.Columns.Add("Fecha", 120, HorizontalAlignment.Left);
                    listView1.Columns.Add("Comentario", 290, HorizontalAlignment.Left);

                    if (versiones != null)
                    {
                        var item = new ListViewItem(release.Release);
                        item.SubItems.Add(release.FechaFmt);
                        item.SubItems.Add(release.Comentario);

                        listView1.Items.Add(item);
                    }
                }
            }

        }

        private void notifyIcon2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            ContextMenu1.MenuItems.Add("&Restaurar", new EventHandler(this.Restaurar_Click));
            ContextMenu1.MenuItems[0].DefaultItem = true;
            //'
            //' Añadimos un separador
            ContextMenu1.MenuItems.Add("-");
            //' Añadimos el elemento Acerca de...
            ContextMenu1.MenuItems.Add("&Acerca de...", new EventHandler(this.AcercaDe_Click));
            //' Añadimos otro separador
            ContextMenu1.MenuItems.Add("-");
            //' Añadimos la opción de salir
            ContextMenu1.MenuItems.Add("&Salir", new EventHandler(this.Salir_Click));

            notifyIcon2.ContextMenu = ContextMenu1;

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
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

                var versiones = new List<VersionBo>();
                string json = StrSendMsg(server, int.Parse(port), "getversiones#");
                versiones = JsonConvert.DeserializeObject<List<VersionBo>>(json);
                if (versiones != null)
                {
                    var release = versiones.SingleOrDefault(x => x.Estado == 'P');
                    if (release != null)
                    {
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
                                var buffer = SendMsg(server, int.Parse(port), newmsg);
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

                            label2.Text = string.Format("Ya se encuentra disponible la versión {0} de Winper. Esta versión contiene las siguientes actualizaciones:", release.Release);
                            treeModulos.Nodes.Clear();

                            treeModulos.Nodes.Add("Winper V " + release.Release);
                            string modulo = "";
                            foreach (var componente in release.Componentes)
                            {
                                if (!modulo.Equals(componente.Modulo))
                                {
                                    modulo = componente.Modulo;
                                    treeModulos.Nodes[0].Nodes.Add(modulo);
                                }
                            }

                            //WindowState = FormWindowState.Normal;

                            ServerInAccept = true;
                            TreePoblado = true;
                        }
                        else if (!TreePoblado)
                        {
                            label2.Text = string.Format("Ya se encuentra disponible la versión {0} de Winper. Esta versión contiene las siguientes actualizaciones:", release.Release);
                            treeModulos.Nodes.Clear();

                            treeModulos.Nodes.Add("Winper V " + release.Release);
                            string modulo = "";
                            foreach (var componente in release.Componentes)
                            {
                                if (!modulo.Equals(componente.Modulo))
                                {
                                    modulo = componente.Modulo;
                                    treeModulos.Nodes[0].Nodes.Add(modulo);
                                }
                            }

                            TreePoblado = true;
                        }
                    }
                }
            }
        }

        private void treeModulos_Click(object sender, EventArgs e)
        {

        }

        private void treeModulos_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string server = ConfigurationManager.AppSettings["server"];
            string port = ConfigurationManager.AppSettings["port"];

            var versiones = new List<VersionBo>();
            string json = StrSendMsg(server, int.Parse(port), "getversiones#");
            versiones = JsonConvert.DeserializeObject<List<VersionBo>>(json);

            string modulo = e.Node.Text;
            var padre = e.Node.Parent;
            if (padre == null)
            {
                listView1.Clear();

                listView1.Columns.Add("Número", 80, HorizontalAlignment.Left);
                listView1.Columns.Add("Fecha", 120, HorizontalAlignment.Left);
                listView1.Columns.Add("Comentario", 290, HorizontalAlignment.Left);

                if (versiones != null)
                {
                    string[] token = modulo.Split(new Char[] { ' ' });

                    var release = versiones.SingleOrDefault(x => x.Release.Equals(token[2]));
                    if (release != null)
                    {
                        var item = new ListViewItem(release.Release);
                        item.SubItems.Add(release.FechaFmt);
                        item.SubItems.Add(release.Comentario);

                        listView1.Items.Add(item);
                    }
                }
            }
            else
            {
                listView1.Clear();

                listView1.Columns.Add("Nombre", 120, HorizontalAlignment.Left);
                listView1.Columns.Add("Fecha", 120, HorizontalAlignment.Left);
                listView1.Columns.Add("Versión", 70, HorizontalAlignment.Left);
                listView1.Columns.Add("Comentario", 290, HorizontalAlignment.Left);

                if (versiones != null)
                {
                    string[] token = padre.Text.Split(new Char[] { ' ' });

                    var release = versiones.SingleOrDefault(x => x.Release.Equals(token[2]));

                    foreach (var componente in release.Componentes.Where(x => x.Modulo.Equals(modulo)))
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

        private void btnInstalar_Click(object sender, EventArgs e)
        {
            string dirTmp = Path.GetTempPath();
            dirTmp += dirTmp.EndsWith("\\") ? "" : "\\";

            string server = ConfigurationManager.AppSettings["server"];
            string port = ConfigurationManager.AppSettings["port"];

            var versiones = new List<VersionBo>();
            string json = StrSendMsg(server, int.Parse(port), "getversiones#");
            versiones = JsonConvert.DeserializeObject<List<VersionBo>>(json);

            for (int i = 0; i < treeModulos.Nodes.Count; i++)
            {
                string[] token = treeModulos.Nodes[i].Text.Split(new Char[] { ' ' });
                var release = versiones.SingleOrDefault(x => x.Release.Equals(token[2]));

                if (release != null)
                {
                    if (File.Exists(dirTmp + release.Instalador))
                    {
                        string Command = dirTmp + release.Instalador;

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
    }
}
