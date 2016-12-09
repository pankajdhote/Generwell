using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.ViewModels
{
    public class ContactSettingViewModel
    {
        public int heartbeat { get; set; }
        public DateTime iosDateFormat { get; set; }
        public DateTime iosTimestampFormat { get; set; }
        public string nativeUnits { get; set; }        
    }
}
