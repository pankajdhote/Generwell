using Generwell.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.Services
{
    public interface IGenerwellServices
    {
        Task<string> ProcessRequest(string userName, string password, string serverUrl);
        Task<string> PostWebApiData(string url, string accessToken, string tokenType, string content);
        Task<string> PostWebApiPictureData(string url, string accessToken, string tokenType, byte[] content, PictureModel pictureModel);
        Task<string> PutWebApiPictureData(string url, string accessToken, string tokenType, byte[] content, PictureModel pictureModel);
        Task<string> PutWebApiData(string url, string accessToken, string tokenType);
        Task<string> DeleteWebApiData(string url, string accessToken, string tokenType);
        Task<string> GetWebApiDetails(string url, string accessToken, string tokenType);
        Task<byte[]> GetWebApiDetailsBytes(string url, string accessToken, string tokenType);
        Task<string> GetWebApiWithTimeZone(string url, string accessToken, string tokenType);
        Task<string> UpdateWebApiData(string url, string accessToken, string tokenType, string Content);
    }
}
