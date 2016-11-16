using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Web.ViewModels
{
    public class LineReportsViewModel
    {
        public int entityId { get; set; }
        public int reportId { get; set; }
        public List<FieldsViewModel> fields { get; set; }
        public string url { get; set; }

    }
}
