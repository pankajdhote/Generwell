using Generwell.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.Management.PictureManagement
{
    public interface IPictureManagement
    {
        Task<AlbumModel> GetPictureAlbum(string id, string accessToken, string tokenType);
    }
}
