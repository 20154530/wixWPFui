using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using ErrorEventArgs = Microsoft.Tools.WindowsInstallerXml.Bootstrapper.ErrorEventArgs;

namespace WixBootstrapperWPF.ViewModel {


    /// <summary>
    /// MainViewModel
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged, ICommand {

        #region Props

        private static MainViewModel _instance = null;
        private static readonly object _lock = new object();
        public static MainViewModel Singleton {
            get {
                if (_instance == null)
                    lock (_lock)
                        if (_instance == null)
                            _instance = new MainViewModel();
                return _instance;
            }
        }

        private BootstrapperApplication _app;
        public BootstrapperApplication APP => _app;

        private Dispatcher _uidispatcher;


        private ApplyState _state;
        public ApplyState ApplyState {
            get => _state;
            set => SetValue(out _state, value, nameof(ApplyState));
        }

        private bool _isbusy;
        public bool IsBusy {
            get => _isbusy;
            set => SetValue(out _isbusy, value, nameof(IsBusy));
        }

        private string _path;
        public string Path {
            get => _path;
            set => SetValue(out _path, value, nameof(Path));
        }

        private string _output;
        public string Output {
            get => _output;
            set => SetValue(out _output, value, nameof(Output));
        }

        #endregion

        #region Events


        #endregion

        #region Delegates


        #endregion

        #region Methods


        private void Install() {
            IsBusy = true;

            APP.Engine.StringVariables["UserInput"] = "Hello";
            APP.Engine.StringVariables["InstallPath"] = Path;
            APP.Engine.Plan(LaunchAction.Install);
        }

        private void UnInstall() {
            IsBusy = true;
            APP.Engine.Plan(LaunchAction.Uninstall);
        }

        public MainViewModel Init(BootstrapperApplication ba) {
            this._app = ba;
            this._app.Error += this.OnError;
            this._app.ApplyComplete += this.OnApplyComplete;
            this._app.DetectPackageComplete += this.OnDetectPackageComplete;
            //this._app.ExecutePackageBegin += _app_ExecutePackageBegin;
            //this._app.ExecuteMsiMessage += _app_ExecuteMsiMessage;
            this._app.PlanComplete += this.OnPlanComplete;
            //this._app.PlanPackageBegin += _app_PlanPackageBegin;

            this._app.Engine.Detect();
            return this;
        }

        private void _app_PlanPackageBegin(object sender, PlanPackageBeginEventArgs e) {
            Output += $"{e.PackageId} PlanPackageBegin";
            e.Result = Result.None;
        }

        private void _app_ExecutePackageBegin(object sender, ExecutePackageBeginEventArgs e) {
            Output += $"{e.PackageId} ExecutePackageBegin";
            e.Result = Result.None;
        }

        private void _app_ExecuteMsiMessage(object sender, ExecuteMsiMessageEventArgs e) {

        }

        private void OnPlanComplete(object sender, PlanCompleteEventArgs e) {
            if (e.Status >= 0)
                APP.Engine.Apply(IntPtr.Zero);
        }

        private void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e) {
            if (e.PackageId == "TestInstall") {
                try {
                    if (e.State == PackageState.Absent)
                        this.ApplyState = ApplyState.Install;
                    else if (e.State == PackageState.Present)
                        this.ApplyState = ApplyState.Uninstall;
                } catch (Exception ex) {
                    _app.Engine.Log(LogLevel.Error, ex.Message);
                }

            }
        }

        private void OnApplyComplete(object sender, ApplyCompleteEventArgs e) {
            IsBusy = false;
            Output += "OK";
        }

        private void OnError(object sender, ErrorEventArgs e) {
            MessageBox.Show(e.ErrorMessage);
        }

        public MainViewModel HookUiThread(UIElement ui) {
            this._uidispatcher = ui.Dispatcher;
            return this;
        }

        #endregion


        #region Interfaces

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetValue<T>(out T pv, T v, string propn) {
            pv = v;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propn));
        }
        #endregion

        #region ICommand
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            switch (parameter.ToString()) {
                case "Close":

                    break;
                case "Exit":
                    break;
                case "UnInstall":
                    this.UnInstall();
                    break;
                case "Install":
                    this.Install();
                    break;
            }
        }
        #endregion
        #endregion

        #region Constructor
        MainViewModel() {
            _path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        }
        #endregion

    }
}
