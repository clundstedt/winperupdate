using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Text;
using System.Configuration;

namespace WinperUpdateClient
{

    public class VersionBo
    {
        public string Release { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class AtributosArchivoBo
    {
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime LastWrite { get; set; }
        public long Length { get; set; }
        public string Version { get; set; }
    }

    public partial class WinperUpdateClient : ServiceBase
    {
        Timer timer = new Timer();
        private bool ServerInAccept = false;

        public WinperUpdateClient()
        {
            InitializeComponent();
            //eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("Winper Update Log"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Winper Update Log", "WinperUpdateLog");
            }
            eventLog1.Source = "Winper Update Log";
            eventLog1.Log = "WinperUpdateLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Start WinperUpdateClient");
            ServerInAccept = false;

            // Set up a timer to trigger every minute.
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Stop WinperUpdateClient");
            ServerInAccept = false;
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            string dirVersiones = ConfigurationManager.AppSettings["dirVersiones"];

        }

    }
}
