using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NTPClockService
{
    internal class MainService
    {
        private static readonly string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private bool isServieStopped;
        private Process process;

        public void Start()
        {
            foreach (var p in Process.GetProcessesByName("NTPClock"))
            {
                p.Kill();
            }

            this.process = new Process
                           {
                               StartInfo = { FileName = Path.Combine(CurrentDirectory, "NTPClock.exe"), UseShellExecute = false },
                               EnableRaisingEvents = true
                           };

            this.process.Exited += this.ProcessOnExited;

            this.process.Start();
        }

        public void Stop()
        {
            this.isServieStopped = true;

            this.process?.Kill();
        }

        private void ProcessOnExited(object sender, EventArgs e)
        {
            if (!this.isServieStopped)
            {
                Environment.Exit(int.MinValue);
            }
        }
    }
}