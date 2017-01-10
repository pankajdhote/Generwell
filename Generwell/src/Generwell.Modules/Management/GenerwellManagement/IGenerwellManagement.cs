using Generwell.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generwell.Modules.Management.GenerwellManagement
{
    public interface IGenerwellManagement
    {
        Task<AccessTokenModel> AuthenticateUser(string userName, string password, string webApiUrl);
        Task<ContactFieldsModel> GetContactDetails(string accessToken, string tokenType);
        Task<SupportModel> GetSupportDetails();
        Task LogError(string param, string accessToken, string tokenType, string content);
        Task KeepLicenseAlive(string id, string accessToken, string tokenType);
        Task<LicenseModel> CreateLicense(string url, string accessToken, string tokenType);
        Task<string> ReleaseLicense(string id, string accessToken, string tokenType);
        Task<string> SetFollowUnfollow(string url, string isFollow, string id, string accessToken, string tokenType);
        Task<List<FilterModel>> GetFilters(string accessToken, string tokenType);
        Task<LineReportsModel> GetWellDetailsByReportId(string reportId, string wellId, string accessToken, string tokenType);
        Task<List<MapModel>> GetAssetsByFilterId(string url, string defaultFilter, string accessToken, string tokenType);
        Task<List<MapModel>> GetAssetsWithoutFilterId(string url, string accessToken, string tokenType);
    }
}
