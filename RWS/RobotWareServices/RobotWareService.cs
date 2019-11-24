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
    public class RobotWareService
    {
        public ControllerSession ControllerSession { get; set; }

        public RobotWareService(ControllerSession cs)
        {
            ControllerSession = cs;
        }

        public async Task<BaseResponse<IONetworksState>> GetIONetworksAsync()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };


            return await ControllerSession.CallAsync<IONetworksState>(method, "rw/iosystem/networks", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<IONetworksState>> GetIONetworkAsync(string network)
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<IONetworksState>(method, "rw/iosystem/networks/" + network, dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<IOSignalsState>> GetIOSignalsAsync()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            var ioResp = await ControllerSession.CallAsync<IOSignalsState>(method, "rw/iosystem/signals", dataParameters, urlParameters).ConfigureAwait(false);

            foreach (var ioState in ioResp.Embedded.State)
                ioState.ControllerSession = ControllerSession;


            return ioResp;
        }

        public async Task<BaseResponse<IODevicesState>> GetIOdevicesAsync()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<IODevicesState>(method, "rw/iosystem/devices", dataParameters, urlParameters).ConfigureAwait(false);

        }



        public async Task UpdateIOSignalValueAsync(string network, string device, string signal, string lvalue)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("lvalue", lvalue) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<IODevicesState>(method, $"rw/iosystem/signals/{network}/{device}/{signal}", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task UpdateIOSignalValueAsync(string signal, string lvalue)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("lvalue", lvalue) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<IODevicesState>(method, $"rw/iosystem/signals/{signal}", dataParameters, urlParameters).ConfigureAwait(false);

        }


        public async Task<BaseResponse<string>> GetMotionSystemActionsAsync()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<string>(method, "rw/motionsystem", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<List<GetMotionSystemState>>> GetMotionSystemAsync()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<List<GetMotionSystemState>>(method, "rw/motionsystem", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<SetMechUnitState>> SetMechUnitAsync(string mechUnit)
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = { Tuple.Create("mechunit-name", mechUnit) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-mechunit"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<SetMechUnitState>(method, "rw/motionsystem", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<string>> GetAllJointSolutionAsync(string mechUnit)
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<string>(method, "rw/motionsystem/mechunits/" + mechUnit, dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task ActDeactMechUnitAsync(string mechUnit, bool activate)
        {

            string method = "POST";
            string actDeact = activate ? "Activated" : "Deactivated";


            Tuple<string, string>[] dataParameters = { Tuple.Create("mode", actDeact) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<dynamic>(method, "rw/motionsystem/mechunits/" + mechUnit, dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task SetMechunitForJoggingAsync(string mechUnit)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("mechunit-name", mechUnit) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-mechunit"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<dynamic>(method, "rw/motionsystem", dataParameters, urlParameters).ConfigureAwait(false);

        }



        public async Task<BaseResponse<GetJointTargetState>> GetJointTargetAsync(string mechUnit)
        {
            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "CalcJointsFromPose"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<GetJointTargetState>(method, "rw/motionsystem/mechunits/" + mechUnit + "/jointtarget", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<GetJointTargetState>> SetRAPIDAsync()
        {
            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("value", "2") };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<GetJointTargetState>(method, "rw/rapid/symbol/data/RAPID/T_ROB1/user/reg1", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<GetJointTargetState>> MastershipReleaseAsync()
        {
            string method = "POST";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "release"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<GetJointTargetState>(method, "rw/mastership/rapid", dataParameters, urlParameters).ConfigureAwait(false);

        }


        public async Task MastershipRequestAsync()
        {
            string method = "POST";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "request") };

            await ControllerSession.CallAsync<dynamic>(method, "rw/mastership", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task MastershipDomainRequestAsync(Enums.MastershipDomain domain)
        {
            string method = "POST";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "request") };

            await ControllerSession.CallAsync<dynamic>(method, "rw/mastership/" + domain.ToString().ToLowerInvariant(), dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<GetJointTargetState>> GetHwDevicesAsync()
        {
            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<GetJointTargetState>(method, "rw/devices/HW_DEVICES/CONTROLLER/COMPUTER_SYSTEM/", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<GetRobtargetState>> GetRobTargetAsync(string mechUnit, [Optional]string tool, [Optional]string wobj, [Optional]string coordinate)
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

            return await ControllerSession.CallAsync<GetRobtargetState>(method, "rw/motionsystem/mechunits/" + mechUnit + "/robtarget", dataParameters, urlParameters).ConfigureAwait(false);

        }
    }
}
