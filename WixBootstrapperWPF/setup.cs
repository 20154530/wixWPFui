using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WixSharp;
using WixSharp.Bootstrapper;
using WixSharp.CommonTasks;
using RegistryHive = WixSharp.RegistryHive;
using ass = System.Reflection.Assembly;
using io = System.IO;
using System.Resources;

namespace WixBootstrapperWPF {


    class setup {
        static void Main(string[] args) {

            var productProj =
                new ManagedProject("My Product",
                new Dir("[INSTALLDIR]",
                    new Dir("bin", new File("ui.config")),
                    new Dir("obj", new File("ui.config"))
                    ),
                new RegValue(RegistryHive.LocalMachine, @"SOFTWARE\My Product", "test", "testvalue"),
                new RemoveRegistryKey(RegistryHive.LocalMachine, @"Software\My Product")) {
                    InstallPrivileges = InstallPrivileges.elevated,
                    InstallScope = InstallScope.perMachine,
                    GUID = new Guid("6a330b47-2577-43ad-9095-1861bb25889a")
                };


            //productProj.Load += (SetupEventArgs e) => {
            //    e.Session.Property("INSTALLDIR");
            //};

            string productMsi = productProj.BuildMsi();

            //------------------------------------

            var bootstrapper =
                new Bundle("TestInstall",
                           new PackageGroupRef("NetFx47Redist"),
                           new MsiPackage(productMsi) {
                               Id = "TestInstall",
                               DisplayInternalUI = false,
                               MsiProperties = @"USERINPUT=[UserInput];INSTALLDIR=[InstallPath]"
                           });

            bootstrapper.WxsFiles.Add("Net47.wxs");
            bootstrapper.Include(WixExtension.Util);
            //bootstrapper.Include(WixExtension.NetFx);
            bootstrapper.AddXmlElement("Wix/Bundle", "Log", "PathVariable=LogFileLocation");
            bootstrapper.Variables = new[] {
                new Variable("LogFileLocation", $"D:\\log.txt"){ Overridable = true },
                new Variable("UserInput", "<none>"),
                new Variable("InstallPath", "[ProgramFiles6432Folder]\\My Company\\My Product"),
            };

            bootstrapper.AddWixFragment("Wix/Bundle",
                                    new UtilRegistrySearch {
                                        Root = RegistryHive.LocalMachine,
                                        Key = @"SOFTWARE\Microsoft\Net Framework Setup\NDP\v4\Full",
                                        Value = "Version",
                                        Variable = "Netfx4FullVersion",
                                        Result = SearchResult.value
                                    });
            bootstrapper.Version = new Version("1.0.0.0");
            bootstrapper.UpgradeCode = new Guid("6a330b47-2577-43ad-9095-1861bb25889a");
            bootstrapper.IconFile = @"Res\test.ico";
            // You can also use System.Reflection.Assembly.GetExecutingAssembly().Location instead of "%this%"
            // Note, passing BootstrapperCore.config is optional and if not provided the default BootstrapperCore.config
            // will be used. The content of the default BootstrapperCore.config can be accessed via
            // ManagedBootstrapperApplication.DefaultBootstrapperCoreConfigContent.
            //
            // Note that the DefaultBootstrapperCoreConfigContent may not be suitable for all build and runtime scenarios.
            // In such cases you may need to use custom BootstrapperCore.config as demonstrated below.
            bootstrapper.Application = new ManagedBootstrapperApplication(typeof(App).Assembly.Location);
            ManagedBootstrapperApplication.DefaultBootstrapperCoreConfigContent = @"<?xml version=""1.0"" encoding=""utf-8"" ?><configuration><configSections><sectionGroup name=""wix.bootstrapper"" type=""Microsoft.Tools.WindowsInstallerXml.Bootstrapper.BootstrapperSectionGroup, BootstrapperCore""><section name=""host"" type=""Microsoft.Tools.WindowsInstallerXml.Bootstrapper.HostSection, BootstrapperCore"" /></sectionGroup></configSections><startup useLegacyV2RuntimeActivationPolicy=""true""><supportedRuntime version=""v4.0"" /></startup><wix.bootstrapper><host assemblyName=""WixBootstrapperWPF"">      <supportedFramework version=""v4\Full"" /><supportedFramework version=""v4\Client"" /></host></wix.bootstrapper></configuration>";
            //var asse = ass.GetExecutingAssembly();
            //var names = asse.GetManifestResourceNames();
            //using (io.StreamReader reader = new io.StreamReader(asse.GetManifestResourceStream("ui.config"))) {
            //    ManagedBootstrapperApplication.DefaultBootstrapperCoreConfigContent = reader.ReadToEnd();
            //}

            bootstrapper.PreserveTempFiles = true;
            bootstrapper.SuppressWixMbaPrereqVars = true;

            bootstrapper.Build("my_app.exe");
        }
    }
}

