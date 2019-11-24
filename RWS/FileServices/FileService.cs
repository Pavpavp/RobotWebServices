using RWS.Data;
using RWS.FileServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.RobotWareServices
{
    public struct FileService
    {
        public ControllerSession Controller { get; set; }

        public FileService(ControllerSession cs)
        {
            Controller = cs;

        }


        public async Task<BaseResponse<GetDirectoryListingState>> GetDirectoryListingAsync(string path)
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = null;
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await Controller.CallAsync<GetDirectoryListingState>(method, "fileservice/" + path, dataParameters, urlParameters).ConfigureAwait(false);

        }
        public async Task<dynamic> UploadFileAsync(string fromPath, string toPath, bool overwrite)
        {

            string method = "PUT";

            Tuple<string, string>[] dataParameters = { Tuple.Create(fromPath, "") };
            Tuple<string, string>[] urlParameters = { Tuple.Create("json", "1") };

            return await Controller.CallAsync<dynamic>(method, "/fileservice/" + toPath, dataParameters, urlParameters).ConfigureAwait(false);

        }

    }
}
