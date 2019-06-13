using System;
using Topshelf;

namespace NTPClockService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rc = HostFactory.Run(
                hc =>
                    {
                        hc.SetServiceName("NTPClockService");
                        hc.SetDisplayName("NTPClock Service");
                        hc.SetDescription("NTPClock Service");

                        hc.StartAutomatically();

                        hc.RunAsLocalSystem();

                        hc.EnableServiceRecovery(
                            sr =>
                                {
                                    sr.RestartService(0);
                                    sr.RestartService(1);
                                    sr.RestartService(1);

                                    sr.SetResetPeriod(1);

                                    sr.OnCrashOnly();
                                });

                        hc.Service<MainService>(
                            sc =>
                                {
                                    sc.ConstructUsing(hs => new MainService());

                                    sc.WhenStarted(s => s.Start());
                                    sc.WhenStopped(s => s.Stop());
                                });
                    });

            Environment.ExitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
        }
    }
}