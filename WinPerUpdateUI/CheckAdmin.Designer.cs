namespace WinPerUpdateUI
{
    partial class CheckAdmin
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
            this.label2 = new System.Windows.Forms.Label();
            this.TxtNombre = new System.Windows.Forms.TextBox();
            this.TxtPassword = new System.Windows.Forms.TextBox();
            this.BtnValidar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "E-Mail del Administrador";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // TxtNombre
            // 
            this.TxtNombre.Location = new System.Drawing.Point(137, 6);
            this.TxtNombre.Name = "TxtNombre";
            this.TxtNombre.Size = new System.Drawing.Size(135, 20);
            this.TxtNombre.TabIndex = 2;
            // 
            // TxtPassword
            // 
            this.TxtPassword.Location = new System.Drawing.Point(137, 37);
            this.TxtPassword.Name = "TxtPassword";
            this.TxtPassword.PasswordChar = 'X';
            this.TxtPassword.Size = new System.Drawing.Size(135, 20);
            this.TxtPassword.TabIndex = 3;
            // 
            // BtnValidar
            // 
            this.BtnValidar.Location = new System.Drawing.Point(15, 63);
            this.BtnValidar.Name = "BtnValidar";
            this.BtnValidar.Size = new System.Drawing.Size(257, 23);
            this.BtnValidar.TabIndex = 4;
            this.BtnValidar.Text = "Validar";
            this.BtnValidar.UseVisualStyleBackColor = true;
            this.BtnValidar.Click += new System.EventHandler(this.BtnValidar_Click);
            // 
            // CheckAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 100);
            this.Controls.Add(this.BtnValidar);
            this.Controls.Add(this.TxtPassword);
            this.Controls.Add(this.TxtNombre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CheckAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CheckAdmin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtNombre;
        private System.Windows.Forms.TextBox TxtPassword;
        private System.Windows.Forms.Button BtnValidar;
    }
}