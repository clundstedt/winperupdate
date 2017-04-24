namespace WinPerUpdateUI
{
    partial class FormPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.notifyIcon2 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.timerUI = new System.Windows.Forms.Timer(this.components);
            this.SvcWPUI = new System.ServiceProcess.ServiceController();
            this.timerPing = new System.Windows.Forms.Timer(this.components);
            this.workerPing = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // notifyIcon2
            // 
            this.notifyIcon2.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon2.Icon")));
            this.notifyIcon2.Text = "Winper Update";
            this.notifyIcon2.Visible = true;
            this.notifyIcon2.BalloonTipClicked += new System.EventHandler(this.notifyIcon2_BalloonTipClicked);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
            this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
            // 
            // timerUI
            // 
            this.timerUI.Interval = 30000;
            this.timerUI.Tick += new System.EventHandler(this.timerUI_Tick);
            // 
            // SvcWPUI
            // 
            this.SvcWPUI.ServiceName = "WPUISVC";
            // 
            // timerPing
            // 
            this.timerPing.Enabled = true;
            this.timerPing.Interval = 120000;
            this.timerPing.Tick += new System.EventHandler(this.timerPing_Tick);
            // 
            // workerPing
            // 
            this.workerPing.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerPing_DoWork);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(863, 487);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrador de Versiones Winper";
            this.Activated += new System.EventHandler(this.FormPrincipal_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            this.Resize += new System.EventHandler(this.FormPrincipal_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon2;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker worker;
        private System.Windows.Forms.Timer timerUI;
        private System.ServiceProcess.ServiceController SvcWPUI;
        private System.Windows.Forms.Timer timerPing;
        private System.ComponentModel.BackgroundWorker workerPing;
    }
}

