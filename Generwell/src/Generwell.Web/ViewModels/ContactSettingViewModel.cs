using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Web.ViewModels
{
    public class ContactSettingViewModel
    {
        public int heartbeat { get; set; }
        public string iosDateFormat { get; set; }
        public string iosTimestampFormat { get; set; }
        public string nativeUnits { get; set; }        
    }
}
