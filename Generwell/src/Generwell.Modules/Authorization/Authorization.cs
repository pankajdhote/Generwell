using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Generwell.Modules.Authorization
{
    public class Authorization
    {
        /// <summary>
        /// Web Service Call for getting the Default Values of Application
        /// </summary>
        /// <returns></returns>
        public async Task<string> AuthenticateUser(string userName,string password,string webApiUrl)
        {
            try
            {
                WebClient webClient = new WebClient();                
                var  webApiDetails= await webClient.ProcessRequest(userName,password, webApiUrl);
                return webApiDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
