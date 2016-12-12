using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Generwell.Modules.Services
{
    public class GenerwellServices : IGenerwellServices
    {
        public GenerwellServices()
        {
        }
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
                using (HttpClient client = new HttpClient())
                {
                    List<KeyValuePair<string, string>> requestParams = new List<KeyValuePair<string, string>>{
                                     new KeyValuePair<string, string>("grant_type","password"),
                                     new KeyValuePair<string, string>("username", userName),
                                     new KeyValuePair<string, string>("password", password)
                 };
                    FormUrlEncodedContent requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    HttpResponseMessage tokenServiceResponse = await client.PostAsync(serverUrl, requestParamsFormUrlEncoded);
                    string responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
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
        public async Task<string> PostWebApiData(string url, string accessToken, string tokenType)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<KeyValuePair<string, string>> requestParams = new List<KeyValuePair<string, string>>
                    {
                    };
                    if (accessToken != null)
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                    }
                    FormUrlEncodedContent requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    HttpResponseMessage tokenServiceResponse = await client.PostAsync(url, requestParamsFormUrlEncoded);
                    string responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    HttpStatusCode responseCode = tokenServiceResponse.StatusCode;
                    HttpResponseMessage responseMsg = new HttpResponseMessage(responseCode)
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
        public async Task<string> DeleteWebApiData(string url, string accessToken, string tokenType)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (accessToken != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                    }
                    HttpResponseMessage tokenServiceResponse = await client.DeleteAsync(url);
                    string responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    HttpStatusCode responseCode = tokenServiceResponse.StatusCode;
                    HttpResponseMessage responseMsg = new HttpResponseMessage(responseCode)
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
        public async Task<string> GetWebApiDetails(string url, string accessToken, string tokenType)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (accessToken != null)
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                    }
                    HttpResponseMessage tokenServiceResponse = await client.GetAsync(url);
                    string responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
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
        public async Task<string> GetWebApiWithTimeZone(string url, string accessToken, string tokenType)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (accessToken != null)
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Time-Zone", "UTC");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                    }
                    HttpResponseMessage tokenServiceResponse = await client.GetAsync(url);
                    string responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
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
        public async Task<string> UpdateTaskData(string url, string accessToken, string tokenType, string Content)
        {
            try
            {
                var tokenServiceUrl = url;
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.Clear();
                hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                hc.DefaultRequestHeaders.Add("Time-Zone", "MDT");
                var method = new HttpMethod("PATCH");
                //string con = "";
                //for (int index = 0; index < Content.Length; index++)
                //{
                //    if (index == 0)
                //    {
                //        con = Content[index];
                //    }
                //    else
                //    {
                //        con += "," + Content[index];
                //    }
                //}
                //string body = "[" + con + "]";
                string replacedString = Content.Replace("\",\"", ",").Replace("\"]", "]").Replace("\"{","{");
                string body = replacedString;
                var request = new HttpRequestMessage(method, url)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json-patch+json")
                };
                HttpResponseMessage hrm = await hc.SendAsync(request);
                if (hrm.IsSuccessStatusCode)
                {
                    string jsonresult = await hrm.Content.ReadAsStringAsync();
                    if (hrm.ReasonPhrase == "OK")
                    {

                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
