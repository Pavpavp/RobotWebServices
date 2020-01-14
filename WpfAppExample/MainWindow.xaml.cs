using RWS;
using RWS.SubscriptionServices;
using System;
using System.Linq;
using System.Net;
using System.Windows;


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
            //Testing with RWS 2.0 with RW7

            ControllerSession rwsCs1 = new ControllerSession(new Address("localhost:80"));

            var ios = rwsCs1.RobotWareService.GetIOSignals7Async().Result;

            var io = ios.Embedded.Resources.FirstOrDefault(io => io.Name.Contains("doSigTest"));

            io.OnValueChanged += IOSignal_ValueChanged;


            ;



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
