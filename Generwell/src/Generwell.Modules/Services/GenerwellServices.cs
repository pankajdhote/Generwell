using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Generwell.Core.Model;

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
        public async Task<string> PostWebApiData(string url, string accessToken, string tokenType, string content)
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

                    StringContent jsonContent = new StringContent(content.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage tokenServiceResponse = await client.PostAsync(url, jsonContent);
                    string responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    HttpStatusCode responseCode = tokenServiceResponse.StatusCode;
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
        /// Date:- 12-11-2016
        /// Get image data from web api in byte array format.
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> GetWebApiDetailsBytes(string url, string accessToken, string tokenType)
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
                    byte[] responseString = await tokenServiceResponse.Content.ReadAsByteArrayAsync();
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
        public async Task<string> UpdateWebApiData(string url, string accessToken, string tokenType, string Content)
        {
            try
            {
                string tokenServiceUrl = url;
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.Clear();
                hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                hc.DefaultRequestHeaders.Add("Time-Zone", "MDT");
                HttpMethod method = new HttpMethod("PATCH");
                string replacedString = Content.Replace("\\", "").Replace("\",\"", ",").Replace("\"]", "]").Replace("\"{", "{");
                string body = replacedString;
                HttpRequestMessage request = new HttpRequestMessage(method, url)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json-patch+json")
                };
                HttpResponseMessage hrm = await hc.SendAsync(request);
                string jsonresult = string.Empty;
                if (hrm.IsSuccessStatusCode)
                {
                    jsonresult = await hrm.Content.ReadAsStringAsync();
                    //if (hrm.ReasonPhrase == "OK")
                    //{
                    //}
                    //else
                    //{
                    //    return "";
                    //}
                }
                return jsonresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by Pankaj
        /// Date:- 21-12-2016
        /// POST Picture data to web api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> PostWebApiPictureData(string url, string accessToken, string tokenType, byte[] content, PictureModel pictureModel)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                ByteArrayContent byteContent = new ByteArrayContent(content); 
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                MultipartFormDataContent contentPost = new MultipartFormDataContent();

                contentPost.Add(new ByteArrayContent(content), "param", "filename");
                contentPost.Add(new StringContent(pictureModel.label), "label");
                contentPost.Add(new StringContent(pictureModel.comment), "comment");
                contentPost.Add(new StringContent(pictureModel.albumId), "albumId");

                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, contentPost);
                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                HttpStatusCode responseCode = httpResponseMessage.StatusCode;

                return responseCode.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by Pankaj
        /// Date:- 22-12-2016
        /// Put Picture data to web api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> PutWebApiPictureData(string url, string accessToken, string tokenType, byte[] content, PictureModel pictureModel)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                ByteArrayContent byteContent = new ByteArrayContent(content);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                MultipartFormDataContent contentPost = new MultipartFormDataContent();

                contentPost.Add(new ByteArrayContent(content), "param", "filename");
                contentPost.Add(new StringContent(pictureModel.label), "label");
                contentPost.Add(new StringContent(pictureModel.comment), "comment");
                contentPost.Add(new StringContent(pictureModel.albumId), "albumId");

                HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(url, contentPost);
                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                HttpStatusCode responseCode = httpResponseMessage.StatusCode;

                return responseCode.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by Pankaj
        /// Date:- 23-12-2016
        /// Put web api request.
        /// </summary>
        /// <returns></returns>
        public async Task<string> PutWebApiData(string url, string accessToken, string tokenType)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                StringContent jsonContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(url, jsonContent);
                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                HttpStatusCode responseCode = httpResponseMessage.StatusCode;

                return responseCode.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
