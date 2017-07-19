using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using WinSCP;
using System.IO;

namespace ProcessMsg
{
    public static class Utils
    {
        ///Correo soporte Winper Update
        public static string CorreoSoporte = ConfigurationManager.AppSettings["correoSoporte"];

        /// Encripta una cadena
        public static string Encriptar(this string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public static string DesEncriptar(this string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

        public static string G_Encripta(string palabra)
        {
            string strRes = "";
            var arr = palabra.ToCharArray().Select(x => Convert.ToInt32(x));
            foreach (var x in arr)
            {
                strRes += (((127 * 3) + x) + (x % 2 != 0 ? 127 : -127));
            }
            return strRes;
        }

        public static string G_Desencripta(string hash)
        {
            string str = "";
            for (int i = 0; i < hash.Length; i += 3)
            {
                var n = int.Parse(hash.Substring(i, 3));
                str += Convert.ToChar((n - (127 * 3) + (n % 2 == 0 ? 127 : -127)));
            }
            return str;
        }
        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string GetPathSetting(string ServerMapPath)
        {
            if (ServerMapPath.Contains("Uploads"))
            {
                var u = ConfigurationManager.AppSettings["upload"];
                if (u != null && !u.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    if (!u.EndsWith("\\")) u += "\\";
                    return u;
                }
            }
            else if (ServerMapPath.Contains("VersionOficial"))
            {
                var vo = ConfigurationManager.AppSettings["voficial"];
                if (vo != null && !vo.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    if (!vo.EndsWith("\\")) vo += "\\";
                    return vo;
                }
            }
            else if (ServerMapPath.Contains("Fuentes"))
            {
                var f = ConfigurationManager.AppSettings["fuentes"];
                if (f != null && !f.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    if (!f.EndsWith("\\")) f += "\\";
                    return f;
                }
            }
            return ServerMapPath;
        }

        public static Boolean SendMail(String mensaje, String asunto, String mailctt)
        {
            string userMail = ConfigurationManager.AppSettings["userMail"];
            string pwdMail = ConfigurationManager.AppSettings["pwdMail"];
            string fromMail = ConfigurationManager.AppSettings["fromMail"];
            string HostMail = ConfigurationManager.AppSettings["HostMail"];
            string portMail = ConfigurationManager.AppSettings["portMail"];
            string sslMail = ConfigurationManager.AppSettings["sslMail"];
            
            var correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress(userMail, fromMail);
            correo.To.Add(new System.Net.Mail.MailAddress(mailctt));
            correo.Subject = asunto;
            correo.IsBodyHtml = true;
            correo.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            correo.Body = mensaje;

            var smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = HostMail;
            smtp.Port = int.Parse(portMail);
            smtp.Credentials = new System.Net.NetworkCredential(userMail, pwdMail);
            smtp.EnableSsl = bool.Parse(sslMail);

            try
            {
                smtp.Send(correo);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error SendMail: {0}", ex.ToString()));
            }
        }

        public static String ReadPlantilla(String sPathPlantilla)
        {
            string textoHtml = "";
            var sr = new System.IO.StreamReader(sPathPlantilla);
            textoHtml = sr.ReadToEnd();
            sr.Close();

            return textoHtml;
        }

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

        public static bool VerificarMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (0 == comparer.Compare(hashOfInput, hash));
        }

        public static string GenerarLicencia(int NumFolio, string mescon
            , int correlativo, int estmtc
            , string mesini, string nrotrbc
            , string nrotrbh, string nrousr)
        {
            string fStr = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", NumFolio, mescon, correlativo, estmtc, mesini,nrotrbc,nrotrbh,nrousr);
            string hash = GetMd5Hash(fStr);
            string licencia = "";
            for (int i = 0; i < hash.Length; i++)
            {
                licencia += hash[i];
                if ((i + 1) % 4 == 0 && (i + 1) < hash.Length)
                {
                    licencia += "-";
                }
            }
            return licencia.ToUpper();
        }

        public static iTextSharp.text.pdf.PdfPTable GenerarTablaPDF(int AnchoPorcentaje, int Alineacion, System.Data.DataTable dt)
        {
            iTextSharp.text.pdf.PdfPTable tbl = new iTextSharp.text.pdf.PdfPTable(dt.Columns.Count);
            iTextSharp.text.pdf.PdfPCell[] celCol = new iTextSharp.text.pdf.PdfPCell[dt.Columns.Count];
            for (int i = 0; i < celCol.Length; i++)
            {
                celCol[i] = new iTextSharp.text.pdf.PdfPCell(
                    new iTextSharp.text.Paragraph(
                        new iTextSharp.text.Chunk(dt.Columns[i].ColumnName,iTextSharp.text.FontFactory.GetFont("ARIAL",10, iTextSharp.text.Font.BOLD))));
            }
            tbl.WidthPercentage = AnchoPorcentaje;
            tbl.HorizontalAlignment = Alineacion;
            tbl.Rows.Add(new iTextSharp.text.pdf.PdfPRow(celCol));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var cell = new iTextSharp.text.pdf.PdfPCell[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    cell[j] = new iTextSharp.text.pdf.PdfPCell(
                        new iTextSharp.text.Paragraph(
                            new iTextSharp.text.Chunk(dt.Rows[i][j].ToString(), iTextSharp.text.FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                }
                tbl.Rows.Add(new iTextSharp.text.pdf.PdfPRow(cell));
            }
            return tbl;
        }
        /// <summary>
        /// Transfiere por FTP todos los archivos del PathOrigen al PathDestino.
        /// </summary>
        /// <param name="pathOrigen"></param>
        /// <param name="pathDestino"></param>
        /// <returns></returns>
        public static bool FtpSend(string pathOrigen, string pathDestino)
        {
            try
            {
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = ConfigurationManager.AppSettings["hostnameftp"],
                    UserName = ConfigurationManager.AppSettings["userftp"],
                    Password = DesEncriptar(ConfigurationManager.AppSettings["passwordftp"]),
                    PortNumber = int.Parse(ConfigurationManager.AppSettings["portftp"]),
                    FtpMode = bool.Parse(ConfigurationManager.AppSettings["passivemode"]) ? FtpMode.Passive : FtpMode.Active
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    if (!session.FileExists(pathDestino))
                    {
                        session.CreateDirectory(pathDestino);
                    }

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                    TransferOperationResult transferResult;


                    transferResult = session.PutFiles(Path.Combine(pathOrigen, "*"), Path.Combine(pathDestino), false, transferOptions);
                    transferResult.Check();


                    session.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool FtpGet(string pathOrigen, string pathDestino)
        {
            try
            {
                var dataEnc = G_Desencripta(ConfigurationManager.AppSettings["cftp"]);
                var dataFtp = DesEncriptar(dataEnc).Split('#');
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = dataFtp[0],
                    UserName = dataFtp[1],
                    Password = dataFtp[2],
                    PortNumber = int.Parse(dataFtp[3]),
                    FtpMode = bool.Parse(dataFtp[4]) ? FtpMode.Passive : FtpMode.Passive,
                    SshHostKeyFingerprint = string.Format("ssh-dss 1024 {0}", dataFtp[5])
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    if (!session.FileExists(pathOrigen))
                    {
                        return false;
                    }

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                    TransferOperationResult transferResult;
                    
                    transferResult = session.GetFiles(pathOrigen+"*", pathDestino, false, transferOptions);
                    transferResult.Check();

                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Download of {0} succeeded", transfer.FileName);
                    }

                    session.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Genera la version siguiente de la especificada en el parametro
        /// </summary>
        /// <param name="VersionActual"></param>
        /// <returns>Version siguiente</returns>
        /// 

        public static string GenerarVersionSiguiente(string VersionActual)
        {
            var nVersion = "";
            var parts = VersionActual.Split('.');
            parts[parts.Length - 1] = (int.Parse(parts[parts.Length - 1]) + 1).ToString();
            for (var i = 0; i < parts.Length; i++)
            {
                nVersion += parts[i] + (i != parts.Length - 1 ? "." : "");
            }
            return nVersion;
        }
        
        /*
            Retorna -1 Si VersionOtra es menor
            Retorna 0 Si las versiones son iguales
            Retorna 1 Si la VersionOtra es mayor
        */
        
        public static int ComparaVersion(string versionBase, string versionOtra)
        {
            try
            {
                var arrVB = versionBase.Split('.');
                var arrVO = versionOtra.Split('.');
                do
                {
                    if (arrVB.Length < arrVO.Length) versionBase += ".0";
                    else if (arrVO.Length < arrVB.Length) versionOtra += ".0";
                    arrVB = versionBase.Split('.');
                    arrVO = versionOtra.Split('.');
                } while (arrVB.Length != arrVO.Length);
                for (var i = 0; i < arrVB.Length; i++)
                {
                    if (int.Parse(arrVB[i]) > int.Parse(arrVO[i])) return -1;
                    else if (int.Parse(arrVO[i]) > int.Parse(arrVB[i])) return 1;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
