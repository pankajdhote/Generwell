using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Core.Model
{
    public class FieldsModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? position { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string excelFormat { get; set; }
        public int? fieldTypeId { get; set; }
        public string value { get; set; }
    }
}
