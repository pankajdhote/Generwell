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
        private readonly List<FacilityLineReportModel> _objFacilityLineList;
        private readonly List<FilterModel> _objFilterList;
        private readonly LineReportsModel _objLineReport;
        public FacilityManagement(IOptions<AppSettingsModel> appSettings,
            IGenerwellServices generwellServices,
            IGenerwellManagement generwellManagement,
            List<FacilityModel> objWellList,
            FacilityModel objWell,
            List<MapModel> objMapList,
            List<FacilityLineReportModel> objFacilityLineList,
            List<FilterModel> objFilterList,
            LineReportsModel objLineReport)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _generwellManagement = generwellManagement;
            _objFacilityList = objWellList;
            _objFacility = objWell;
            _objMapList = objMapList;
            _objFacilityLineList = objFacilityLineList;
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
                if (!string.IsNullOrEmpty(id) && id != Constants.NullValue)
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
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well line reports from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FacilityLineReportModel>> GetFacilityLineReports(string accessToken, string tokenType)
        {
            try
            {
                string facililtyLineReportResponse = await _generwellServices.GetWebApiDetails(_appSettings.FacilityLineReports, accessToken, tokenType);
                List<FacilityLineReportModel> facilityLineReport = JsonConvert.DeserializeObject<List<FacilityLineReportModel>>(facililtyLineReportResponse);
                return facilityLineReport;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetWellLineReports method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objFacilityLineList;
            }

        }
    }
}
