using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Web.ViewModels
{
    public class TaskFieldsUpdateViewModel
    {
        public int fieldId { get; set; }  
        public string fieldDesc { get; set; }  

        [Required]
        public string displayValue { get; set; }
    }
}
