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
            this.btnAceptar.Location = new System.Drawing.Point(225, 183);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(100, 23);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // txtNroLicencia
            // 
            this.txtNroLicencia.Location = new System.Drawing.Point(17, 25);
            this.txtNroLicencia.Name = "txtNroLicencia";
            this.txtNroLicencia.Size = new System.Drawing.Size(309, 20);
            this.txtNroLicencia.TabIndex = 3;
            this.txtNroLicencia.TextChanged += new System.EventHandler(this.txtNroLicencia_TextChanged);
            this.txtNroLicencia.Leave += new System.EventHandler(this.txtNroLicencia_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ambientes donde Opera";
            // 
            // clbAmbientes
            // 
            this.clbAmbientes.FormattingEnabled = true;
            this.clbAmbientes.Location = new System.Drawing.Point(19, 81);
            this.clbAmbientes.Name = "clbAmbientes";
            this.clbAmbientes.Size = new System.Drawing.Size(306, 94);
            this.clbAmbientes.TabIndex = 4;
            // 
            // Ambiente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 218);
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
    }
}