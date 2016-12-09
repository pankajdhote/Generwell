using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Web.ViewModels
{
    public class LookupFieldsItemsViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parentId { get; set; }
        public bool isActive { get; set; }
        public int sortOrder { get; set; }
        
    }
}
