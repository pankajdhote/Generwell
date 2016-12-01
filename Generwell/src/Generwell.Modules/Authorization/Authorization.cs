using System;
using System.Threading.Tasks;

namespace Generwell.Modules.Authorization
{
    public class Authorization
    {
        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Authenticate user for successfull login
        /// 
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
