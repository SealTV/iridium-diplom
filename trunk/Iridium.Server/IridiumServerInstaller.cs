namespace Iridium.Server
{
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.Linq;
    using System.Reflection;
    using System.ServiceProcess;

    [RunInstaller(true)]
    public class IridiumServerInstaller : Installer
    {
        public IridiumServerInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            //set the privileges
            processInstaller.Account = ServiceAccount.User;

            serviceInstaller.ServiceName = IridiumMasterServer.ServiceApplicationName;
            serviceInstaller.DisplayName = "Irirdium Master Server";
            serviceInstaller.Description = "Network service that processes Irirdium Master Server.";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }

        public static bool IsInstalled()
        {
            return ServiceController.GetServices().Any(sc => sc.ServiceName == IridiumMasterServer.ServiceApplicationName);
        }

        internal static void Install()
        {
            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetCallingAssembly().Location });
        }

        internal static void Uninstall()
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/U", Assembly.GetCallingAssembly().Location });
        }
    }
}
