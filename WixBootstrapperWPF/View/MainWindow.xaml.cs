using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

using WixBootstrapperWPF.Res;
using WixBootstrapperWPF.ViewModel;

namespace WixBootstrapperWPF.View {


    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : WindowBase {


        public MainWindow() {
            try {
                InitializeComponent();
                this.Resources.MergedDictionaries.Add(MainRes.Singleton);
            } catch (Exception e) {
                MainViewModel.Singleton.APP.Engine.Log(LogLevel.Error, e.Message);
            }

        }


        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {



            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
