using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using ProcessMsg;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;

namespace WinperUpdateServer
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public partial class WinperUpdateServer : ServiceBase
    {
        private int eventId = 0;
        private bool ServerInAccept = false;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        System.Timers.Timer timer = new System.Timers.Timer();

        public void StartListening(int port)
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    eventLog1.WriteEntry("Waiting for a connection...", EventLogEntryType.Information, ++eventId);
                    ServerInAccept = true;

                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                eventLog1.WriteEntry(e.ToString(), EventLogEntryType.Error, ++eventId);
            }

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.
                allDone.Set();

                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (SocketException e)
            {
                string output = "AcceptCallback SocketException: " + e.ToString();
                eventLog1.WriteEntry(output, EventLogEntryType.Error, ++eventId);
            }
            catch (Exception ex)
            {
                string output = "AcceptCallback Exception: " + ex.ToString();
                eventLog1.WriteEntry(output, EventLogEntryType.Error, ++eventId);
            }
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            string json = string.Empty;
            int idCliente = 0;

            try
            {
                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);
                eventLog1.WriteEntry(String.Format("Read {0} bytes from socket. \n", bytesRead), EventLogEntryType.Information, ++eventId);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();
                    eventLog1.WriteEntry("Read was: " + content, EventLogEntryType.Information, ++eventId);
                    if (content.EndsWith("#"))
                    {
                        // All the data has been read from the 
                        // client. Display it on the console.
                        //eventLog1.WriteEntry(String.Format("Read {0} bytes from socket. \n Data : {1}", content.Length, content), EventLogEntryType.Information, ++eventId);

                        string[] token = content.Split(new Char[] { '#' });
                        string dirVersiones = ConfigurationManager.AppSettings["dirVersiones"];
                        dirVersiones += dirVersiones.EndsWith(@"\") ? "" : @"\";

                        switch (token[0])
                        {
                            case "querys":
                                idCliente = int.Parse(token[1]);

                                break;

                            case "checklicencia": // checklicencia#nroLicencia
                                string nroLicencia = token[1];
                                var cliente = ProcessMsg.Cliente.GetClienteByLicencia(nroLicencia, eventLog1);

                                json = JsonConvert.SerializeObject(cliente);
                                Send(handler, json);
                                break;

                            case "ambientes": // ambientes#idCliente
                                idCliente = int.Parse(token[1]);
                                var ambientes = ProcessMsg.Ambiente.GetAmbientesByCliente(idCliente, eventLog1);

                                json = JsonConvert.SerializeObject(ambientes);
                                Send(handler, json);
                                break;

                            case "getversiones": // getversiones#idCliente
                                idCliente = int.Parse(token[1]);
                                var lista = ProcessMsg.Cliente.GetVersiones(idCliente, eventLog1);
                                foreach (var item in lista)
                                {
                                    if (!String.IsNullOrEmpty(item.Instalador))
                                    {
                                        string fileName = dirVersiones + item.Release + "\\Output\\" + item.Instalador;
                                        System.IO.FileInfo info = new System.IO.FileInfo(fileName);
                                        item.Length = info.Length;
                                    }
                                }

                                json = JsonConvert.SerializeObject(lista);
                                Send(handler, json);
                                break;

                            case "getversion": // getversiones#NumVersion
                                string release = token[1];
                                var version = ProcessMsg.Version.GetVersion(release, eventLog1);

                                json = JsonConvert.SerializeObject(version);
                                Send(handler, json);
                                break;

                            case "getmodulos":
                                //eventLog1.WriteEntry(String.Format("Call ListarVersiones('{0}','{1}')", token[1], dirVersiones), EventLogEntryType.Information, ++eventId);

                                //var lista = ProcessMsg.Version.ListarVersiones(token[1], dirVersiones, eventLog1);
                                var listaModulos = ProcessMsg.Version.GetModulosVersiones(int.Parse(token[1]), eventLog1);

                                json = JsonConvert.SerializeObject(listaModulos);
                                Send(handler, json);
                                break;

                            case "getcomponentes":
                                //var listaArchivos = ProcessMsg.Version.ListarDirectorio(token[1], dirVersiones, eventLog1);
                                var version = ProcessMsg.Version.GetVersiones(eventLog1).SingleOrDefault(x => x.IdVersion == int.Parse(token[1]));

                                var listaArchivos = ProcessMsg.Componente.GetComponentes(int.Parse(token[1]), token[2], eventLog1);

                                foreach (var archivo in listaArchivos)
                                {
                                    string fileName = dirVersiones + version.Release + "\\" + archivo.Name;
                                    System.IO.FileInfo info = new System.IO.FileInfo(fileName);
                                    archivo.Length = info.Length;
                                }

                                json = JsonConvert.SerializeObject(listaArchivos);
                                Send(handler, json);
                                break;

                            case "getfile":
                                var buffer = ProcessMsg.Version.DownloadFile(token[1], int.Parse(token[2]), int.Parse(token[3]), dirVersiones, eventLog1);
                                Send(handler, buffer);
                                break;

                            default:
                                // Echo the data back to the client.
                                Send(handler, content);
                                break;
                        }
                    }
                    else
                    {
                        // Not all data received. Get more.
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }
                }
            }
            catch (SocketException e)
            {
                string output = "ReadCallback SocketException: " + e.ErrorCode.ToString();
                eventLog1.WriteEntry(output, EventLogEntryType.Error, ++eventId);
            }
            catch (Exception ex)
            {
                string output = "ReadCallback Exception: " + ex.ToString();
                eventLog1.WriteEntry(output, EventLogEntryType.Error, ++eventId);
            }
        }

        private void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            eventLog1.WriteEntry(String.Format("Send {0} bytes from socket. \n", byteData.Length), EventLogEntryType.Information, ++eventId);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private void Send(Socket handler, byte[] byteData)
        {
            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                eventLog1.WriteEntry(string.Format("Sent {0} bytes to client.", bytesSent), EventLogEntryType.Information, ++eventId);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                eventLog1.WriteEntry(e.ToString(), EventLogEntryType.Error, ++eventId);
            }
        }

        public WinperUpdateServer()
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
            eventLog1.WriteEntry("Start WinperUpdateServer");
            ServerInAccept = false;

            // Set up a timer to trigger every minute.
            timer.Interval = 30000; // 30 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Stop WinperUpdate");
            ServerInAccept = false;
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            if (!ServerInAccept)
            {
                eventLog1.WriteEntry("Lanzamos servidor");
                StartListening(10000);
            }
        }

    }
}
