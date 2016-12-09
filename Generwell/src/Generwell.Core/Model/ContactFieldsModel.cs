namespace Generwell.Core.Model
{
    public class ContactFieldsModel
    {
        //need to implement later by rohit. do not remove.
        //public List<ContactSettingViewModel> settings;
        public object id { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailBusiness { get; set; }
        public int? phoneBusiness { get; set; }
        public string company { get; set; }
        public bool isActive { get; set; }
        public bool sameOrgUnitFlag { get; set; }
        public bool isSystemUser { get; set; }
        public int? groups { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }
}
