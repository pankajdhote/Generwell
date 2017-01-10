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
        Task<List<FacilityLineReportModel>> GetFacilityLineReports(string accessToken, string tokenType);
    }
}
