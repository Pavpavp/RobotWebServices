using RWS.Data;
using System;
using System.Globalization;
using System.Threading.Tasks;
using static RWS.Enums;

namespace RWS
{
    public class ControllerService
    {

        public ControllerSession ControllerSession { get; set; }
        public ClockOperations ClockOps { get; set; }
        public IdentityOperations IdentityOps { get; set; }
        public ControllerService(ControllerSession cs)
        {
            ControllerSession = cs;

            ClockOps = new ClockOperations(ControllerSession);
            IdentityOps = new IdentityOperations(ControllerSession);
        }

        public async Task<BaseResponse<ControllerResourcesState>> GetControllerResourcesAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<ControllerResourcesState>(RequestMethod.GET, "ctrl", dataParameters, urlParameters).ConfigureAwait(false);

        }


        public Task<BaseResponse<ControllerResourcesState>> GetControllerActionsAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

            return ControllerSession.CallAsync<ControllerResourcesState>(RequestMethod.GET, "ctrl", dataParameters, urlParameters);

        }

        public async Task RestartAsync(RestartMode restartMode)
        {
            string rstMode = GetRestartModeString(restartMode);

            Tuple<string, string>[] dataParameters = { Tuple.Create("restart-mode", rstMode) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<dynamic>(RequestMethod.POST, "ctrl", dataParameters, urlParameters).ConfigureAwait(false);

        }

    }

    public class ClockOperations
    {
        public ControllerSession ControllerSession { get; set; }
        public ClockOperations(ControllerSession cs)
        {
            ControllerSession = cs;
        }

        public Task<BaseResponse<ClockResourceState>> GetClockResourceAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return ControllerSession.CallAsync<ClockResourceState>(RequestMethod.GET, "ctrl/clock", dataParameters, urlParameters);

        }

        public Task<BaseResponse<TimeZoneResourceState>> GetTimeZoneResourceAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return ControllerSession.CallAsync<TimeZoneResourceState>(RequestMethod.GET, "ctrl/timezone", dataParameters, urlParameters);

        }

        public async Task<BaseResponse<ClockActionsState>> GetClockActionsAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<ClockActionsState>(RequestMethod.GET, "ctrl/clock", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<TimeZoneActionsState>> GetTimeZoneActionsAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<TimeZoneActionsState>(RequestMethod.GET, "ctrl/timezone", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task SetTimeZoneAsync(string timeZone)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("timezone", timeZone) };

            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-timezone"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<dynamic>(RequestMethod.POST, "ctrl/time", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task SetControllerClockAsync(DateTime date)
        {

            Tuple<string, string>[] dataParameters = {
                                        Tuple.Create("sys-clock-year", date.Year.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-month", date.Month.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-day", date.Day.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-hour", date.Hour.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-min", date.Minute.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-sec", date.Second.ToString("00", CultureInfo.InvariantCulture))
                                    };

            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<dynamic>(RequestMethod.PUT, "ctrl/clock", dataParameters, urlParameters).ConfigureAwait(false);

        }


        public async Task<BaseResponse<TimeServerResourceState>> GetTimeServerResourceAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<TimeServerResourceState>(RequestMethod.GET, "ctrl/clock/timeserver", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<TimeServerActionsState>> GetTimeServerActionsAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<TimeServerActionsState>(RequestMethod.GET, "ctrl/timeserver", dataParameters, urlParameters).ConfigureAwait(false);

        }


        public async Task SetTimeServerAsync(string timeServer)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("timeserver", timeServer) };

            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-timeserver"), Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<dynamic>(RequestMethod.POST, "ctrl/clock/timeserver", dataParameters, urlParameters).ConfigureAwait(false);

        }

    }


    public class IdentityOperations
    {
        public ControllerSession ControllerSession { get; set; }
        public IdentityOperations(ControllerSession cs)
        {
            ControllerSession = cs;
        }

        public async Task<BaseResponse<IdentityResourceState>> GetIdentityResourceAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<IdentityResourceState>(RequestMethod.GET, "ctrl/identity", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task<BaseResponse<IdentityActionsState>> GetIdentityActionsAsync()
        {

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

            return await ControllerSession.CallAsync<IdentityActionsState>(RequestMethod.GET, "ctrl/identity", dataParameters, urlParameters).ConfigureAwait(false);

        }

        public async Task SetIdentityAsync(string controllerName, string controllerID)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("ctrl-name", controllerName), Tuple.Create("ctrl-id", controllerID) };

            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            await ControllerSession.CallAsync<dynamic>(RequestMethod.PUT, "ctrl/identity", dataParameters, urlParameters).ConfigureAwait(false);

        }

    }



}
