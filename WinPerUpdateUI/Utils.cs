using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.SQLite;

namespace WinPerUpdateUI
{
    public class Utils
    {
        public static List<ProcessMsg.Model.ModuloBo> ModulosContratados = new List<ProcessMsg.Model.ModuloBo>();
        public static bool isCentralizado = false;
        public static bool blockMenu = false;

        const int SIZEBUFFER = 524288;

        public static string GetMd5Hash(string input)
        {
            byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public static void RegistrarLog(string NombreLog, string Text)
        {
            var dirTempUser = Path.Combine(Path.GetTempPath(), "WinperUI");
            if (!Directory.Exists(dirTempUser))
            {
                Directory.CreateDirectory(dirTempUser);
            }
            string dir = Path.Combine(dirTempUser, string.Format("{0:ddMMyyyy}", DateTime.Now));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var dirLog = Path.Combine(dir, NombreLog.Replace(".log", ""));
            if (!Directory.Exists(dirLog))
            {
                Directory.CreateDirectory(dirLog);
            }
            var log = Path.Combine(dirLog, string.Format("{0}",NombreLog));
            StreamWriter writer = new StreamWriter(log, true);
            writer.WriteLine(string.Format("{0:dd/MM/yyyy HH:mm:ss} | {1}", DateTime.Now, Text));
            writer.Close();
        }

        public static string ShowDialogInput(string text, string caption, bool isPassword = false)
        {
            System.Windows.Forms.Form prompt = new System.Windows.Forms.Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            };
            System.Windows.Forms.Label textLabel = new System.Windows.Forms.Label() { Left = 50, Top = 20, Text = text, AutoSize = true, AutoEllipsis = true };
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox() { Left = 50, Top = 50, Width = 400 };
            if (isPassword) textBox.PasswordChar = 'X';
            System.Windows.Forms.Button confirmation = new System.Windows.Forms.Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = System.Windows.Forms.DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == System.Windows.Forms.DialogResult.OK ? textBox.Text : "";
        }
        static string BytesToStringConverted(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static byte[] SendMsg(string ipServer, int port, string message, int sizeBuffer)
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
                Byte[] data = new Byte[sizeBuffer];
                data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, message.Length);

                output = "Sent: " + message;
                //eventLog1.WriteEntry(output);

                // Buffer to store the response bytes.
                data = new Byte[sizeBuffer];

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

        public static byte[] SendMsg(string ipServer, int port, string message)
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
                stream.Write(data, 0, message.Length);

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

        public static string StrSendMsg(string ipServer, int port, string message)
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
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
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
                throw new Exception(output);
                //eventLog1.WriteEntry(output);
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                //eventLog1.WriteEntry(output);
                throw new Exception(output);
            }
            
        }

        public static void SetSetting(string campo, object valor)
        {
            string sql = "";
            
            if(valor is string || valor is bool) sql = string.Format("UPDATE Setting SET {0}='{1}'", campo, valor);
            else sql = string.Format("UPDATE Setting SET {0}={1}", campo, valor);

            var dirSqlite = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"WinActUI");
            if (!Directory.Exists(dirSqlite)) Directory.CreateDirectory(dirSqlite);
            dirSqlite = Path.Combine(dirSqlite, "AppSetting.sqlite");
            if (!File.Exists(dirSqlite)) CrearAppSetting(dirSqlite);

            var strConn = string.Format("Data Source={0};Version=3;", dirSqlite);
            SQLiteConnection lite = new SQLiteConnection(strConn);
            lite.Open();
            SQLiteCommand cmdUpd = new SQLiteCommand(sql, lite);
            cmdUpd.ExecuteNonQuery();
            lite.Close();
        }

        public static string GetSetting(string campo)
        {
            var dirSqlite = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinActUI");
            if (!Directory.Exists(dirSqlite)) Directory.CreateDirectory(dirSqlite);
            dirSqlite = Path.Combine(dirSqlite, "AppSetting.sqlite");
            if (!File.Exists(dirSqlite))CrearAppSetting(dirSqlite);

            var strConn = string.Format("Data Source={0};Version=3;", dirSqlite);
            SQLiteConnection lite = new SQLiteConnection(strConn);
            lite.Open();

            SQLiteCommand cmd = new SQLiteCommand(string.Format("SELECT {0} FROM Setting", campo), lite);
            string val = null;
            var r = cmd.ExecuteReader();
            while (r.Read())
            {
                val = r[0].ToString();
            }
            r.Close();
            lite.Close();
            return val;
        }
        private static void CrearAppSetting(string file)
        {
            SQLiteConnection.CreateFile(file);
            var strConn = string.Format("Data Source={0};Version=3;", file);
            SQLiteConnection lite = new SQLiteConnection(strConn);
            lite.Open();
            SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE Setting (server varchar(10), port int, sql bit, cftp text)", lite);
            cmd.ExecuteNonQuery();
            cmd = new SQLiteCommand("INSERT INTO Setting (server, port, sql, cftp) VALUES (null,0,0,null)", lite);
            cmd.ExecuteNonQuery();
            lite.Close();
        }

        public static void SetAppSetting(string nodo, string valor)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("AppSetting.xml");
            var a = xDoc.DocumentElement;
            for (int i = 0; i < a.ChildNodes.Count; i++)
            {
                if (a.ChildNodes[i].LocalName.Equals(nodo, StringComparison.OrdinalIgnoreCase))
                {
                    a.ChildNodes[i].InnerText = valor;
                }
            }
            xDoc.Save("AppSetting.xml");
        }

        public static string GetAppSetting(string nodo)
        {
            string str = null;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("AppSetting.xml");
            var a = xDoc.DocumentElement;
            for (int i = 0; i < a.ChildNodes.Count; i++)
            {
                if (a.ChildNodes[i].LocalName.Equals(nodo, StringComparison.OrdinalIgnoreCase))
                {
                    str = a.ChildNodes[i].InnerText;
                }
            }
            return str;
        }
    }
}
