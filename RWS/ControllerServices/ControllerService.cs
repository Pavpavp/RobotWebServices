using RWS.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RWS.ControllerSession;
using static RWS.Enums;

namespace RWS
{
    public class ControllerService
    {

        public ControllerSession Controller { get; set; }
        protected ClockOperations ClockOps { get; set; }
        protected IdentityOperations IdentityOps { get; set; }
        public ControllerService(ControllerSession cs)
        {
            Controller = cs;

            ClockOps = new ClockOperations(Controller);
            IdentityOps = new IdentityOperations(Controller);
        }

        public GenResponse<ControllerResourcesState> GetControllerResources()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return Controller.Call<ControllerResourcesState>(method, "ctrl", dataParameters, urlParameters);

        }


        public GenResponse<ControllerResourcesState> GetControllerActions()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

            return Controller.Call<ControllerResourcesState>(method, "ctrl", dataParameters, urlParameters);

        }

        public void Restart(RestartMode restartMode)
        {

            string method = "POST";
            string rstMode = GetRestartModeString(restartMode);

            Tuple<string, string>[] dataParameters = { Tuple.Create("restart-mode", rstMode) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            Controller.Call<dynamic>(method, "ctrl", dataParameters, urlParameters);

        }


        protected class ClockOperations
        {
            public ControllerSession Controller { get; set; }
            public ClockOperations(ControllerSession cs)
            {
                Controller = cs;
            }

            public GenResponse<ClockResourceState> GetClockResource()
            {

                string method = "GET";

                Tuple<string, string>[] dataParameters = null;
                Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

                return Controller.Call<ClockResourceState>(method, "ctrl/clock", dataParameters, urlParameters);

            }

            public GenResponse<TimeZoneResourceState> GetTimeZoneResource()
            {

                string method = "GET";

                Tuple<string, string>[] dataParameters = null;
                Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

                return Controller.Call<TimeZoneResourceState>(method, "ctrl/timezone", dataParameters, urlParameters);

            }

            public GenResponse<ClockActionsState> GetClockActions()
            {

                string method = "GET";

                Tuple<string, string>[] dataParameters = null;
                Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

                return Controller.Call<ClockActionsState>(method, "ctrl/clock", dataParameters, urlParameters);

            }

            public GenResponse<TimeZoneActionsState> GetTimeZoneActions()
            {

                string method = "GET";

                Tuple<string, string>[] dataParameters = null;
                Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

                return Controller.Call<TimeZoneActionsState>(method, "ctrl/timezone", dataParameters, urlParameters);

            }

            public void SetTimeZone(string timeZone)
            {

                string method = "POST";

                Tuple<string, string>[] dataParameters = { Tuple.Create("timezone", timeZone) };

                Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-timezone"), Tuple.Create("json", "1") };

                var response = Controller.Call<dynamic>(method, "ctrl/time", dataParameters, urlParameters);

            }

            public void SetControllerClock(DateTime date)
            {

                string method = "PUT";

                Tuple<string, string>[] dataParameters = {
                                        Tuple.Create("sys-clock-year", date.Year.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-month", date.Month.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-day", date.Day.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-hour", date.Hour.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-min", date.Minute.ToString("00", CultureInfo.InvariantCulture)),
                                        Tuple.Create("sys-clock-sec", date.Second.ToString("00", CultureInfo.InvariantCulture))
                                    };

                Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

                Controller.Call<dynamic>(method, "ctrl/clock", dataParameters, urlParameters);

            }


            public GenResponse<TimeServerResourceState> GetTimeServerResource()
            {

                string method = "GET";

                Tuple<string, string>[] dataParameters = null;
                Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

                return Controller.Call<TimeServerResourceState>(method, "ctrl/clock/timeserver", dataParameters, urlParameters);

            }

            public GenResponse<TimeServerActionsState> GetTimeServerActions()
            {

                string method = "GET";

                Tuple<string, string>[] dataParameters = null;
                Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

                return Controller.Call<TimeServerActionsState>(method, "ctrl/timeserver", dataParameters, urlParameters);

            }


            public void SetTimeServer(string timeServer)
            {

                string method = "POST";

                Tuple<string, string>[] dataParameters = { Tuple.Create("timeserver", timeServer) };

                Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-timeserver"), Tuple.Create("json", "1") };

                var response = Controller.Call<dynamic>(method, "ctrl/clock/timeserver", dataParameters, urlParameters);

            }

        }



        protected class IdentityOperations
        {
            public ControllerSession Controller { get; set; }
            public IdentityOperations(ControllerSession cs)
            {
                Controller = cs;
            }

            public GenResponse<IdentityResourceState> GetIdentityResource()
            {

                string method = "GET";

                Tuple<string, string>[] dataParameters = null;
                Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

                return Controller.Call<IdentityResourceState>(method, "ctrl/identity", dataParameters, urlParameters);

            }

            public GenResponse<IdentityActionsState> GetIdentityActions()
            {

                string method = "GET";

                Tuple<string, string>[] dataParameters = null;
                Tuple<string, string>[] urlParameters = { Tuple.Create("action", "show"), Tuple.Create("json", "1") };

                return Controller.Call<IdentityActionsState>(method, "ctrl/identity", dataParameters, urlParameters);

            }

            public void SetIdentity(string controllerName, string controllerID)
            {

                string method = "PUT";

                Tuple<string, string>[] dataParameters = { Tuple.Create("ctrl-name", controllerName), Tuple.Create("ctrl-id", controllerID) };

                Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

                var response = Controller.Call<dynamic>(method, "ctrl/identity", dataParameters, urlParameters);

            }


        }

    }

}
