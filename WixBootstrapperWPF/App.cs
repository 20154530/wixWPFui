
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using WixBootstrapperWPF;
using WixBootstrapperWPF.View;
using WixBootstrapperWPF.ViewModel;

[assembly: BootstrapperApplication(typeof(App))]

namespace WixBootstrapperWPF {


    public class App : BootstrapperApplication {


        protected override void Run() {

            MainViewModel.Singleton.Init(this);
            MainWindow window = new MainWindow();
            window.ShowDialog();
            this.Engine.Quit(0);
        }
    }
}
