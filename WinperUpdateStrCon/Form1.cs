﻿using System;
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
            TxtStrConEnc.Text = Encriptar(TxtStrCon.Text);
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
                TxtStrConEnc.Text = DesEncriptar(TxtStrCon.Text);
            }
            catch (FormatException fex)
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

    }
}
