using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinperUpdateStrCon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnEncriptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtStrCon.Text))
            {
                MessageBox.Show("Campo 'String de Conexión' se encuentra vacío","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (RdbBase.Checked) TxtStrConEnc.Text = Encriptar(TxtStrCon.Text);
            else TxtStrConEnc.Text = G_Encripta(TxtStrCon.Text);
        }

        private void BtnSelCop_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtStrConEnc.Text))
                {
                    MessageBox.Show("Primero debe encriptar la string de conexión", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                TxtStrConEnc.SelectAll();
                Clipboard.SetDataObject(TxtStrConEnc.Text, true);
                MessageBox.Show("String de Conexión copiado al portapeles de Windows", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al copiar el texto en el portapapeles. \n\nError: {0}",ex.Message),"Excepción Controlada", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        
        private void BtnDesencriptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtStrCon.Text))
                {
                    MessageBox.Show("Campo 'String de Conexión' se encuentra vacío", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (RdbBase.Checked) TxtStrConEnc.Text = DesEncriptar(TxtStrCon.Text);
                else TxtStrConEnc.Text = G_Desencripta(TxtStrCon.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("El 'String de Conexión' no se encuentra encriptado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// Encripta una cadena
        private string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }
        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public string DesEncriptar(string _cadenaAdesencriptar)
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

    }
}
