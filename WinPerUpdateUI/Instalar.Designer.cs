namespace WinPerUpdateUI
{
    partial class Instalar
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
            this.components = new System.ComponentModel.Container();
            this.loginstalacion = new System.Windows.Forms.Label();
            this.timerInstalar = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // loginstalacion
            // 
            this.loginstalacion.AutoSize = true;
            this.loginstalacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginstalacion.Location = new System.Drawing.Point(13, 13);
            this.loginstalacion.Name = "loginstalacion";
            this.loginstalacion.Size = new System.Drawing.Size(234, 13);
            this.loginstalacion.TabIndex = 0;
            this.loginstalacion.Text = "Aquí se muestra lo que está pasando ...";
            // 
            // timerInstalar
            // 
            this.timerInstalar.Interval = 1000;
            this.timerInstalar.Tick += new System.EventHandler(this.timerInstalar_Tick);
            // 
            // Instalar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 42);
            this.ControlBox = false;
            this.Controls.Add(this.loginstalacion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Instalar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Instalar";
            this.Load += new System.EventHandler(this.Instalar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label loginstalacion;
        private System.Windows.Forms.Timer timerInstalar;
    }
}