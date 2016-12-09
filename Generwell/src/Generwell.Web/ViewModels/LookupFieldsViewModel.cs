using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Web.ViewModels
{
    public class LookupFieldsViewModel
    {
        public int id { get; set; }  
        public string name { get; set; }
        public int parentId { get; set; }
        public int manualSortFlag { get; set; }
        public List<LookupFieldsItemsViewModel> items;
    }
}
