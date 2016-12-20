using Generwell.Core.Model;
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
    }
}
