using Generwell.Core.Model;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Generwell.Modules.Management.PictureManagement
{
    public class PictureManagement : IPictureManagement
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly PictureModel _pictureModel;

        public PictureManagement(IOptions<AppSettingsModel> appSettings,
            IGenerwellServices generwellServices,
            PictureModel pictureModel,
            IGenerwellManagement generwellManagement)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _generwellManagement = generwellManagement;
            _pictureModel = pictureModel;
        }


        /// <summary>
        /// Added by pankaj
        /// Date:-14-12-2016
        /// Fetch all pictures by albumId from web api and display on screen.
        /// </summary>
        /// <returns></returns>
        public async Task<AlbumModel> GetPictureAlbum(string id, string accessToken, string tokenType)
        {
            try
            {
                string albumData = await _generwellServices.GetWebApiDetails(_appSettings.PictureAlbum + "/" + id, accessToken, tokenType);
                AlbumModel albumRecord = JsonConvert.DeserializeObject<AlbumModel>(albumData);
                foreach (PictureModel item in albumRecord.pictures)
                {
                    item.picture = await _generwellServices.GetWebApiDetailsBytes(item.fileUrl, accessToken, tokenType);
                }
                return albumRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-14-12-2016
        /// Fetch all pictures by albumId from web api and display on screen.
        /// </summary>
        /// <returns></returns>
        public async Task<PictureModel> GetPicture(string fileUrl, string accessToken, string tokenType)
        {
            try
            {
                _pictureModel.picture = await _generwellServices.GetWebApiDetailsBytes(fileUrl, accessToken, tokenType);
                return _pictureModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Added by pankaj
        /// Date:-20-12-2016
        /// Update picture Details fields using patch api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdatePicture(string Content, string pictureId, string accessToken, string tokenType)
        {
            try
            {
                string taskDetailsReecord = await _generwellServices.UpdateWebApiData(_appSettings.TaskDetails + "/" + pictureId, accessToken, tokenType, Content);
                return taskDetailsReecord;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in PictureManagement UpdatePicture method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return response;
            }
        }

        /// <summary>
        /// Added by pankaj
        /// Date:-20-12-2016
        /// Delete picture using api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> DeletePicture(string pictureId, string accessToken, string tokenType)
        {
            try
            {
                string taskDetailsReecord = await _generwellServices.DeleteWebApiData(_appSettings.Picture + "/" + pictureId, accessToken, tokenType);
                return taskDetailsReecord;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in PictureManagement UpdatePicture method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return response;
            }
        }
    }
}
