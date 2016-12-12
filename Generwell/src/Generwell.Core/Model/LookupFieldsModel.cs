using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Core.Model
{
    public class LookupFieldsModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parentId { get; set; }
        public int manualSortFlag { get; set; }
        public List<LookupFieldsItemsModel> items;
    }
}
