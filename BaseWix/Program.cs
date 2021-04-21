using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WixSharp;

namespace BaseWix {
    public class Program {
        public static void Main() {


            var productProj =
                new ManagedProject("My Product",
                new Dir(@"%ProgramFiles%\My Company\My Product", new File("readme.txt")),
                new RegValue( RegistryHive.LocalMachineOrUsers, @"SOFTWARE\My Product", "test", "testvalue"),
                new RemoveRegistryKey(RegistryHive.LocalMachineOrUsers, @"Software\My Product"));

            productProj.InstallScope = InstallScope.perMachine;
            productProj.GUID = new Guid("6f330b47-2577-43ad-9095-1861bb258777");


            string productMsi = productProj.BuildMsi();
        }
    }
}
