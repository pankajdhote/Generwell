using AutoMapper;
using Generwell.Core.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generwell.Modules.Management.PictureManagement
{
    public class PictureManagement : IPictureManagement
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        private readonly IMapper _mapper;

        public PictureManagement(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _mapper = mapper;
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
                string albumData = await _generwellServices.GetWebApiDetails(_appSettings.PictureAlbum + "/" + Convert.ToDouble(id), accessToken, tokenType);
                AlbumModel albumRecord = JsonConvert.DeserializeObject<AlbumModel>(albumData);
                foreach (PictureModel item in albumRecord.pictures)
                {
                    item.picture = await _generwellServices.GetWebApiDetailsArray(item.fileUrl, accessToken, tokenType);
                }
                return albumRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
