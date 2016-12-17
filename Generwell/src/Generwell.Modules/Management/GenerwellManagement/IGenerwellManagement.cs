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
        Task<AccessTokenModel> AuthenticateUser(string userName, string password, string webApiUrl);
        Task<ContactFieldsModel> GetContactDetails(string accessToken, string tokenType);
        Task<SupportModel> GetSupportDetails();
        Task<string> LogError(string param, string accessToken, string tokenType, string content);

    }
}
