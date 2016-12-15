using AutoMapper;
using Generwell.Core.Model;
using Generwell.Modules.Services;
using Generwell.Modules.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
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
        public async Task<AccessTokenViewModel> AuthenticateUser(string userName, string password, string webApiUrl)
        {
            try
            {
                string url = webApiUrl + "/" + _appSettings.Token;
                string webApiDetails = await _generwellServices.ProcessRequest(userName, password, url);
                AccessTokenModel accessTokenModel = JsonConvert.DeserializeObject<AccessTokenModel>(webApiDetails);
                AccessTokenViewModel accessTokenViewModel = _mapper.Map<AccessTokenViewModel>(accessTokenModel);
                return accessTokenViewModel;
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
        public async Task<ContactFieldsViewModel> GetContactDetails(string accessToken, string tokenType)
        {
            string personnelRecord = await _generwellServices.GetWebApiDetails(_appSettings.ContactDetails, accessToken, tokenType);
            ContactFieldsModel contactFieldRecord = JsonConvert.DeserializeObject<ContactFieldsModel>(personnelRecord);
            ContactFieldsViewModel contactFieldsViewModel = _mapper.Map<ContactFieldsViewModel>(contactFieldRecord);
            return contactFieldsViewModel;
        }
        /// <summary>
        /// Added by Rohit
        /// Date:-14-12-2016
        /// Fetch all contact details from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<ContactFieldsViewModel> GetContactInformation(string accessToken, string tokenType)
        {
            string personnelRecord = await _generwellServices.GetWebApiDetails(_appSettings.ContactDetails, accessToken, tokenType);
            ContactFieldsModel contactFieldRecord = JsonConvert.DeserializeObject<ContactFieldsModel>(personnelRecord);
            ContactFieldsViewModel contactFieldsViewModel = _mapper.Map<ContactFieldsViewModel>(contactFieldRecord);
            return contactFieldsViewModel;
        }

        /// <summary>
        /// Added by pankaj
        /// Date:-10-12-2016
        /// Fetch support details from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<SupportViewModel> GetSupportDetails()
        {
            string supportData = await _generwellServices.GetWebApiDetails(_appSettings.Support, null, null);
            SupportModel contactFieldRecord = JsonConvert.DeserializeObject<SupportModel>(supportData);
            SupportViewModel contactFieldsViewModel = _mapper.Map<SupportViewModel>(contactFieldRecord);
            return contactFieldsViewModel;
        }
    }
}
