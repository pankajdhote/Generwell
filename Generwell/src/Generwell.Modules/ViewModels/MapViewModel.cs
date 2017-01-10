﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.ViewModels
{
    public class MapViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public bool isFavorite { get; set; }
    }
}
