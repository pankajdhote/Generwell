using System;
using AutoMapper;
using Generwell.Core.Model;
using System.Threading.Tasks;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.GenerwellConstants;
using System.Collections.Generic;
using System.Text;

namespace Generwell.Modules.Management.GenerwellManagement
{
    public class GenerwellManagement : IGenerwellManagement
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<FilterModel> _objFilterList;
        private readonly LineReportsModel _objLineReport;
        private readonly List<MapModel> _objMapList;

        private HttpContext _httpContext => _httpContextAccessor.HttpContext;

        public GenerwellManagement(List<FilterModel> objFilterList,
            IHttpContextAccessor httpContextAccessor, 
            IOptions<AppSettingsModel> appSettings, 
            IGenerwellServices generwellServices,
            LineReportsModel objLineReport,
            List<MapModel> objMapList,
            IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _httpContextAccessor = httpContextAccessor;
            _objFilterList = objFilterList;
            _objLineReport= objLineReport;
            _objMapList = objMapList;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Authenticate user for successfull login
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessTokenModel> AuthenticateUser(string userName, string password, string webApiUrl)
        {
            try
            {
                string url = webApiUrl + "/" + _appSettings.Token;
                string webApiDetails = await _generwellServices.ProcessRequest(userName, password, url);
                AccessTokenModel accessTokenModel = JsonConvert.DeserializeObject<AccessTokenModel>(webApiDetails);
                return accessTokenModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 23-11-2016
        /// Keep License alive after 50 seconds.
        /// </summary>
        /// <returns></returns>
        public async Task KeepLicenseAlive(string id, string accessToken, string tokenType)
        {
            try
            {
                string response = await _generwellServices.PutWebApiData(_appSettings.License + "/" + id, accessToken, tokenType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 23-11-2016
        /// Create Licenses
        /// </summary>
        /// <returns></returns>
        public async Task<LicenseModel> CreateLicense(string url, string accessToken, string tokenType)
        {
            try
            {
                string response = await _generwellServices.PostWebApiData(url + "/licenses", accessToken, tokenType, string.Empty);
                LicenseModel licenseModel = JsonConvert.DeserializeObject<LicenseModel>(response);
                return licenseModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 23-11-2016
        /// Release Licenses
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReleaseLicense(string id, string accessToken, string tokenType)
        {
            try
            {
                string response = await _generwellServices.DeleteWebApiData(_appSettings.License + "/" + id, accessToken, tokenType);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all contact details from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<ContactFieldsModel> GetContactDetails(string accessToken, string tokenType)
        {
            string personnelRecord = await _generwellServices.GetWebApiDetails(_appSettings.ContactDetails, accessToken, tokenType);
            ContactFieldsModel contactFieldRecord = JsonConvert.DeserializeObject<ContactFieldsModel>(personnelRecord);
            return contactFieldRecord;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-10-12-2016
        /// Fetch support details from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<SupportModel> GetSupportDetails()
        {
            string supportData = await _generwellServices.GetWebApiDetails(_appSettings.Support, null, null);
            SupportModel contactFieldRecord = JsonConvert.DeserializeObject<SupportModel>(supportData);
            return contactFieldRecord;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-16-12-2016
        /// Created for error logging using post api..
        /// </summary>
        /// <returns></returns>
        public async Task LogError(string param, string accessToken, string tokenType, string content)
        {
            await _generwellServices.PostWebApiData(_appSettings.LogError + "/" + param, accessToken, tokenType, content);
            _httpContext.Response.Redirect("/Accounts/Logout");
        }


        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Follow and UnFollow Wells.
        /// </summary>
        /// <returns></returns>
        public async Task<string> SetFollowUnfollow(string url, string isFollow, string id, string accessToken, string tokenType)
        {
            try
            {
                if (isFollow == Constants.trueState)
                {
                    string response = await _generwellServices.PostWebApiData(url + "/" + id + "/follow", accessToken, tokenType, string.Empty);
                    return response;
                }
                else
                {
                    string response = await _generwellServices.DeleteWebApiData(url + "/" + id + "/unfollow", accessToken, tokenType);
                    return response;
                }
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetFilters method.\"}";
                await LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return string.Empty;
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
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in GenerwellManagement GetFilters method.\"}";
                await LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objFilterList;
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
                await LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objLineReport;
            }
        }

        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api by filterId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapModel>> GetAssetsByFilterId(string url,string defaultFilter, string accessToken, string tokenType)
        {
            try
            {
                string recordByFilter = await _generwellServices.GetWebApiDetails(url + "=" + defaultFilter, accessToken, tokenType);
                List<MapModel> mapModel = JsonConvert.DeserializeObject<List<MapModel>>(recordByFilter);
                return mapModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetWellsByFilterId method.\"}";
                await LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objMapList;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api by filterId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapModel>> GetAssetsWithoutFilterId(string url,string accessToken, string tokenType)
        {
            try
            {
                string recordByFilter = await _generwellServices.GetWebApiDetails(url, accessToken, tokenType);
                List<MapModel> mapModel = JsonConvert.DeserializeObject<List<MapModel>>(recordByFilter);
                return mapModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellManagement GetWellsWithoutFilterId method.\"}";
                await LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objMapList;
            }

        }
    }
}
