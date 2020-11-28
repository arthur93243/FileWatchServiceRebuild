using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;

namespace FileWatchService
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FileWatchService()
            };

            if (Environment.UserInteractive)
            {
                RunInteractive(ServicesToRun);
            }
            else
            {
                ServiceBase.Run(ServicesToRun);
            }
        }

        public static void RunInteractive(ServiceBase[] services)
        {
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ \r\n");
            Console.WriteLine("Install the services on interactive mode.");

            // Get the method to invoke on each service to start it
            var onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);

            // Start services loop
            foreach (var service in services)
            {
                onStartMethod.Invoke(service, new object[] { new string[] { } });
            }

            Console.WriteLine("=========================================================================== \r\n");

#if DEBUG
            // Waiting the end
            Console.WriteLine("Press a key to uninstall all services... \r\n");
            Console.ReadKey();
            Console.WriteLine();

            // Get the method to invoke on each service to stop it
            var onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);

            // Stop loop
            foreach (var service in services)
            {
                onStopMethod.Invoke(service, null);
            }

            Console.WriteLine();
            Console.WriteLine("All services are uninstalled. \r\n");
            Console.WriteLine("------------------------------------------------------------------------------ \r\n");

            // Waiting a key press to not return to VS directly
            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.Write("=== Press a key to quit ===");
                Console.ReadKey();
            }
#endif
        }
    }
}
