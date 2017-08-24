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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ambiente));
            this.label1 = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.txtNroLicencia = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPerfil = new System.Windows.Forms.ComboBox();
            this.dgAmbientes = new System.Windows.Forms.DataGridView();
            this.CodAmbiente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NomAmbiente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirAmbiente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkPermitirSQL = new System.Windows.Forms.CheckBox();
            this.FbdDirectorioWinper = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgAmbientes)).BeginInit();
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
            this.btnAceptar.Location = new System.Drawing.Point(368, 297);
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
            this.txtNroLicencia.Size = new System.Drawing.Size(451, 20);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 219);
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
            this.cmbPerfil.Location = new System.Drawing.Point(17, 236);
            this.cmbPerfil.Name = "cmbPerfil";
            this.cmbPerfil.Size = new System.Drawing.Size(451, 21);
            this.cmbPerfil.TabIndex = 6;
            this.cmbPerfil.SelectedIndexChanged += new System.EventHandler(this.cmbPerfil_SelectedIndexChanged);
            // 
            // dgAmbientes
            // 
            this.dgAmbientes.AllowUserToAddRows = false;
            this.dgAmbientes.AllowUserToDeleteRows = false;
            this.dgAmbientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgAmbientes.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgAmbientes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgAmbientes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dgAmbientes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgAmbientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAmbientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CodAmbiente,
            this.NomAmbiente,
            this.DirAmbiente});
            this.dgAmbientes.Location = new System.Drawing.Point(17, 76);
            this.dgAmbientes.Name = "dgAmbientes";
            this.dgAmbientes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAmbientes.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgAmbientes.Size = new System.Drawing.Size(451, 125);
            this.dgAmbientes.TabIndex = 7;
            this.dgAmbientes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dgAmbientes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgAmbientes_CellDoubleClick);
            // 
            // CodAmbiente
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.CodAmbiente.DefaultCellStyle = dataGridViewCellStyle1;
            this.CodAmbiente.HeaderText = "Codigo";
            this.CodAmbiente.Name = "CodAmbiente";
            this.CodAmbiente.ReadOnly = true;
            this.CodAmbiente.Visible = false;
            // 
            // NomAmbiente
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.NomAmbiente.DefaultCellStyle = dataGridViewCellStyle2;
            this.NomAmbiente.HeaderText = "Ambiente";
            this.NomAmbiente.Name = "NomAmbiente";
            this.NomAmbiente.ReadOnly = true;
            // 
            // DirAmbiente
            // 
            this.DirAmbiente.HeaderText = "Directorio";
            this.DirAmbiente.Name = "DirAmbiente";
            // 
            // chkPermitirSQL
            // 
            this.chkPermitirSQL.AutoSize = true;
            this.chkPermitirSQL.Location = new System.Drawing.Point(17, 264);
            this.chkPermitirSQL.Name = "chkPermitirSQL";
            this.chkPermitirSQL.Size = new System.Drawing.Size(238, 17);
            this.chkPermitirSQL.TabIndex = 8;
            this.chkPermitirSQL.Text = "Permitir al Administrador ejecutar scripts SQL.";
            this.chkPermitirSQL.UseVisualStyleBackColor = true;
            this.chkPermitirSQL.Visible = false;
            // 
            // Ambiente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 326);
            this.Controls.Add(this.chkPermitirSQL);
            this.Controls.Add(this.dgAmbientes);
            this.Controls.Add(this.cmbPerfil);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNroLicencia);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Ambiente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ambiente";
            this.Load += new System.EventHandler(this.Ambiente_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgAmbientes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAceptar;
        public System.Windows.Forms.TextBox txtNroLicencia;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbPerfil;
        private System.Windows.Forms.DataGridView dgAmbientes;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodAmbiente;
        private System.Windows.Forms.DataGridViewTextBoxColumn NomAmbiente;
        private System.Windows.Forms.DataGridViewTextBoxColumn DirAmbiente;
        private System.Windows.Forms.CheckBox chkPermitirSQL;
        private System.Windows.Forms.FolderBrowserDialog FbdDirectorioWinper;
    }
}