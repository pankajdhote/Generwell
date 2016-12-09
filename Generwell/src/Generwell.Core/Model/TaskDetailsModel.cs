using System.Collections.Generic;

namespace Generwell.Core.Model
{
    public class TaskDetailsModel
    {
        
        public string id { get; set; }
        public int taskId { get; set; }
        public int keyId { get; set; }
        public int fieldLevelId { get; set; }
        public string keyName { get; set; }
        public int moduleId { get; set; }
        public string status { get; set; }
        public int? forecastDate { get; set; }
        public int? completedDate { get; set; }
        public int? expectedCompletionDate { get; set; }
        public int? activatedDate { get; set; }
        public bool completed { get; set; }
        public int? completedByUserId { get; set; }
        public int? taskIconId { get; set; }
        public string assigneeName { get; set; }
        public string groupName { get; set; }
       
        public List<TaskFieldsModel> fields;
        public string name { get; set; }
        public string url { get; set; }

        public ContactFieldsModel contactFields { get; set; }

        public TaskFieldsUpdateModel TaskFields { get; set; }
       
    }
}
