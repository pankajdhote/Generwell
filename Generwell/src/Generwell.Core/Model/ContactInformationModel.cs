using System.Collections.Generic;

namespace Generwell.Core.Model
{
    public class ContactInformationModel
    {
        public object id { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailBusiness { get; set; }
        public string phoneBusiness { get; set; }
        public string company { get; set; }
        public bool isActive { get; set; }
        public bool sameOrgUnitFlag { get; set; }
        public bool isSystemUser { get; set; }

        public List<ContactGroupModel> groups;
        public string name { get; set; }
        public string url { get; set; }
    }
}
