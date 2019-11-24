using RWS.Data;
using RWS.UserServices.StateData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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


        public async Task RequestRmmpAsync(Privilege privilege)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("privilege", privilege.ToString().ToLowerInvariant()) };
            Tuple<string, string>[] urlParameters = Array.Empty<Tuple<string, string>>();

            await Controller.CallAsync<dynamic>(method, "users/rmmp", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task GrantOrDenyRmmpAsync(long uid, Privilege privilege)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("uid", uid.ToString(CultureInfo.InvariantCulture)), Tuple.Create("privilege", privilege.ToString().ToLowerInvariant()) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set") };

            await Controller.CallAsync<dynamic>(method, "users/rmmp", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task CancelHeldOrRequestedRmmpAsync()
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = Array.Empty<Tuple<string, string>>();
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "cancel") };

            await Controller.CallAsync<dynamic>(method, "users/rmmp", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task<BaseResponse<GetRmmpState>> GetRmmpStateAsync()
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = Array.Empty<Tuple<string, string>>();
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await Controller.CallAsync<GetRmmpState>(method, "users/rmmp", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task LoginAsAsync(LoginType loginType)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = { Tuple.Create("type", loginType.ToString().ToLowerInvariant()) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-locale"), Tuple.Create("json", "1") };

            await Controller.CallAsync<dynamic>(method, "users", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task RegisterUserAsync(string username, string application, string location, LoginType ulocale)
        {

            string method = "POST";

            Tuple<string, string>[] dataParameters = {
                                                       Tuple.Create("username", username),
                                                       Tuple.Create("application", application),
                                                       Tuple.Create("location", location),
                                                       Tuple.Create("ulocale", ulocale.ToString().ToLower())
                                                     };

            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            await Controller.CallAsync<dynamic>(method, "users", dataParameters, urlParameters).ConfigureAwait(true);

        }

    }
}
