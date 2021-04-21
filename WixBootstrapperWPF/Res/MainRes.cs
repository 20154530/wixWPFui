using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WixBootstrapperWPF.Res {
    /// <summary>
    /// 单例资源字典
    /// </summary>
    public class MainRes : ResourceDictionary {
        private static MainRes _res;
        private static readonly object _lock = new object();
        public static MainRes Singleton {
            get {
                if (_res == null)
                    lock (_lock)
                        if (_res == null)
                            _res = new MainRes();
                return _res;
            }
        }

        private MainRes() {
            this.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(@"/WixBootstrapperWPF;component/Themes/Themes.xaml", UriKind.RelativeOrAbsolute)});
        }
    }
}
