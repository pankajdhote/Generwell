using System.ComponentModel.DataAnnotations;

namespace Generwell.Core.Model
{
    public class TaskFieldsModel
    {
        public int taskId { get; set; }
        public int keyId { get; set; }
        public int mainKeyId { get; set; }
        public int fieldLevelId { get; set; }
        public int fieldId { get; set; }
        public int fieldTypeId { get; set; }
        public int unitId { get; set; }
        public int moduleId { get; set; }
        public string fieldTable { get; set; }
        public string fieldName { get; set; }
        public string fieldDesc { get; set; }
        public int fieldDecimals { get; set; }
        public string fieldLabel { get; set; }
        public int fieldControlType { get; set; }
        public int? dictId { get; set; }
        public int? contactGroupId { get; set; }
        public int lookupLevelId { get; set; }
        public string unitString { get; set; }
        public int permissionId { get; set; }
        public int numberMin { get; set; }
        public int numberMax { get; set; }
        public int fieldMaxLength { get; set; }
        public int fieldComma { get; set; }
        public int? fieldIdParent { get; set; }
        public int? albumMaxCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public string value { get; set; }

        [Required]
        public string displayValue { get; set; }

       

    }
}
