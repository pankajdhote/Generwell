using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Extensions;
using Generwell.Modules.GenerwellConstants;




namespace Generwell.Modules
{
    public class WebClient
    {
        public object ViewBag { get; private set; }

        /// <summary>
        /// Added by Pankaj
        /// Date:- 12-11-2016
        /// get access token from web api
        /// </summary>
        /// <returns></returns>
        public async Task<string> ProcessRequest(string userName, string password, string serverUrl)
        {
            try
            {
                var tokenServiceUrl = serverUrl + "/api/Token";
                using (var client = new HttpClient())
                {
                    var requestParams = new List<KeyValuePair<string, string>>{

                 new KeyValuePair<string, string>("grant_type","password"),
                 new KeyValuePair<string, string>("username", userName),
                 new KeyValuePair<string, string>("password", password)

                 };
                    var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    return responseString;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Added by Pankaj
        /// Date:- 12-11-2016
        /// post data to web api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> PostWebApiData(string url, string accessToken)
        {
            try
            {
                var tokenServiceUrl = url;
                using (var client = new HttpClient())
                {
                    var requestParams = new List<KeyValuePair<string, string>>
                    {
                    };
                    if (accessToken != null)
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    var responseCode = tokenServiceResponse.StatusCode;
                    var responseMsg = new HttpResponseMessage(responseCode)
                    {
                        Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                    };
                    return responseString;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        /// <summary>
        /// Added by Pankaj
        /// Date:- 13-11-2016
        /// call delete web api
        /// </summary>
        /// <returns></returns>
        public async Task<string> DeleteWebApiData(string url, string accessToken)
        {
            try
            {
                var tokenServiceUrl = url;
                using (var client = new HttpClient())
                {
                    if (accessToken != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    var tokenServiceResponse = await client.DeleteAsync(tokenServiceUrl);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    var responseCode = tokenServiceResponse.StatusCode;
                    var responseMsg = new HttpResponseMessage(responseCode)
                    {
                        Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                    };
                    return responseString;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Added by Pankaj
        /// Date:- 12-11-2016
        /// Get data from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWebApiDetails(string url, string accessToken)
        {
            try
            {
                var tokenServiceUrl = url;
                using (var client = new HttpClient())
                {
                    if (accessToken != null)
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    var tokenServiceResponse = await client.GetAsync(tokenServiceUrl);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    return responseString;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Added by Pankaj
        /// Date:- 15-11-2016
        /// Get web api data with timezone
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWebApiWithTimeZone(string url, string accessToken)
        {
            try
            {
                var tokenServiceUrl = url;
                using (var client = new HttpClient())
                {
                    if (accessToken != null)
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Time-Zone", "UTC");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    var tokenServiceResponse = await client.GetAsync(tokenServiceUrl);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    return responseString;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Added by Rohit
        /// Date:- 25-11-2016
        /// Post data using update url
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateTaskData(string url, string accessToken, string value, int fieldId)
        {
            try
            {
                var tokenServiceUrl = url;               
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.Clear();
                hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                hc.DefaultRequestHeaders.Add("Time-Zone", "MDT");

                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, url)
                {
                  
                //Content = new StringContent("[{ \"op\": \"replace\", \"path\": \"/Fields/6\", \"value\": \"4444\"}]", Encoding.UTF8, "application/json-patch+json")
                Content = new StringContent("[{ \"op\": \"replace\", \"path\": \"/Fields/"+ fieldId + "\", \"value\": " + value +"}]", Encoding.UTF8, "application/json-patch+json")
                };

                HttpResponseMessage hrm = await hc.SendAsync(request);
                if (hrm.IsSuccessStatusCode)
                {
                    string jsonresult = await hrm.Content.ReadAsStringAsync();
                }
                else
                {
                }
                return string.Empty;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
