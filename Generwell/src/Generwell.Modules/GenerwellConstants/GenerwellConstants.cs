﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.GenerwellConstants
{
    public class GenerwellConstants
    {
        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Declare constant variable to use globally
        /// 
        /// </summary>
        /// <returns></returns>
        public static class Constants
        {
            public const string Token = "api/Token";
            public const string Support = "https://anar.whelby.com/api/v2016.1/support/contact";
            public const string Well = "https://anar.whelby.com/api/v2016.1/Wells";
<<<<<<< HEAD
            public const string Task = "https://anar.whelby.com/api/v2016.1/Tasks";
=======
            public const string WellFilter = "https://anar.whelby.com/api/v2016.1/wells?filterId";
            public const string WellLineReports = "https://anar.whelby.com/api/v2016.1/linereports?fieldLevelId=1";
            public const string WellDetails = "https://anar.whelby.com/api/v2016.1/wells/7361/linereports/5006";
>>>>>>> 457905fc65a8a15167d794b68fc72b22c53c1c8b
            public const string Filters = "https://anar.whelby.com/api/v2016.1/filters";
            public const string Status = "Active";
          

            public static string AccessToken = "";
            public static string TokenType = "";

            public const string NoData = "No Data";
            public static string WellId = string.Empty;

            public static string TimeZone = "EDT";

            



        }
    }
}
