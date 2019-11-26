using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using RWS;
using RWS.Data;
using RWS.SubscriptionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync();
        }

        private async static void MainAsync()
        {
            #region Find VC ports with PCSDK
            var scanner = new NetworkScanner();
            scanner.Scan();
            ControllerInfoCollection controllers = scanner.Controllers;
            var vcPorts = controllers.Where(c => c.IsVirtual).Select(c => c.WebServicesPort);
            #endregion

            ControllerSession rwsCs1 = new ControllerSession(new Address("localhost"));
            var dev = rwsCs1.RobotWareService.GetIOdevicesAsync().Result;

          //  rwsCs1.UserService.RequestRmmpAsync(Enums.Privilege.MODIFY);
            //var rmmpState = await rwsCs1.UserService.GetRmmpStateAsync().ConfigureAwait(false);
            //await rwsCs1.UserService.RegisterUserAsync("SEPARIA", "RobotStudio", "SWE", Enums.LoginType.LOCAL).ConfigureAwait(false);
            //await rwsCs1.UserService.GrantOrDenyRmmpAsync(rmmpState.Embedded.State.First().UserID, Enums.Privilege.MODIFY).ConfigureAwait(false);
            //rwsCs1.RobotWareService.MastershipRequest();
            //rwsCs1.UserService.CancelHeldOrRequestedRmmp();
            //rwsCs1.ControllerService.Restart(Enums.RestartMode.RESTART);



            var io0 = rwsCs1.RobotWareService.GetIOSignalsAsync().Result;

            io0.Embedded.State[0].OnValueChanged += IOSignal_ValueChanged;
            Console.ReadKey();

            io0.Embedded.State[0].OnValueChanged -= IOSignal_ValueChanged;
            Console.ReadKey();



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
