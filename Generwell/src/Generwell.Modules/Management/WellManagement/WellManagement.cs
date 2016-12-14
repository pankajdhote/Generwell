using Newtonsoft.Json;
using Generwell.Core.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Generwell.Modules.ViewModels;
using Generwell.Modules.GenerwellConstants;
using System.Text;
using System;

namespace Generwell.Modules.Management
{
    public class WellManagement : IWellManagement
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        private readonly IMapper _mapper;
        public WellManagement(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices,IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _mapper = mapper;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<WellViewModel>> GetWells(string id, string accessToken, string tokenType)
        {
            try
            {
                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    string getWellList = await _generwellServices.GetWebApiDetails(_appSettings.WellFilter + "=" + id, accessToken, tokenType);
                    List<WellModel> wellModel = JsonConvert.DeserializeObject<List<WellModel>>(getWellList);
                    List<WellViewModel> wellViewModel = _mapper.Map<List<WellViewModel>>(wellModel);
                    return wellViewModel;
                }
                else
                {
                    string getWellList = await _generwellServices.GetWebApiDetails(_appSettings.Well, accessToken, tokenType);
                    List<WellModel> wellModel = JsonConvert.DeserializeObject<List<WellModel>>(getWellList);
                    List<WellViewModel> wellViewModel = _mapper.Map<List<WellViewModel>>(wellModel);
                    return wellViewModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch well by id from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<WellViewModel> GetWellById(string id, string accessToken, string tokenType)
        {
            string getWellList = await _generwellServices.GetWebApiDetails(_appSettings.Well + "/" + id, accessToken, tokenType);
            WellModel wellModel = JsonConvert.DeserializeObject<WellModel>(getWellList);
            WellViewModel wellViewModel = _mapper.Map<WellViewModel>(wellModel);
            return wellViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api by filterId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapViewModel>> GetWellsByFilterId(string defaultFilter, string accessToken, string tokenType)
        {
            string wellRecordByFilter = await _generwellServices.GetWebApiDetails(_appSettings.WellFilter + "=" + defaultFilter, accessToken, tokenType);
            List<MapModel> mapModel = JsonConvert.DeserializeObject<List<MapModel>>(wellRecordByFilter);
            List<MapViewModel> mapViewModel = _mapper.Map<List<MapViewModel>>(mapModel);
            return mapViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api by filterId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapViewModel>> GetWellsWithoutFilterId(string accessToken, string tokenType)
        {
            string wellRecordByFilter = await _generwellServices.GetWebApiDetails(_appSettings.Well, accessToken, tokenType);
            List<MapModel> mapModel = JsonConvert.DeserializeObject<List<MapModel>>(wellRecordByFilter);
            List<MapViewModel> mapViewModel = _mapper.Map<List<MapViewModel>>(mapModel);
            return mapViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well line reports from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<WellLineReportViewModel>> GetWellLineReports(string accessToken, string tokenType)
        {
            string wellLineReportList = await _generwellServices.GetWebApiDetails(_appSettings.WellLineReports, accessToken, tokenType);
            List<WellLineReportModel> wellLineReport = JsonConvert.DeserializeObject<List<WellLineReportModel>>(wellLineReportList);
            List<WellLineReportViewModel> wellLineReportViewModel = _mapper.Map<List<WellLineReportViewModel>>(wellLineReport);
            return wellLineReportViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all filters from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FilterViewModel>> GetFilters(string accessToken, string tokenType)
        {
            try
            {
                string filterList = await _generwellServices.GetWebApiDetails(_appSettings.Filters, accessToken, tokenType);
                List<FilterModel> filterModel = JsonConvert.DeserializeObject<List<FilterModel>>(filterList);
                List<FilterViewModel> filterViewModel = _mapper.Map<List<FilterViewModel>>(filterModel);
                return filterViewModel;
            }
            catch (Exception)
            {

                throw;
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
            if (isFollow == Constants.trueState)
            {
                string response = await _generwellServices.PostWebApiData(_appSettings.Well + "/" + id + "/follow", accessToken, tokenType);
                return response;
            }
            else
            {
                string response = await _generwellServices.DeleteWebApiData(_appSettings.Well + "/" + id + "/unfollow", accessToken, tokenType);
                return response;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        ///Get well details from reportId.
        /// </summary>
        /// <returns></returns>
        public async Task<LineReportsViewModel> GetWellDetailsByReportId(string reportId, string wellId, string accessToken, string tokenType)
        {
            string wellDetailsList = await _generwellServices.GetWebApiWithTimeZone(_appSettings.Well + "/" + wellId + "/linereports/" + Encoding.UTF8.GetString(Convert.FromBase64String(reportId)), accessToken, tokenType);
            LineReportsViewModel wellDetailsModel = JsonConvert.DeserializeObject<LineReportsViewModel>(wellDetailsList);
            LineReportsViewModel wellDetailsViewModel = _mapper.Map<LineReportsViewModel>(wellDetailsModel);
            return wellDetailsViewModel;
        }
    }
}
