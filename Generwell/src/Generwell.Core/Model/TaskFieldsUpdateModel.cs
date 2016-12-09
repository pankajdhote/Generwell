using System.ComponentModel.DataAnnotations;

namespace Generwell.Core.Model
{
    public class TaskFieldsUpdateModel
    {
        public int fieldId { get; set; }  
        public string fieldDesc { get; set; }  

        [Required]
        public string displayValue { get; set; }
    }
}
