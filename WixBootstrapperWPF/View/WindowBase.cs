using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace WixBootstrapperWPF.View {



    public class WindowBase : Window {

        #region Props
        private HwndSource _handle;

        protected HwndSource Handle {
            get { return _handle; }
            set { _handle = value; }
        }

        #endregion

        #region Events

        #endregion

        #region Delegates
        #endregion

        #region Methods

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {

            return IntPtr.Zero;
        }
        #endregion

        #region Interfaces
        #endregion

        #region Constructor

        public WindowBase() {
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            SourceInitialized += (s, e) => {
                Handle = (PresentationSource.FromVisual((Visual)s) as HwndSource);
                if(Handle != null) {
                    Handle.AddHook(WndProc);
                }
            };
        }



        static WindowBase() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowBase), new FrameworkPropertyMetadata(typeof(WindowBase)));
        }
        #endregion

    }
}
