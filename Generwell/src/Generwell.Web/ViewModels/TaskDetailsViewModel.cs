using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Web.ViewModels
{
    public class TaskDetailsViewModel
    {
        public string id { get; set; }
        public int TaskId { get; set; }
        public string Name { get; set; }
        public int ActivatedDate { get; set; }
        public int? ForecastDate { get; set; }
        public int? ExpectedCompletionDate { get; set; }
    }
}
