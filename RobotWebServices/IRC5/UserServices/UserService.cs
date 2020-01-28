using RWS.IRC5.ResponseTypes;
using RWS.IRC5.UserServices.StateData;
using System;
using System.Globalization;
using System.Threading.Tasks;
using static RWS.Enums;

namespace RWS.IRC5.UserServices
{
    public class UserService
    {

        public IRC5Session Controller { get; set; }

        public UserService(IRC5Session cs)
        {
            Controller = cs;

        }


        public async Task RequestRmmpAsync(Privilege privilege)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("privilege", privilege.ToString().ToLowerInvariant()) };
            Tuple<string, string>[] urlParameters = Array.Empty<Tuple<string, string>>();

            await Controller.CallAsync<dynamic>(RequestMethod.POST, "users/rmmp", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task GrantOrDenyRmmpAsync(long uid, Privilege privilege)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("uid", uid.ToString(CultureInfo.InvariantCulture)), Tuple.Create("privilege", privilege.ToString().ToLowerInvariant()) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set") };

            await Controller.CallAsync<dynamic>(RequestMethod.POST, "users/rmmp", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task CancelHeldOrRequestedRmmpAsync()
        {

            Tuple<string, string>[] dataParameters = Array.Empty<Tuple<string, string>>();
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "cancel") };

            await Controller.CallAsync<dynamic>(RequestMethod.POST, "users/rmmp", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task<BaseResponse<GetRmmpState>> GetRmmpStateAsync()
        {

            Tuple<string, string>[] dataParameters = Array.Empty<Tuple<string, string>>();
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await Controller.CallAsync<GetRmmpState>(RequestMethod.GET, "users/rmmp", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task LoginAsAsync(LoginType loginType)
        {

            Tuple<string, string>[] dataParameters = { Tuple.Create("type", loginType.ToString().ToLowerInvariant()) };
            Tuple<string, string>[] urlParameters = { Tuple.Create("action", "set-locale"), Tuple.Create("json", "1") };

            await Controller.CallAsync<dynamic>(RequestMethod.POST, "users", dataParameters, urlParameters).ConfigureAwait(true);

        }

        public async Task RegisterUserAsync(string username, string application, string location, LoginType ulocale)
        {

            Tuple<string, string>[] dataParameters = {
                                                       Tuple.Create("username", username),
                                                       Tuple.Create("application", application),
                                                       Tuple.Create("location", location),
                                                       Tuple.Create("ulocale", ulocale.ToString().ToLowerInvariant())
                                                     };

            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            await Controller.CallAsync<dynamic>(RequestMethod.POST, "users", dataParameters, urlParameters).ConfigureAwait(true);

        }

    }
}
