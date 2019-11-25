using RWS.Data;
using RWS.UserServices.StateData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static RWS.ControllerSession;
using static RWS.Enums;

namespace RWS.UserServices
{
    public class UserService
    {

        public ControllerSession Controller { get; set; }

        public UserService(ControllerSession cs)
        {
            Controller = cs;

        }


        public void RequestRmmp(Privilege privilege)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("privilege", privilege.ToString().ToLowerInvariant()) };
            Tuple<string, string>[] urlParameters = Array.Empty<Tuple<string, string>>();

            Controller.Call<dynamic>(method, "users/rmmp", dataParameters, urlParameters);

        }

        public void GrantOrDenyRmmp(long uid, Privilege privilege)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("uid", uid.ToString(CultureInfo.InvariantCulture)), Tuple.Create("privilege", privilege.ToString().ToLowerInvariant()) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set") };

            Controller.Call<dynamic>(method, "users/rmmp", dataParameters, urlParameters);

        }

        public void CancelHeldOrRequestedRmmp()
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = Array.Empty<Tuple<string, string>>();
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "cancel") };

            Controller.Call<dynamic>(method, "users/rmmp", dataParameters, urlParameters);

        }

        public BaseResponse<GetRmmpState> GetRmmpState()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = Array.Empty<Tuple<string, string>>();
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return Controller.Call<GetRmmpState>(method, "users/rmmp", dataParameters, urlParameters);

        }

        public void LoginAs(LoginType loginType)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("type", loginType.ToString().ToLowerInvariant()) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-locale"), Tuple.Create("json", "1") };

            Controller.Call<dynamic>(method, "users", dataParameters, urlParameters);

        }

        public void RegisterUser(string username, string application, string location, LoginType ulocale)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("username", username), Tuple.Create("application", application), Tuple.Create("location", location), Tuple.Create("ulocale", ulocale.ToString().ToLower()) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            Controller.Call<dynamic>(method, "users", dataParameters, urlParameters);

        }

    }
}
