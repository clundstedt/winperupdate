using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WinperUpdateServer
{
    public class Utils
    {
        const int SIZEBUFFER = 524288;

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
                Console.WriteLine("Servidor acepta coneccion ...");

                // Get a client stream for reading and writing.
                // Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Translate the passed message into ASCII and store it as a byte array.
                Byte[] data = new Byte[SIZEBUFFER];
                data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                output = "Sent: " + message;
                Console.WriteLine(output);

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
                Console.WriteLine(output);

                // Close everything.
                stream.Close();
                client.Close();

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                output = "ArgumentNullException: " + e;
                Console.WriteLine(output);
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                Console.WriteLine(output);
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
                Console.WriteLine("Servidor acepta coneccion ...");

                // Get a client stream for reading and writing.
                // Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Translate the passed message into ASCII and store it as a byte array.
                Byte[] data = new Byte[1024];
                data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                output = "Sent: " + message;
                Console.WriteLine(output);

                // Buffer to store the response bytes.
                data = new Byte[4096];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                output = string.Format("Received ({0}): {1} ", bytes, responseData);
                Console.WriteLine(output);

                // Close everything.
                stream.Close();
                client.Close();

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                output = "ArgumentNullException: " + e;
                Console.WriteLine(output);
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                Console.WriteLine(output);
            }

            return null;
        }
    }
}
