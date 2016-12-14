using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Core.Model
{
    public class PictureModel
    {
        public string id { get; set; }
        public string albumId { get; set; }
        public string label { get; set; }
        public string comment { get; set; }
        public string fileUrl { get; set; }
        public string url { get; set; }
        public string picture { get; set; }
    }
}
