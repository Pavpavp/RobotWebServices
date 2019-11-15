using RWS.Data;
using RWS.RobotWareServices.StateData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RWS.RobotWareServices
{
    public struct RobotWareService
    {


        public ControllerSession Controller { get; set; }

        public RobotWareService(ControllerSession cs)
        {
            Controller = cs;

        }

        public GenResponse<IONetworksState> GetIONetworks()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };


            return Controller.Call<IONetworksState>(method, "rw/iosystem/networks", dataParameters, urlParameters);

        }

        public GenResponse<IONetworksState> GetIONetwork(string network)
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return Controller.Call<IONetworksState>(method, "rw/iosystem/networks/" + network, dataParameters, urlParameters);

        }

        public GenResponse<IOSignalsState> GetIOsignals()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return Controller.Call<IOSignalsState>(method, "rw/iosystem/signals", dataParameters, urlParameters);

        }

        public GenResponse<IODevicesState> GetIOdevices()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return Controller.Call<IODevicesState>(method, "rw/iosystem/devices", dataParameters, urlParameters);

        }



        public void UpdateIOSignalValue(string network, string device, string signal, string lvalue)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("lvalue", lvalue) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            Controller.Call<IODevicesState>(method, $"rw/iosystem/signals/{network}/{device}/{signal}", dataParameters, urlParameters);

        }

        public void UpdateIOSignalValue(string signal, string lvalue)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("lvalue", lvalue) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            Controller.Call<IODevicesState>(method, $"rw/iosystem/signals/{signal}", dataParameters, urlParameters);

        }


        public GenResponse<string> GetMotionSystemActions()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

            return Controller.Call<string>(method, "rw/motionsystem", dataParameters, urlParameters);

        }

        public GenResponse<List<GetMotionSystemState>> GetMotionSystem()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return Controller.Call<List<GetMotionSystemState>>(method, "rw/motionsystem", dataParameters, urlParameters);

        }

        public GenResponse<SetMechUnitState> SetMechUnit(string mechUnit)
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = { Tuple.Create("mechunit-name", mechUnit) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-mechunit"), Tuple.Create("json", "1") };

            return Controller.Call<SetMechUnitState>(method, "rw/motionsystem", dataParameters, urlParameters);

        }

        public GenResponse<string> GetAllJointSolution(string mechUnit)
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return Controller.Call<string>(method, "rw/motionsystem/mechunits/" + mechUnit, dataParameters, urlParameters);

        }

        public void ActDeactMechUnit(string mechUnit, bool activate)
        {

            string method = "POST";
            string actDeact = activate ? "Activated" : "Deactivated";


            Tuple<string, string>[] dataParameters = { Tuple.Create("mode", actDeact) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            Controller.Call<dynamic>(method, "rw/motionsystem/mechunits/" + mechUnit, dataParameters, urlParameters);

        }

        public void SetMechunitForJogging(string mechUnit)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("mechunit-name", mechUnit) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-mechunit"), Tuple.Create("json", "1") };

            Controller.Call<dynamic>(method, "rw/motionsystem", dataParameters, urlParameters);

        }



        public GenResponse<GetJointTargetState> GetJointTarget(string mechUnit)
        {
            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "CalcJointsFromPose"), Tuple.Create("json", "1") };

            return Controller.Call<GetJointTargetState>(method, "rw/motionsystem/mechunits/" + mechUnit + "/jointtarget", dataParameters, urlParameters);

        }

        public GenResponse<GetJointTargetState> SetRAPID()
        {
            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("value", "2") };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            return Controller.Call<GetJointTargetState>(method, "rw/rapid/symbol/data/RAPID/T_ROB1/user/reg1", dataParameters, urlParameters);

        }

        public GenResponse<GetJointTargetState> MastershipRelease()
        {
            string method = "POST";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "release"), Tuple.Create("json", "1") };

            return Controller.Call<GetJointTargetState>(method, "rw/mastership/rapid", dataParameters, urlParameters);

        }


        public void MastershipRequest()
        {
            string method = "POST";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "request") };

            Controller.Call<dynamic>(method, "rw/mastership", dataParameters, urlParameters);

        }

        public void MastershipDomainRequest(Enums.MastershipDomain domain)
        {
            string method = "POST";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "request") };

            Controller.Call<dynamic>(method, "rw/mastership/" + domain.ToString().ToLowerInvariant(), dataParameters, urlParameters);

        }

        public GenResponse<GetJointTargetState> GetHwDevices()
        {
            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return Controller.Call<GetJointTargetState>(method, "rw/devices/HW_DEVICES/CONTROLLER/COMPUTER_SYSTEM/", dataParameters, urlParameters);

        }

        public GenResponse<GetRobtargetState> GetRobTarget(string mechUnit, [Optional]string tool, [Optional]string wobj, [Optional]string coordinate)
        {
            string method = "GET";


            string tool1 = "tool0";
            string wobj1 = "wobj0";
            string coordinate1 = "Base";

            if (!string.IsNullOrEmpty(tool))
                tool1 = tool;
            if (!string.IsNullOrEmpty(wobj))
                wobj1 = wobj;
            if (!string.IsNullOrEmpty(coordinate))
                coordinate1 = coordinate;

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("tool", tool1), Tuple.Create("wobj", wobj1), Tuple.Create("coordinate", coordinate1), Tuple.Create("json", "1") };

            return Controller.Call<GetRobtargetState>(method, "rw/motionsystem/mechunits/" + mechUnit + "/robtarget", dataParameters, urlParameters);

        }
    }
}
