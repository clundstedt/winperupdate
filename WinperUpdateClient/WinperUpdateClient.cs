using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Text;
using System.Configuration;
using ProcessMsg.Model;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

namespace WinperUpdateClient
{

    public partial class WinperUpdateClient : ServiceBase
    {
        private bool ServerInAccept = false;
        const int SIZEBUFFER = 16384;
        System.Timers.Timer timer = new System.Timers.Timer();

        static byte[] SendMsg(string ipServer, int port, string message, EventLog eventLog1)
        {
            string output = "";

            try
            {
                // Create a TcpClient.
                // The client requires a TcpServer that is connected
                // to the same address specified by the server and port
                // combination.
                TcpClient client = new TcpClient(ipServer, port);
                eventLog1.WriteEntry("Servidor acepta coneccion ...");

                // Get a client stream for reading and writing.
                // Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Translate the passed message into ASCII and store it as a byte array.
                Byte[] data = new Byte[SIZEBUFFER];
                data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                output = "Sent: " + message;
                eventLog1.WriteEntry(output);

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
                eventLog1.WriteEntry(output);

                // Close everything.
                stream.Close();
                client.Close();

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                output = "ArgumentNullException: " + e;
                eventLog1.WriteEntry(output);
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                eventLog1.WriteEntry(output);
            }

            return null;
        }

        static string StrSendMsg(string ipServer, int port, string message, EventLog eventLog1)
        {
            string output = "";

            try
            {
                // Create a TcpClient.
                // The client requires a TcpServer that is connected
                // to the same address specified by the server and port
                // combination.
                TcpClient client = new TcpClient(ipServer, port);
                eventLog1.WriteEntry("Servidor acepta coneccion ...");

                // Get a client stream for reading and writing.
                // Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Translate the passed message into ASCII and store it as a byte array.
                Byte[] data = new Byte[1024];
                data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                output = "Sent: " + message;
                eventLog1.WriteEntry(output);

                // Buffer to store the response bytes.
                data = new Byte[SIZEBUFFER];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                output = string.Format("Received ({0}): {1} ", bytes, responseData);
                eventLog1.WriteEntry(output);

                // Close everything.
                stream.Close();
                client.Close();

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                output = "ArgumentNullException: " + e;
                eventLog1.WriteEntry(output);
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                eventLog1.WriteEntry(output);
            }

            return null;
        }

        public WinperUpdateClient()
        {
            InitializeComponent();
            //eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("Winper Update Log"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Winper Update Log", "WinperUpdateLog");
            }
            eventLog1.Source = "Winper Update Log";
            eventLog1.Log = "WinperUpdateLog";
        }


        protected override void OnStart(string[] args)
        {

            eventLog1.WriteEntry("Start WinperUpdateClient v1.0.13");
            ServerInAccept = true;

            // Set up a timer to trigger every minute.
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Stop WinperUpdateClient");
            ServerInAccept = false;
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            if (ServerInAccept)
            {
                // TODO: Insert monitoring activities here.
                string server = ConfigurationManager.AppSettings["server"];
                string port = ConfigurationManager.AppSettings["port"];
                string dirTmp = Path.GetTempPath();
                dirTmp += dirTmp.EndsWith("\\") ? "" : "\\";

                eventLog1.WriteEntry("server: " + server);
                eventLog1.WriteEntry("port: " + port);
                eventLog1.WriteEntry("dirTmp: " + dirTmp);

                eventLog1.WriteEntry("Preguntamos al servidor si existen versiones ...");

                var versiones = new List<VersionBo>();
                string json = StrSendMsg(server, int.Parse(port), "getversiones#", eventLog1);
                versiones = JsonConvert.DeserializeObject<List<VersionBo>>(json);
                if (versiones != null)
                {
                    var release = versiones.SingleOrDefault(x => x.Estado == 'P');
                    if (release != null)
                    {
                        if (release.Instalador != null)
                        {

                            if (!File.Exists(dirTmp + release.Instalador))
                            {
                                ServerInAccept = false;

                                eventLog1.WriteEntry("Se detecta paquete de instalacion: " + release.Instalador);
                                eventLog1.WriteEntry("Se inicia bajada a: " + dirTmp);

                                FileStream stream = new FileStream(dirTmp + release.Instalador, FileMode.CreateNew, FileAccess.Write);
                                BinaryWriter writer = new BinaryWriter(stream);

                                int nPosIni = 0;
                                while (nPosIni < release.Length)
                                {
                                    long largoMax = release.Length - nPosIni;
                                    if (largoMax > SIZEBUFFER) largoMax = SIZEBUFFER;
                                    string newmsg = string.Format("getfile#{0}\\Output\\{1}#{2}#{3}#", release.Release, release.Instalador, nPosIni, largoMax);
                                    var buffer = SendMsg(server, int.Parse(port), newmsg, eventLog1);
                                    writer.Write(buffer, 0, buffer.Length);

                                    nPosIni += SIZEBUFFER;
                                }

                                writer.Close();
                                stream.Close();

                                // Avisamos que ha llegado una nueva versión de winper
                                string Command = Application.ExecutablePath.Replace("WinperUpdateClient.exe", "WinPerUpdateUI.exe");
                                eventLog1.WriteEntry("Se inicia Interfaz Gráfica de Winper Update: " + Command);

                                Process myProcess = new Process();
                                myProcess.StartInfo.FileName = Command;
                                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                                myProcess.StartInfo.RedirectStandardError = true;

                                myProcess.Start();

                                StreamReader myStreamReader = myProcess.StandardError;
                                // Read the standard error of net.exe and write it on to console.
                                eventLog1.WriteEntry("Error mostrado por Interfaz Gráfica de Winper Update: " + myStreamReader.ReadLine());
                                myProcess.Close();

                                ServerInAccept = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
