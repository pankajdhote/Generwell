using Generwell.Core.Model;
using Generwell.Modules.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generwell.Modules.Management
{
    public interface IWellManagement
    {
        Task<List<WellViewModel>> GetWells(string id, string accessToken, string tokenType);
        Task<WellViewModel> GetWellById(string id, string accessToken, string tokenType);
        Task<List<MapViewModel>> GetWellsByFilterId(string defaultFilter, string accessToken, string tokenType);
        Task<List<MapViewModel>> GetWellsWithoutFilterId(string accessToken, string tokenType);
        Task<List<WellLineReportViewModel>> GetWellLineReports(string accessToken, string tokenType);
        Task<List<FilterViewModel>> GetFilters(string accessToken, string tokenType);
        Task<string> SetFollowUnfollow(string isFollow, string id, string accessToken, string tokenType);
        Task<LineReportsViewModel> GetWellDetailsByReportId(string reportId, string wellId, string accessToken, string tokenType);
    }
}
