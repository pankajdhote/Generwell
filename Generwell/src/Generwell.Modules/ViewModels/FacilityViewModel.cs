using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.ViewModels
{
    public class FacilityViewModel
    {
        public string id { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string activeTaskCount { get; set; }
        public bool isFavorite { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }
}
