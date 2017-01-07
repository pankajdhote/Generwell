using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generwell.Core.Model;
using Generwell.Modules.Services;
using Generwell.Modules.Management.GenerwellManagement;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;

namespace Generwell.Modules.Management.FacilityManagement
{
    public class FacilityManagement : IFacilityManagement
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly List<FacilityModel> _objFacilityList;
        private readonly FacilityModel _objFacility;
        private readonly List<MapModel> _objMapList;
        private readonly List<WellLineReportModel> _objWellLineList;
        private readonly List<FilterModel> _objFilterList;
        private readonly LineReportsModel _objLineReport;
        public FacilityManagement(IOptions<AppSettingsModel> appSettings,
            IGenerwellServices generwellServices,
            IGenerwellManagement generwellManagement,
            List<FacilityModel> objWellList,
            FacilityModel objWell,
            List<MapModel> objMapList,
            List<WellLineReportModel> objWellLineList,
            List<FilterModel> objFilterList,
            LineReportsModel objLineReport)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _generwellManagement = generwellManagement;
            _objFacilityList = objWellList;
            _objFacility = objWell;
            _objMapList = objMapList;
            _objWellLineList = objWellLineList;
            _objFilterList = objFilterList;
            _objLineReport = objLineReport;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-07-01-2017
        /// Fetch all facilities from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FacilityModel>> GetFacilities(string id, string accessToken, string tokenType)
        {
            try
            {
                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    string getFacilityList = await _generwellServices.GetWebApiDetails(_appSettings.FacilityFilter + "=" + id, accessToken, tokenType);
                    List<FacilityModel> facilityModel = JsonConvert.DeserializeObject<List<FacilityModel>>(getFacilityList);
                    return facilityModel;
                }
                else
                {
                    string getFacilityList = await _generwellServices.GetWebApiDetails(_appSettings.Facilities, accessToken, tokenType);
                    List<FacilityModel> facilityModel = JsonConvert.DeserializeObject<List<FacilityModel>>(getFacilityList);
                    return facilityModel;
                }
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in FacilityManagement GetFacilities method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objFacilityList;
            }
        }

        //public Task<List<MapModel>> GetFacilitiesByFilterId(string defaultFilter, string accessToken, string tokenType)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<MapModel>> GetFacilitiesWithoutFilterId(string accessToken, string tokenType)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<FacilityModel> GetFacilityById(string id, string accessToken, string tokenType)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<LineReportsModel> GetFacilityDetailsByReportId(string reportId, string wellId, string accessToken, string tokenType)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<WellLineReportModel>> GetFacilityLineReports(string accessToken, string tokenType)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<FilterModel>> GetFilters(string accessToken, string tokenType)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<string> SetFollowUnfollow(string isFollow, string id, string accessToken, string tokenType)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
