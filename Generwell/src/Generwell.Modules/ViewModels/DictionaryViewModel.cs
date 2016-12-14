﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.ViewModels
{
    public class DictionaryViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string parentId { get; set; }
        public string manualSortFlag { get; set; }
        public List<DictionaryItemsViewModel> items { get; set; }
    }
}
