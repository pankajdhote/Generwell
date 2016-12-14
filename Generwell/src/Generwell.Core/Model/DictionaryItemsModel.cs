using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Core.Model
{
    public class DictionaryItemsModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string parentId { get; set; }
        public string isActive { get; set; }
        public string sortOrder { get; set; }
    }
}
