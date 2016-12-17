using System.Collections.Generic;

namespace Generwell.Modules.ViewModels
{
    public class ContactFieldsViewModel
    {
        public object id { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailBusiness { get; set; }
        public int? phoneBusiness { get; set; }

        public List<ContactGroupViewModel> groups;

    }
}
