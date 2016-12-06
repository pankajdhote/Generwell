using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.Services
{
    public interface IGenerwellServices
    {
        Task<string> ProcessRequest(string userName, string password, string serverUrl);
        Task<string> PostWebApiData(string url, string accessToken);
        Task<string> DeleteWebApiData(string url, string accessToken);
        Task<string> GetWebApiDetails(string url, string accessToken);
        Task<string> GetWebApiWithTimeZone(string url, string accessToken);
        Task<string> UpdateTaskData(string url, string accessToken, string value, int fieldId);
    }
}
