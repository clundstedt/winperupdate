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
using WebSocketSharp;

namespace WinperUpdateServer
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 65700;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class DllFileUI
    {
        public string Nombre { get; set; }
        public string VersionArchivo { get; set; }
    }

    public class UpdateUI
    {
        public long SetupLength { get; set; }
        public List<DllFileUI> Lista { get; set; }

        public string SetupVersion { get; set; }
    }

    public partial class WinperUpdateServer : ServiceBase
    {
        private int eventId = 0;
        private bool ServerInAccept = false;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        System.Timers.Timer timer = new System.Timers.Timer();

        public void StartListening(int port)
        {
            try
            {
                // Data buffer for incoming data.
                byte[] bytes = new Byte[StateObject.BufferSize];

                // Establish the local endpoint for the socket.
                // The DNS name of the computer
                // running the listener is "host.contoso.com".

                string mensaje = string.Format("port: {0}, HostName={1}, HostEntry={2}", port, Dns.GetHostName(), Dns.GetHostEntry(Dns.GetHostName()));

                eventLog1.WriteEntry(mensaje, EventLogEntryType.Information, eventId);

                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipAddress = ipHostInfo.AddressList.ToList().SingleOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                IPAddress ipAddress = ipHostInfo.AddressList.ToList().Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.

                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    eventLog1.WriteEntry("Waiting for a connection...", EventLogEntryType.Information, eventId);
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
                eventLog1.WriteEntry(e.ToString(), EventLogEntryType.Error, eventId);
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
                eventLog1.WriteEntry(output, EventLogEntryType.Error, eventId);
            }
            catch (Exception ex)
            {
                string output = "AcceptCallback Exception: " + ex.ToString();
                eventLog1.WriteEntry(output, EventLogEntryType.Error, eventId);
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
                eventLog1.WriteEntry(String.Format("Read {0} bytes from socket. \n", bytesRead), EventLogEntryType.Information, eventId);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();
                    eventLog1.WriteEntry("Read was: " + content, EventLogEntryType.Information, eventId);
                    if (content.EndsWith("#"))
                    {
                        // All the data has been read from the 
                        // client. Display it on the console.
                        //eventLog1.WriteEntry(String.Format("Read {0} bytes from socket. \n Data : {1}", content.Length, content), EventLogEntryType.Information, eventId);

                        string[] token = content.Split(new Char[] { '#' });
                        string dirVersiones = ConfigurationManager.AppSettings["dirVersiones"];
                        var dirAchivosUI = ConfigurationManager.AppSettings["dirArchivosUI"];
                        dirVersiones += dirVersiones.EndsWith(@"\") ? "" : @"\";

                        switch (token[0])
                        {
                            case "settarea": // settarea#idTarea#estado#mensaje
                                int idTarea = int.Parse(token[1]);
                                int estado = int.Parse(token[2]);
                                string mensaje = "";
                                for (int i = 3; i < token.Length; i++)
                                {
                                    mensaje += (mensaje == "") ? token[i] : "#" + token[i];
                                }

                                int respuesta = ProcessMsg.Tareas.SetEstadoTarea(idTarea, estado, mensaje);
                                json = JsonConvert.SerializeObject(respuesta);
                                Send(handler, json);

                                break; 

                            case "tareas": // tareas#idCliente#Codprf#
                                idCliente = int.Parse(token[1]);
                                int CodPrf = int.Parse(token[2]);

                                var tareas = ProcessMsg.Tareas.GetTareasPendientes(idCliente, CodPrf);
                                foreach (var tarea in tareas)
                                {
                                    var item = ProcessMsg.Version.GetVersiones(eventLog1).SingleOrDefault(x => x.IdVersion == tarea.idVersion);

                                    string fileName = "";
                                    if (!new FileInfo(tarea.NameFile).Extension.Equals(".sql", StringComparison.OrdinalIgnoreCase)) fileName = dirVersiones + item.Release + "\\" + tarea.NameFile;
                                    else fileName = Path.Combine(dirVersiones,item.Release,"Scripts",tarea.NameFile);
                                    if (File.Exists(fileName))
                                    {
                                        System.IO.FileInfo info = new System.IO.FileInfo(fileName);
                                        tarea.LengthFile = info.Length;
                                    }
                                }

                                json = JsonConvert.SerializeObject(tareas);
                                Send(handler, json);
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

                            case "getversiones": // getversiones#idCliente#idAmbiente
                                idCliente = int.Parse(token[1]);
                                int idAmbiente = int.Parse(token[2]);
                                var lista = ProcessMsg.Cliente.GetVersionesAmbiente(idCliente, idAmbiente, eventLog1);
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

                            case "getfunes": // getfunes#idCliente
                                idCliente = int.Parse(token[1]);
                                var listafunes = ProcessMsg.Funes.GetFunes(idCliente, eventLog1);

                                json = JsonConvert.SerializeObject(listafunes);
                                Send(handler, json);
                                break;

                            case "updfunes": // updfunes#idCliente
                                idCliente = int.Parse(token[1]);
                                try
                                {
                                    ProcessMsg.Funes.Actualizar(idCliente, eventLog1);
                                    json = JsonConvert.SerializeObject(new { coderr = 0, msgerr = "" });
                                }
                                catch (Exception ex)
                                {
                                    json = JsonConvert.SerializeObject(new { coderr = 1, msgerr = ex.Message });
                                }
                                Send(handler, json);
                                break;

                            case "getversion": // getversion#NumVersion
                                string release = token[1];
                                var version = ProcessMsg.Version.GetVersion(release, eventLog1);

                                json = JsonConvert.SerializeObject(version);
                                Send(handler, json);
                                break;

                            case "getversionbyid": // getversionbyid#NumVersion
                                var version2 = ProcessMsg.Version.GetVersiones(eventLog1).SingleOrDefault(x => x.IdVersion == int.Parse(token[1]));

                                json = JsonConvert.SerializeObject(version2);
                                Send(handler, json);
                                break;

                            case "getmodulos":
                                var listaModulos = ProcessMsg.Version.GetModulosVersiones(int.Parse(token[1]), eventLog1);

                                json = JsonConvert.SerializeObject(listaModulos);
                                Send(handler, json);
                                break;

                            case "getcomponentes":
                                //var listaArchivos = ProcessMsg.Version.ListarDirectorio(token[1], dirVersiones, eventLog1);
                                var version3 = ProcessMsg.Version.GetVersiones(eventLog1).SingleOrDefault(x => x.IdVersion == int.Parse(token[1]));

                                var listaArchivos = ProcessMsg.Componente.GetComponentes(int.Parse(token[1]), token[2], eventLog1);

                                foreach (var archivo in listaArchivos)
                                {
                                    string fileName = dirVersiones + version3.Release + "\\" + archivo.Name;
                                    System.IO.FileInfo info = new System.IO.FileInfo(fileName);
                                    archivo.Length = info.Length;
                                }

                                json = JsonConvert.SerializeObject(listaArchivos);
                                Send(handler, json);
                                break;

                            case "getfile":
                                bool bok = false;
                                int intentos = 0;
                                while (!bok)
                                {
                                    intentos++;
                                    var buffer = ProcessMsg.Version.DownloadFile(token[1], int.Parse(token[2]), int.Parse(token[3]), dirVersiones, eventLog1);
                                    if (buffer.Length == int.Parse(token[3]))
                                    {
                                        bok = true;
                                        if (intentos > 1)
                                        {
                                            eventLog1.WriteEntry(String.Format("Bytes enviados en intento {0}", intentos), EventLogEntryType.Information, eventId);
                                        }
                                        Send(handler, buffer);
                                    }
                                }                         
                                break;

                            case "modulos":
                                var modulosCliente = ProcessMsg.Modulo.GetModulosWithComponenteByCliente(int.Parse(token[1]));

                                json = JsonConvert.SerializeObject(modulosCliente);
                                Send(handler, json);
                                break;

                            case "checkupui":
                                var clt = ProcessMsg.Cliente.GetClienteByLicencia(token[1], null);
                                if (clt != null)
                                {
                                    List<DllFileUI> listaDll = new List<DllFileUI>();
                                    new DirectoryInfo(dirAchivosUI).GetFiles().ToList().ForEach(file =>
                                    {
                                        listaDll.Add(new DllFileUI
                                        {
                                            Nombre = file.Name,
                                            VersionArchivo = (FileVersionInfo.GetVersionInfo(file.FullName) != null ? FileVersionInfo.GetVersionInfo(file.FullName).FileVersion : "S/I")
                                        });
                                    });
                                    var setup = new FileInfo(Path.Combine(dirAchivosUI, "update", "SetUpdateUI.exe"));
                                    if (setup != null  && setup.Exists) {
                                        UpdateUI uui = new UpdateUI
                                        {
                                            Lista = listaDll,
                                            SetupLength = setup.Length,
                                            SetupVersion = (FileVersionInfo.GetVersionInfo(setup.FullName) != null ? FileVersionInfo.GetVersionInfo(setup.FullName).FileVersion : "S/I")
                                        };

                                        json = JsonConvert.SerializeObject(uui);
                                        Send(handler, json);
                                    }
                                    else { Send(handler, "0"); }
                                }
                                else { Send(handler, "0"); }
                                break;

                            case "downsetup":
                                bool downsetupbok = false;
                                int downsetupintentos = 0;
                                while (!downsetupbok)
                                {
                                    downsetupintentos++;
                                    var buffer = ProcessMsg.Version.DownloadFile("SetUpdateUI.exe", int.Parse(token[1]), int.Parse(token[2]), dirAchivosUI+"\\update\\", eventLog1);
                                    if (buffer.Length == int.Parse(token[2]))
                                    {
                                        downsetupbok = true;
                                        if (downsetupintentos > 1)
                                        {
                                            eventLog1.WriteEntry(String.Format("Bytes enviados en intento {0}", downsetupintentos), EventLogEntryType.Information, eventId);
                                        }
                                        Send(handler, buffer);
                                    }
                                }
                                break;

                            case "checkadmin":
                                var mail = token[1];
                                var pass = token[2];
                                var licencia = token[3];
                                var res = ProcessMsg.Seguridad.GetUsuario(mail);
                                if (res != null)
                                {
                                    var cltAdm = ProcessMsg.Cliente.GetClienteUsuario(res.Id);
                                    if (cltAdm != null)
                                    {
                                        if (pass.Equals(Utils.GetMd5Hash(Utils.DesEncriptar(res.Clave))) && res.CodPrf == 11 && cltAdm.NroLicencia.Equals(licencia))
                                        {
                                            Send(handler, "1");
                                        }
                                        else
                                        {
                                            Send(handler, "0");
                                        }
                                    }
                                    else
                                    {
                                        Send(handler, "0");
                                    }
                                }
                                else
                                {
                                    Send(handler, "0");
                                }
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
                eventLog1.WriteEntry(output, EventLogEntryType.Error, eventId);
            }
            catch (Exception ex)
            {
                string output = "ReadCallback Exception: " + ex.ToString();
                eventLog1.WriteEntry(output, EventLogEntryType.Error, eventId);
            }
        }

        private void Send(Socket handler, String data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.UTF8.GetBytes(data);

                eventLog1.WriteEntry(String.Format("Send {0} bytes from socket. \n", byteData.Length), EventLogEntryType.Information, eventId);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            catch(Exception ex)
            {
                eventLog1.WriteEntry(String.Format("Excepcion en 'Send': {0}", ex.ToString()), EventLogEntryType.Warning, eventId);
            }
        }

        private void Send(Socket handler, byte[] byteData)
        {
            try
            {
                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);

            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(String.Format("Excepcion en 'Send(Socket, byte[])': {0}", ex.ToString()), EventLogEntryType.Warning, eventId);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                eventLog1.WriteEntry(string.Format("Sent {0} bytes to client.", bytesSent), EventLogEntryType.Information, eventId);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                eventLog1.WriteEntry(e.ToString(), EventLogEntryType.Error, eventId);
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
            try
            {
                // TODO: Insert monitoring activities here.
                if (!ServerInAccept)
                {
                    eventLog1.WriteEntry("Lanzamos servidor");
                    string port = ConfigurationManager.AppSettings["port"];
                    StartListening(int.Parse(port));
                }
            }
            catch (Exception e)
            {
                eventLog1.WriteEntry(e.ToString(), EventLogEntryType.Error, eventId);
            }

        }

        

    }
}
