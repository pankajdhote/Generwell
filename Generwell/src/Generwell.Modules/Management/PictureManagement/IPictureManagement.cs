using Generwell.Core.Model;
using System.Threading.Tasks;

namespace Generwell.Modules.Management.PictureManagement
{
    public interface IPictureManagement
    {
        Task<AlbumModel> GetPictureAlbum(string id, string accessToken, string tokenType);
        Task<PictureModel> GetPicture(string fileUrl, string accessToken, string tokenType);
        Task<string> UpdatePicture(byte[] content, PictureModel pictureModel, string accessToken, string tokenType);
        Task<string> UpdatePictureDetails(string content, string pictureId, string accessToken, string tokenType);
        Task<string> DeletePicture(string pictureId, string accessToken, string tokenType);
        Task<string> AddPicture(byte[] content, PictureModel pictureModel, string accessToken, string tokenType);
        Task<string> UpdateTaskDetails(string Content, string taskId, string accessToken, string tokenType);
    }
}
