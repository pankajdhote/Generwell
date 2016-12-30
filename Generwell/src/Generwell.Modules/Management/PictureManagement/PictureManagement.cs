using Generwell.Core.Model;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
                AlbumModel albumModel = new AlbumModel();
                List<PictureModel> pictureList = new List<PictureModel>();
                string albumData = await _generwellServices.GetWebApiDetails(_appSettings.PictureAlbum + "/" + id, accessToken, tokenType);
                if (!string.IsNullOrEmpty(albumData))
                {
                    AlbumModel albumRecord = JsonConvert.DeserializeObject<AlbumModel>(albumData);
                    foreach (PictureModel item in albumRecord.pictures)
                    {
                        item.picture = await _generwellServices.GetWebApiDetailsBytes(item.fileUrl, accessToken, tokenType);
                    }
                    return albumRecord;
                }
                return albumModel;
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
        /// Update picture using patch api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdatePicture(byte[] content, PictureModel pictureModel, string accessToken, string tokenType)
        {
            try
            {
                string pictureDetailsReecord = await _generwellServices.PutWebApiPictureData(_appSettings.Picture + "/" + pictureModel.id + "/file", accessToken, tokenType, content, pictureModel);
                return pictureDetailsReecord;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in PictureManagement AddPicture method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return string.Empty;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-22-12-2016
        /// Update picture Details fields using patch api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdatePictureDetails(string content, string pictureId, string accessToken, string tokenType)
        {
            try
            {
                string pictureDetailsReecord = await _generwellServices.UpdateWebApiData(_appSettings.Picture + "/" + pictureId, accessToken, tokenType, content);
                return pictureDetailsReecord;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in PictureManagement AddPicture method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return string.Empty;
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
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return string.Empty;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-21-12-2016
        /// Add picture Details fields using POST api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> AddPicture(byte[] content, PictureModel pictureModel, string accessToken, string tokenType)
        {
            try
            {
                string pictureDetailsReecord = await _generwellServices.PostWebApiPictureData(_appSettings.PictureFile, accessToken, tokenType, content, pictureModel);
                return pictureDetailsReecord;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in PictureManagement AddPicture method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return string.Empty;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-30-12-2016
        /// Update Task Details fields using patch api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateTaskDetails(string Content, string taskId, string accessToken, string tokenType)
        {
            try
            {
                string taskDetailsReecord = await _generwellServices.UpdateWebApiData(_appSettings.TaskDetails + "/" + taskId, accessToken, tokenType, Content);
                return taskDetailsReecord;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Picture Management UpdateTaskDetails method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return string.Empty;
            }
        }
    }
}
