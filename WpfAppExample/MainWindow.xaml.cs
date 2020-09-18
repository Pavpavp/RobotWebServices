using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using RWS;
using RWS.IRC5.SubscriptionServices;
using RWS.OmniCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Zeroconf;

namespace WpfAppExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        List<IRC5Session> _ctrlList = new List<IRC5Session>();

        public List<IRC5Session> CtrlList
        {
            get { return _ctrlList; }

            set
            {
                _ctrlList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CtrlList"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();


            Main();


        }


        private async void Main()
        {
            var rwsIrc5Session = new IRC5Session(new Address("localhost:80"));

            var rwsOmniSession = new OmniCoreSession(new Address("localhost:80"));
            var ios = await rwsOmniSession.RobotWareService.GetIOSignalsAsync();


            //var rwsCs6 = new IRC5Session(new Address($"{vc6.IPAddress}:{vc6.WebServicesPort}"));
            //////var info6 = await rwsCs6.RobotWareService.GetSystemInformationAsync();
            //var ios6 = await rwsCs6.RobotWareService.GetIOSignalsAsync();
            //var io6 = ios6.Embedded.State.FirstOrDefault(io => io.Name.Contains("doSigTest"));

            //io6.OnValueChanged += IOSignal_ValueChanged;
            //io7.OnValueChanged += IOSignal_ValueChanged;

            //var ios = await rwsCs1.RobotWareService.GetSystemInformationAsync();


            // var ios = await rwsCs1.RobotWareService.GetIOSignalsAsync();

            //var dev = await rwsCs1.RobotWareService.GetIODevicesAsync();
            //var dev2 = await rwsCs1.RobotWareService.GetIODevicesAsync();
            //  rwsCs1.UserService.RequestRmmpAsync(Enums.Privilege.MODIFY);
            //var rmmpState = await rwsCs1.UserService.GetRmmpStateAsync().ConfigureAwait(false);
            //await rwsCs1.UserService.RegisterUserAsync("SEPARIA", "RobotStudio", "SWE", Enums.LoginType.LOCAL).ConfigureAwait(false);
            //await rwsCs1.UserService.GrantOrDenyRmmpAsync(rmmpState.Embedded.State.First().UserID, Enums.Privilege.MODIFY).ConfigureAwait(false);
            //rwsCs1.RobotWareService.MastershipRequest();
            //rwsCs1.UserService.CancelHeldOrRequestedRmmp();
            //rwsCs1.ControllerService.Restart(Enums.RestartMode.RESTART);


        }



        private async Task GetSysInfo(IRC5Session c)
        {

            if (!c.IsOmnicore)
            {
                var sysInfo = await c.RobotWareService.GetSystemInformationAsync();

                c.Version = sysInfo.Embedded.State.First().RWVersionName;
                c.CtrlName = sysInfo.Embedded.State.First().Name;
            }
            else
            {
                var sysInfo = await ((OmniCoreSession)c).RobotWareService.GetSystemInformationAsync();

                c.Version = sysInfo.State.First().RWVersionName;
                c.CtrlName = sysInfo.State.First().Name;

            }

            if (c.Version == null)
                return;

            CtrlList.Add(c);

            ListView_CtrlList.ItemsSource = null;
            ListView_CtrlList.ItemsSource = CtrlList;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListView_CtrlList.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Version", ListSortDirection.Ascending));
        }

        private void IOSignal_ValueChanged(object source, IOEventArgs args)
        {
            // CheckBox_sig.IsChecked = args.ValueChanged == 1 ? true : false;
            ;
        }

        private void Button_Scan_Click(object sender, RoutedEventArgs e)
        {
            CtrlScan();
        }


        private async void CtrlScan()
        {
            var scanner = new NetworkScanner();
            scanner.Scan();
            ControllerInfoCollection controllers = scanner.Controllers;

            var taskList = new List<Task>();

            foreach (ControllerInfo ctrl in controllers)
            {

                if (ctrl.VersionName.Contains("6."))
                {
                    var c = new IRC5Session(new Address($"{ctrl.IPAddress}{(ctrl.IsVirtual ? ":" + ctrl.WebServicesPort.ToString() : string.Empty)}"));

                    taskList.Add(GetSysInfo(c));

                }
                else if (ctrl.VersionName.Contains("7."))
                {
                    var c = new OmniCoreSession(new Address($"{ctrl.IPAddress}{(ctrl.IsVirtual ? ":" + ctrl.WebServicesPort.ToString() : string.Empty)}"));

                    taskList.Add(GetSysInfo(c));

                }
            }


            Task t = Task.WhenAll(taskList);
            try
            {
                await t;
            }
            catch
            {
                ;
            }

        }
    }
}
