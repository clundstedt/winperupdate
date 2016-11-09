namespace WinPerUpdateUI
{
    partial class Ambiente
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
            this.btnAceptar = new System.Windows.Forms.Button();
            this.txtNroLicencia = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.clbAmbientes = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPerfil = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDirWinper = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Número de Licencia del Producto Winper";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(223, 291);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(100, 23);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // txtNroLicencia
            // 
            this.txtNroLicencia.Location = new System.Drawing.Point(17, 24);
            this.txtNroLicencia.Name = "txtNroLicencia";
            this.txtNroLicencia.Size = new System.Drawing.Size(309, 20);
            this.txtNroLicencia.TabIndex = 3;
            this.txtNroLicencia.TextChanged += new System.EventHandler(this.txtNroLicencia_TextChanged);
            this.txtNroLicencia.Leave += new System.EventHandler(this.txtNroLicencia_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ambientes donde Opera";
            // 
            // clbAmbientes
            // 
            this.clbAmbientes.FormattingEnabled = true;
            this.clbAmbientes.Location = new System.Drawing.Point(19, 76);
            this.clbAmbientes.Name = "clbAmbientes";
            this.clbAmbientes.Size = new System.Drawing.Size(306, 94);
            this.clbAmbientes.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Perfíl Usuario";
            // 
            // cmbPerfil
            // 
            this.cmbPerfil.FormattingEnabled = true;
            this.cmbPerfil.Items.AddRange(new object[] {
            "Administrador",
            "DBA",
            "Otro"});
            this.cmbPerfil.Location = new System.Drawing.Point(20, 199);
            this.cmbPerfil.Name = "cmbPerfil";
            this.cmbPerfil.Size = new System.Drawing.Size(303, 21);
            this.cmbPerfil.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Directorio Winper";
            // 
            // txtDirWinper
            // 
            this.txtDirWinper.Location = new System.Drawing.Point(20, 253);
            this.txtDirWinper.Name = "txtDirWinper";
            this.txtDirWinper.Size = new System.Drawing.Size(303, 20);
            this.txtDirWinper.TabIndex = 8;
            // 
            // Ambiente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 324);
            this.Controls.Add(this.txtDirWinper);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbPerfil);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clbAmbientes);
            this.Controls.Add(this.txtNroLicencia);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Ambiente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ambiente";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.TextBox txtNroLicencia;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox clbAmbientes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbPerfil;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDirWinper;
    }
}