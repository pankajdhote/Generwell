using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Core.Model
{
    public class DictionaryModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string parentId { get; set; }
        public string manualSortFlag { get; set; }
        public List<DictionaryItemsModel> items { get; set; }
    }
}
