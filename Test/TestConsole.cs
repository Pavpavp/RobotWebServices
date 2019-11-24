using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using RWS;
using RWS.Data;
using RWS.SubscriptionServices;
using System;
using System.Linq;

namespace Test
{
    class TestConsole
    {
        static async void Main(string[] args)
        {
            #region Find VC ports with PCSDK
            //var scanner = new NetworkScanner();
            //scanner.Scan();
            //ControllerInfoCollection controllers = scanner.Controllers;
            //var vcPorts = controllers.Where(c => c.IsVirtual).Select(c => c.WebServicesPort);
            #endregion

            ControllerSession rwsCs1 = new ControllerSession("localhost");
            var ios = await rwsCs1.RobotWareService.GetIOSignalsAsync().ConfigureAwait(false);

            var io0 = ios.Embedded.State[0];

            io0.OnValueChanged += IOSignal_ValueChanged;
            Console.ReadKey();

            io0.OnValueChanged -= IOSignal_ValueChanged;
            Console.ReadKey();

            //"/rw/panel/opmode"
            //"/rw/elog/0"
            //rwsCs1.UserService.RequestRmmp(Enums.Privilege.MODIFY);
            //var rmmpState = rwsCs1.UserService.GetRmmpState();
            //rwsCs1.UserService.RegisterUser("SEPARIA", "RobotStudio", "SWE", Enums.LoginType.LOCAL);
            //rwsCs1.UserService.GrantOrDenyRmmp(rmmpState.Embedded.State.First().UserID, Enums.Privilege.MODIFY);
            //rwsCs1.RobotWareService.MastershipRequest();
            //rwsCs1.UserService.CancelHeldOrRequestedRmmp();
            //rwsCs1.ControllerService.Restart(Enums.RestartMode.RESTART);

            //rwsCs1.RobotWareService.SetMechunitForJogging("ROB_R");
            //var sdf = rwsCs1.RobotWareService.GetMotionSystem();
            //rwsCs.FileService.UploadFile(@"C:/Users/SEPARIA/Downloads/Sync14050W.pgf", "$home/Sync14050W.pgf", true);  //Replace with your paths



        }
        private static void IOSignal_ValueChanged(object sender, IOEventArgs args)
        {
            var lvalue = args.LValue;
        }

    }
}
