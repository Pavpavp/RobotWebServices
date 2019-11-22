using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS
{
    public static class Enums
    {
        public enum TimeZone
        {
            EUROPE_STOCKHOLM,
            //EUROPE_STOCKHOLM,
            //EUROPE_STOCKHOLM,
            //EUROPE_STOCKHOLM,
            //EUROPE_STOCKHOLM,
            //EUROPE_STOCKHOLM
        }



        public static string GetTimeZoneString(TimeZone timeZone)
        {

            switch (timeZone)
            {
                case TimeZone.EUROPE_STOCKHOLM:
                    return "Europe/Stockholm";
                //case TimeZones.EUROPE_STOCKHOLM:
                //    return "Europe/Stockholm";
                //case TimeZones.EUROPE_STOCKHOLM:
                //    return "Europe/Stockholm";
                //case TimeZones.EUROPE_STOCKHOLM:
                //    return "Europe/Stockholm";
                //case TimeZones.EUROPE_STOCKHOLM:
                //    return "Europe/Stockholm";
                //case TimeZones.EUROPE_STOCKHOLM:
                //    return "";
                default:
                    return "";
            }


        }

        public enum RestartMode
        {
            RESTART,
            SHUTDOWN,
            XSTART,
            ISTART,
            PSTART,
            BSTART
        }

        public enum LoginType
        {
            LOCAL,
            REMOTE

        }

        public enum Privilege
        {
            MODIFY,
            EXEC,
            DENY

        }

        public enum MastershipDomain
        {
            CFG,
            MOTION,
            RAPID

        }


        public static string GetRestartModeString(RestartMode restartMode)
        {
            switch (restartMode)
            {
                case RestartMode.RESTART:
                    return "restart";
                case RestartMode.SHUTDOWN:
                    return "shutdown";
                case RestartMode.XSTART:
                    return "xstart";
                case RestartMode.ISTART:
                    return "istart";
                case RestartMode.PSTART:
                    return "pstart";
                case RestartMode.BSTART:
                    return "bstart";
                default:
                    return "restart";
            }
        }



    }
}
