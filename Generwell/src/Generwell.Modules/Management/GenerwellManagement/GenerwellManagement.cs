using AutoMapper;
using Generwell.Core.Model;
using Generwell.Modules.Services;
using Generwell.Modules.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generwell.Modules.Management.GenerwellManagement
{
    public class GenerwellManagement : IGenerwellManagement
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        private readonly IMapper _mapper;

        public GenerwellManagement(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _mapper = mapper;
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
        public async Task<string> LogError(string param, string accessToken, string tokenType, string content)
        {
            string response = await _generwellServices.PostWebApiData(_appSettings.LogError + "/" + param, accessToken, tokenType, content);
            return response;
        }
    }
}
