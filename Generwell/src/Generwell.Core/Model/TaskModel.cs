using System.ComponentModel.DataAnnotations;

namespace Generwell.Core.Model
{
    public class TaskModel
    {
        public string id { get; set; }       
        public int TaskId { get; set; }
        public int KeyId { get; set; }
        public int FieldLevelId { get; set; }
        public string KeyName { get; set; }
        public int ModuleId { get; set; }
        public string Status { get; set; }
        public int? ForecastDate  { get; set; }
        public int? CompletedDate { get; set; }
        public int? ExpectedCompletionDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM-yyyy}")]
        public int? ActivatedDate { get; set; }

        //public int? ActivatedDate { get; set; }
        public bool Completed { get; set; }
        public int? CompletedByUserId { get; set; }
        public int? TaskIconId { get; set; }
        public string AssigneeName { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        
    }
}
