using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using RWS;
using System.Linq;

namespace Test
{
    class TestProgram
    {
        static void Main(string[] args)
        {
            #region Find VC ports
            var scanner = new NetworkScanner();
            scanner.Scan();
            ControllerInfoCollection controllers = scanner.Controllers;
            var vcPorts = controllers.Where(c => c.IsVirtual).Select(c => c.WebServicesPort);
            #endregion

            ControllerSession rwsCs1 = new ControllerSession("localhost");
         
            rwsCs1.UserService.RequestRmmp(Enums.Privilege.MODIFY);
            var rmmpState = rwsCs1.UserService.GetRmmpState();
            rwsCs1.UserService.RegisterUser("SEPARIA", "RobotStudio", "SWE", Enums.LoginType.LOCAL);
            rwsCs1.UserService.GrantOrDenyRmmp(rmmpState.Embedded.State.First().UserID, Enums.Privilege.MODIFY);
            rwsCs1.RobotWareService.MastershipRequest();
            rwsCs1.UserService.CancelHeldOrRequestedRmmp();
            rwsCs1.ControllerService.Restart(Enums.RestartMode.RESTART);

           //rwsCs1.RobotWareService.SetMechunitForJogging("ROB_R");

           //var sdf = rwsCs1.RobotWareService.GetMotionSystem();



           ;



            //rwsCs.FileService.UploadFile(@"C:/Users/SEPARIA/Downloads/Sync14050W.pgf", "$home/Sync14050W.pgf", true);  //Replace with your paths
            //rwsCs1.SubscriptionService.StartSubscription("/rw/rapid/symbol/data/RAPID/T_ROB1/Module1/PNum;value", 1);
        }
    }
}
