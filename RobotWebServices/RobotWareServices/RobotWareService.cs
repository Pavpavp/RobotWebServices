using RWS.Data;
using RWS.RobotWareServices.StateData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RWS.Enums;

namespace RWS.RobotWareServices
{
    public class RobotWareService
    {
        public ControllerSession ControllerSession { get; set; }

        public RobotWareService(ControllerSession cs)
        {
            ControllerSession = cs;
        }

        public async Task<BaseResponse7<Resource7, SystemInformationState7>> GetSystemInformation7Async()
        {

            Tuple<string, string>[] dataParameters = null;
            //Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };
            Tuple<string, string>[] urlParameters = null;

            return await ControllerSession.Call7Async<Resource7, SystemInformationState7>(RequestMethod.GET, "rw/system", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<GetSystemInformationState>> GetSystemInformationAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };


            return await ControllerSession.CallAsync<GetSystemInformationState>(RequestMethod.GET, "rw/system", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<IONetworksState>> GetIONetworksAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };


            return await ControllerSession.CallAsync<IONetworksState>(RequestMethod.GET, "rw/iosystem/networks", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<IONetworksState>> GetIONetworkAsync(string network)
        {
            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<IONetworksState>(RequestMethod.GET, "rw/iosystem/networks/" + network, dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<IOSignalsState>> GetIOSignalsAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            var ioResp = await ControllerSession.CallAsync<IOSignalsState>(RequestMethod.GET, "rw/iosystem/signals", dataParameters, urlParameters).ConfigureAwait(false);

            foreach (var ioState in ioResp.Embedded.State)
                ioState.ControllerSession = ControllerSession;


            return ioResp;
        }

        public async Task<BaseResponse7<IOSignalsResource, Resource7>> GetIOSignals7Async()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            var ioResp = await ControllerSession.Call7Async<IOSignalsResource, Resource7>(RequestMethod.GET, "rw/iosystem/signals", dataParameters, urlParameters).ConfigureAwait(false);

            foreach (var res in ioResp.Embedded.Resources)
                res.ControllerSession = ControllerSession;


            return ioResp;
        }

        public async Task<BaseResponse<IODevicesState>> GetIODevicesAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<IODevicesState>(RequestMethod.GET, "rw/iosystem/devices", dataParameters, urlParameters).ConfigureAwait(false);

        }



        public async Task UpdateIOSignalValueAsync(string network, string device, string signal, string lvalue)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("lvalue", lvalue) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<IODevicesState>(RequestMethod.POST, $"rw/iosystem/signals/{network}/{device}/{signal}", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task UpdateIOSignalValueAsync(string signal, string lvalue)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("lvalue", lvalue) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<IODevicesState>(RequestMethod.POST, $"rw/iosystem/signals/{signal}", dataParameters, urlParameters).ConfigureAwait(false);

        }


        public async Task<BaseResponse<string>> GetMotionSystemActionsAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<string>(RequestMethod.GET, "rw/motionsystem", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<List<GetMotionSystemState>>> GetMotionSystemAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<List<GetMotionSystemState>>(RequestMethod.GET, "rw/motionsystem", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<SetMechUnitState>> SetMechUnitAsync(string mechUnit)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("mechunit-name", mechUnit) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-mechunit"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<SetMechUnitState>(RequestMethod.GET, "rw/motionsystem", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<string>> GetAllJointSolutionAsync(string mechUnit)
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<string>(RequestMethod.GET, "rw/motionsystem/mechunits/" + mechUnit, dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task ActDeactMechUnitAsync(string mechUnit, bool activate)
        {

            string actDeact = activate ? "Activated" : "Deactivated";


            Tuple<string, string>[] dataParameters = { Tuple.Create("mode", actDeact) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<dynamic>(RequestMethod.POST, "rw/motionsystem/mechunits/" + mechUnit, dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task SetMechunitForJoggingAsync(string mechUnit)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("mechunit-name", mechUnit) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-mechunit"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<dynamic>(RequestMethod.POST, "rw/motionsystem", dataParameters, urlParameters).ConfigureAwait(false);

        }



        public async Task<BaseResponse<GetJointTargetState>> GetJointTargetAsync(string mechUnit)
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "CalcJointsFromPose"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<GetJointTargetState>(RequestMethod.GET, "rw/motionsystem/mechunits/" + mechUnit + "/jointtarget", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<GetJointTargetState>> SetRAPIDAsync()
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("value", "2") };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<GetJointTargetState>(RequestMethod.POST, "rw/rapid/symbol/data/RAPID/T_ROB1/user/reg1", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<GetJointTargetState>> MastershipReleaseAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "release"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<GetJointTargetState>(RequestMethod.POST, "rw/mastership/rapid", dataParameters, urlParameters).ConfigureAwait(false);

        }


        public async Task MastershipRequestAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "request") };

            await ControllerSession.CallAsync<dynamic>(RequestMethod.POST, "rw/mastership", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task MastershipDomainRequestAsync(Enums.MastershipDomain domain)
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "request") };

            await ControllerSession.CallAsync<dynamic>(RequestMethod.POST, "rw/mastership/" + domain.ToString().ToLowerInvariant(), dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<GetJointTargetState>> GetHwDevicesAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<GetJointTargetState>(RequestMethod.GET, "rw/devices/HW_DEVICES/CONTROLLER/COMPUTER_SYSTEM/", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<GetRobtargetState>> GetRobTargetAsync(string mechUnit, [Optional]string tool, [Optional]string wobj, [Optional]string coordinate)
        {

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

            return await ControllerSession.CallAsync<GetRobtargetState>(RequestMethod.GET, "rw/motionsystem/mechunits/" + mechUnit + "/robtarget", dataParameters, urlParameters).ConfigureAwait(false);

        }
    }
}
