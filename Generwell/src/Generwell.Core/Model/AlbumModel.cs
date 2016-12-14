using System.Collections.Generic;

namespace Generwell.Core.Model
{
    public class AlbumModel
    {
        public string id { get; set; }
        public List<PictureModel> pictures { get; set; }
        public string url { get; set; }
    }
}
