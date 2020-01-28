using RWS.IRC5.ResponseTypes;
using RWS.IRC5.RobotWareServices.ResponseTypes;
using RWS.OmniCore;
using RWS.OmniCore.ResponseTypes;
using System;
using System.Threading.Tasks;
using static RWS.Enums;

namespace RobotWebServices.OmniCoreServices.RobotWareServices
{
    public class RobotWareService
    {
        public OmniCoreSession ControllerSession { get; set; }

        public RobotWareService(OmniCoreSession cs)
        {
            ControllerSession = cs;
        }


        public async Task<BaseResponse<Resource7, SystemInformationState7>> GetSystemInformationAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<Resource7, SystemInformationState7>(RequestMethod.GET, "rw/system", dataParameters, urlParameters).ConfigureAwait(false);


        }

        public async Task<BaseResponse<IOSignalsResource, Resource7>> GetIOSignalsAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            var ioResp = await ControllerSession.CallAsync<IOSignalsResource, Resource7>(RequestMethod.GET, "rw/iosystem/signals", dataParameters, urlParameters).ConfigureAwait(false);

            foreach (var res in ioResp.Embedded.Resources)
                res.ControllerSession = ControllerSession;

            return ioResp;
        }

    }
}
