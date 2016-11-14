using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Web.ViewModels
{
    public class WellViewModel
    {
        public int id { get; set; }
        public string bottomHoleLocation { get; set; }
        public string surfaceHoleLocation { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int activeTaskCount { get; set; }
        public bool isFavorite { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }
}
