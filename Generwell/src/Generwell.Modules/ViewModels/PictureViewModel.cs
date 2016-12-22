using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.ViewModels
{
    public class PictureViewModel
    {
        public string id { get; set; }
        public string albumId { get; set; }
        public string label { get; set; }
        public string comment { get; set; }
        public string fileUrl { get; set; }
        public string url { get; set; }
        public byte[] picture { get; set; }

    }
}
