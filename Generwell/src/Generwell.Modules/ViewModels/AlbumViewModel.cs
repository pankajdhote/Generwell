using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.ViewModels
{
    public class AlbumViewModel
    {
        public string id { get; set; }
        public List<PictureViewModel> pictures { get; set; }
        public string url { get; set; }
        //Added for notification
        public string flagCheck { get; set; }

    }
}
