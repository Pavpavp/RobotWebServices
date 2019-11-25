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


            #region Find VC ports with PCSDK
            //var scanner = new NetworkScanner();
            //scanner.Scan();
            //ControllerInfoCollection controllers = scanner.Controllers;
            //var vcPorts = controllers.Where(c => c.IsVirtual).Select(c => c.WebServicesPort);
            #endregion

            ControllerSession rwsCs1 = new ControllerSession(new Address("localhost:80"));

            RequestRmmpAsync(rwsCs1);


            //var io0 = GetIOSignalsAsync(rwsCs1).Result.Embedded.State[0];

            //io0.OnValueChanged += IOSignal_ValueChanged;
            Console.ReadKey();

            //io0.OnValueChanged -= IOSignal_ValueChanged;
            //Console.ReadKey();


            //"/rw/panel/opmode"
            //"/rw/elog/0"

            //rwsCs1.RobotWareService.MastershipRequest();
            //rwsCs1.UserService.CancelHeldOrRequestedRmmp();
            //rwsCs1.ControllerService.Restart(Enums.RestartMode.RESTART);

            //rwsCs1.RobotWareService.SetMechunitForJogging("ROB_R");
            //var sdf = rwsCs1.RobotWareService.GetMotionSystem();
            //rwsCs.FileService.UploadFile(@"C:/Users/SEPARIA/Downloads/Sync14050W.pgf", "$home/Sync14050W.pgf", true);  //Replace with your paths



        }

        private async static Task<BaseResponse<IOSignalsState>> GetIOSignalsAsync(ControllerSession rwsCs1)
        {
            return await rwsCs1.RobotWareService.GetIOSignalsAsync().ConfigureAwait(false);
        }

        private async static void RequestRmmpAsync(ControllerSession rwsCs1)
        {
            await rwsCs1.UserService.RequestRmmpAsync(Enums.Privilege.MODIFY).ConfigureAwait(false);
            var rmmpState = await rwsCs1.UserService.GetRmmpStateAsync().ConfigureAwait(false);
            await rwsCs1.UserService.RegisterUserAsync("SEPARIA", "RobotStudio", "SWE", Enums.LoginType.LOCAL).ConfigureAwait(false);
            await rwsCs1.UserService.GrantOrDenyRmmpAsync(rmmpState.Embedded.State.First().UserID, Enums.Privilege.MODIFY).ConfigureAwait(false);

            ;
        }

        private static void IOSignal_ValueChanged(object sender, IOEventArgs args)
        {
            var lvalue = args.LValue;
        }
    }
}
