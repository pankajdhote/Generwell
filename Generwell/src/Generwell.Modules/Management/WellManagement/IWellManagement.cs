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
        Task<List<WellLineReportModel>> GetWellLineReports(string accessToken, string tokenType);
        
    }
}
