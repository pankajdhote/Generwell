using Generwell.Core.Model;
using System.Threading.Tasks;

namespace Generwell.Modules.Management.PictureManagement
{
    public interface IPictureManagement
    {
        Task<AlbumModel> GetPictureAlbum(string id, string accessToken, string tokenType);
        Task<PictureModel> GetPicture(string fileUrl, string accessToken, string tokenType);
    }
}
