using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using RWS;
using RWS.SubscriptionServices;
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

namespace WpfAppExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            Main();





        }

        private async void Main()
        {

            //#region Find VC ports with PCSDK
            //var scanner = new NetworkScanner();
            //scanner.Scan();
            //ControllerInfoCollection controllers = scanner.Controllers;
            //var vcPorts = controllers.Where(c => c.IsVirtual).Select(c => c.WebServicesPort);
            //#endregion


            var sdf = await ControllerDiscovery.Discover();

            ControllerSession rwsCs1 = new ControllerSession(new Address("localhost:80"));


            var io0 = rwsCs1.RobotWareService.GetIOSignalsAsync().Result;

            io0.Embedded.State.FirstOrDefault(io => io.Name == "doSigTest").OnValueChanged += IOSignal_ValueChanged;








            // var dev = rwsCs1.RobotWareService.GetIODevicesAsync().Result;




            //  rwsCs1.UserService.RequestRmmpAsync(Enums.Privilege.MODIFY);
            //var rmmpState = await rwsCs1.UserService.GetRmmpStateAsync().ConfigureAwait(false);
            //await rwsCs1.UserService.RegisterUserAsync("SEPARIA", "RobotStudio", "SWE", Enums.LoginType.LOCAL).ConfigureAwait(false);
            //await rwsCs1.UserService.GrantOrDenyRmmpAsync(rmmpState.Embedded.State.First().UserID, Enums.Privilege.MODIFY).ConfigureAwait(false);
            //rwsCs1.RobotWareService.MastershipRequest();
            //rwsCs1.UserService.CancelHeldOrRequestedRmmp();
            //rwsCs1.ControllerService.Restart(Enums.RestartMode.RESTART);





        }

        private void IOSignal_ValueChanged(object source, IOEventArgs args)
        {
            CheckBox_sig.IsChecked = args.ValueChanged == 1 ? true : false;

        }


    }
}
