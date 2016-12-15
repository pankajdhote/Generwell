using Generwell.Core.Model;
using Generwell.Modules.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.Management.GenerwellManagement
{
    public interface IGenerwellManagement
    {
        Task<AccessTokenViewModel> AuthenticateUser(string userName, string password, string webApiUrl);
        Task<ContactFieldsViewModel> GetContactDetails(string accessToken, string tokenType);
        Task<List<ContactInformationViewModel>> GetContactInformation(string accessToken, string tokenType);
        Task<SupportViewModel> GetSupportDetails();
        //Task<ContactInformationViewModel> GetContactInformation(string accessToken, string tokenType);

    }
}
