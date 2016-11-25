namespace WinperUpdateStrCon
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.TxtStrCon = new System.Windows.Forms.TextBox();
            this.BtnEncriptar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtStrConEnc = new System.Windows.Forms.TextBox();
            this.BtnSelCop = new System.Windows.Forms.Button();
            this.BtnDesencriptar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "String de Conexión";
            // 
            // TxtStrCon
            // 
            this.TxtStrCon.Location = new System.Drawing.Point(15, 25);
            this.TxtStrCon.Name = "TxtStrCon";
            this.TxtStrCon.Size = new System.Drawing.Size(677, 20);
            this.TxtStrCon.TabIndex = 1;
            // 
            // BtnEncriptar
            // 
            this.BtnEncriptar.Location = new System.Drawing.Point(12, 51);
            this.BtnEncriptar.Name = "BtnEncriptar";
            this.BtnEncriptar.Size = new System.Drawing.Size(339, 23);
            this.BtnEncriptar.TabIndex = 2;
            this.BtnEncriptar.Text = "Encriptar";
            this.BtnEncriptar.UseVisualStyleBackColor = true;
            this.BtnEncriptar.Click += new System.EventHandler(this.BtnEncriptar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Salida String de Conexión";
            // 
            // TxtStrConEnc
            // 
            this.TxtStrConEnc.Location = new System.Drawing.Point(15, 93);
            this.TxtStrConEnc.Name = "TxtStrConEnc";
            this.TxtStrConEnc.Size = new System.Drawing.Size(677, 20);
            this.TxtStrConEnc.TabIndex = 4;
            // 
            // BtnSelCop
            // 
            this.BtnSelCop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSelCop.Location = new System.Drawing.Point(12, 119);
            this.BtnSelCop.Name = "BtnSelCop";
            this.BtnSelCop.Size = new System.Drawing.Size(680, 34);
            this.BtnSelCop.TabIndex = 5;
            this.BtnSelCop.Text = "Copiar String Encriptado";
            this.BtnSelCop.UseVisualStyleBackColor = true;
            this.BtnSelCop.Click += new System.EventHandler(this.BtnSelCop_Click);
            // 
            // BtnDesencriptar
            // 
            this.BtnDesencriptar.Location = new System.Drawing.Point(357, 51);
            this.BtnDesencriptar.Name = "BtnDesencriptar";
            this.BtnDesencriptar.Size = new System.Drawing.Size(335, 23);
            this.BtnDesencriptar.TabIndex = 6;
            this.BtnDesencriptar.Text = "Desencriptar";
            this.BtnDesencriptar.UseVisualStyleBackColor = true;
            this.BtnDesencriptar.Click += new System.EventHandler(this.BtnDesencriptar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 170);
            this.Controls.Add(this.BtnDesencriptar);
            this.Controls.Add(this.BtnSelCop);
            this.Controls.Add(this.TxtStrConEnc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnEncriptar);
            this.Controls.Add(this.TxtStrCon);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Encriptador de StringConnection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtStrCon;
        private System.Windows.Forms.Button BtnEncriptar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtStrConEnc;
        private System.Windows.Forms.Button BtnSelCop;
        private System.Windows.Forms.Button BtnDesencriptar;
    }
}

