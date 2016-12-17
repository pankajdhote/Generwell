using Generwell.Core.Model;
using Generwell.Modules.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generwell.Modules.Management
{
    public interface IWellManagement
    {
        Task<List<WellModel>> GetWells(string id, string accessToken, string tokenType);
        Task<WellModel> GetWellById(string id, string accessToken, string tokenType);
        Task<List<MapModel>> GetWellsByFilterId(string defaultFilter, string accessToken, string tokenType);
        Task<List<MapModel>> GetWellsWithoutFilterId(string accessToken, string tokenType);
        Task<List<WellLineReportModel>> GetWellLineReports(string accessToken, string tokenType);
        Task<List<FilterModel>> GetFilters(string accessToken, string tokenType);
        Task<string> SetFollowUnfollow(string isFollow, string id, string accessToken, string tokenType);
        Task<LineReportsModel> GetWellDetailsByReportId(string reportId, string wellId, string accessToken, string tokenType);
    }
}
