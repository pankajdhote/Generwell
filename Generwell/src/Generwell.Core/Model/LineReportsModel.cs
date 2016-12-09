using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Core.Model
{
    public class LineReportsModel
    {
        public int entityId { get; set; }
        public int reportId { get; set; }
        public List<FieldsModel> fields { get; set; }
        public string url { get; set; }

    }
}
