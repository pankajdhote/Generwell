using Generwell.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.Management.FacilityManagement
{
    public interface IFacilityManagement
    {
        Task<List<FacilityModel>> GetFacilities(string id, string accessToken, string tokenType);
        //Task<FacilityModel> GetFacilityById(string id, string accessToken, string tokenType);
        //Task<List<MapModel>> GetFacilitiesByFilterId(string defaultFilter, string accessToken, string tokenType);
        //Task<List<MapModel>> GetFacilitiesWithoutFilterId(string accessToken, string tokenType);
        //Task<List<WellLineReportModel>> GetFacilityLineReports(string accessToken, string tokenType);
        //Task<List<FilterModel>> GetFilters(string accessToken, string tokenType);
        //Task<string> SetFollowUnfollow(string isFollow, string id, string accessToken, string tokenType);
        //Task<LineReportsModel> GetFacilityDetailsByReportId(string reportId, string wellId, string accessToken, string tokenType);
    }
}
