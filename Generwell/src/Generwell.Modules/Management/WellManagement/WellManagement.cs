using Newtonsoft.Json;
using Generwell.Core.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generwell.Modules.GenerwellConstants;
using System.Text;
using System;
using Generwell.Modules.Management.GenerwellManagement;
using Microsoft.AspNetCore.Http;


namespace Generwell.Modules.Management
{
    public class WellManagement : IWellManagement
    {

        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly List<WellModel> _objWellList;
        private readonly WellModel _objWell;
        private readonly List<MapModel> _objMapList;
        private readonly List<WellLineReportModel> _objWellLineList;
        private readonly List<FilterModel> _objFilterList;
        private readonly LineReportsModel _objLineReport;
        public WellManagement(IOptions<AppSettingsModel> appSettings, 
            IGenerwellServices generwellServices, 
            IGenerwellManagement generwellManagement, 
            List<WellModel> objWellList, 
            WellModel objWell, 
            List<MapModel> objMapList, 
            List<WellLineReportModel> objWellLineList, 
            List<FilterModel> objFilterList,
            LineReportsModel objLineReport)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _generwellManagement = generwellManagement;
            _objWellList = objWellList;
            _objWell = objWell;
            _objMapList = objMapList;
            _objWellLineList = objWellLineList;
            _objFilterList = objFilterList;
            _objLineReport = objLineReport;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<WellModel>> GetWells(string id, string accessToken, string tokenType)
        {
            try
            {
                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    string getWellList = await _generwellServices.GetWebApiDetails(_appSettings.WellFilter + "=" + id, accessToken, tokenType);
                    List<WellModel> wellModel = JsonConvert.DeserializeObject<List<WellModel>>(getWellList);
                    return wellModel;
                }
                else
                {
                    string getWellList = await _generwellServices.GetWebApiDetails(_appSettings.Well, accessToken, tokenType);
                    List<WellModel> wellModel = JsonConvert.DeserializeObject<List<WellModel>>(getWellList);
                    return wellModel;
                }
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetWells method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objWellList;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch well by id from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<WellModel> GetWellById(string id, string accessToken, string tokenType)
        {
            try
            {
                string getWellList = await _generwellServices.GetWebApiDetails(_appSettings.Well + "/" + id, accessToken, tokenType);
                WellModel wellModel = JsonConvert.DeserializeObject<WellModel>(getWellList);
                return wellModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetWellById method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objWell;
            }

        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api by filterId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapModel>> GetWellsByFilterId(string defaultFilter, string accessToken, string tokenType)
        {
            try
            {
                string wellRecordByFilter = await _generwellServices.GetWebApiDetails(_appSettings.WellFilter + "=" + defaultFilter, accessToken, tokenType);
                List<MapModel> mapModel = JsonConvert.DeserializeObject<List<MapModel>>(wellRecordByFilter);
                return mapModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetWellsByFilterId method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objMapList;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api by filterId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapModel>> GetWellsWithoutFilterId(string accessToken, string tokenType)
        {
            try
            {
                string wellRecordByFilter = await _generwellServices.GetWebApiDetails(_appSettings.Well, accessToken, tokenType);
                List<MapModel> mapModel = JsonConvert.DeserializeObject<List<MapModel>>(wellRecordByFilter);
                return mapModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetWellsWithoutFilterId method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objMapList;
            }

        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well line reports from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<WellLineReportModel>> GetWellLineReports(string accessToken, string tokenType)
        {
            try
            {
                string wellLineReportList = await _generwellServices.GetWebApiDetails(_appSettings.WellLineReports, accessToken, tokenType);
                List<WellLineReportModel> wellLineReport = JsonConvert.DeserializeObject<List<WellLineReportModel>>(wellLineReportList);
                return wellLineReport;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetWellLineReports method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objWellLineList;
            }

        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all filters from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FilterModel>> GetFilters(string accessToken, string tokenType)
        {
            try
            {
                string filterList = await _generwellServices.GetWebApiDetails(_appSettings.Filters, accessToken, tokenType);
                List<FilterModel> filterModel = JsonConvert.DeserializeObject<List<FilterModel>>(filterList);
                return filterModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetFilters method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objFilterList;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Follow and UnFollow Wells.
        /// </summary>
        /// <returns></returns>
        public async Task<string> SetFollowUnfollow(string isFollow, string id, string accessToken, string tokenType)
        {
            try
            {
                if (isFollow == Constants.trueState)
                {
                    string response = await _generwellServices.PostWebApiData(_appSettings.Well + "/" + id + "/follow", accessToken, tokenType, string.Empty);
                    return response;
                }
                else
                {
                    string response = await _generwellServices.DeleteWebApiData(_appSettings.Well + "/" + id + "/unfollow", accessToken, tokenType);
                    return response;
                }
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetFilters method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return string.Empty;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        ///Get well details from reportId.
        /// </summary>
        /// <returns></returns>
        public async Task<LineReportsModel> GetWellDetailsByReportId(string reportId, string wellId, string accessToken, string tokenType)
        {
            try
            {
                string wellDetailsList = await _generwellServices.GetWebApiWithTimeZone(_appSettings.Well + "/" + wellId + "/linereports/" + Encoding.UTF8.GetString(Convert.FromBase64String(reportId)), accessToken, tokenType);
                LineReportsModel wellDetailsModel = JsonConvert.DeserializeObject<LineReportsModel>(wellDetailsList);
                return wellDetailsModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetWellDetailsByReportId method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objLineReport;
            }
        }
    }
}
