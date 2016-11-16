using System;
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
            public const string Task = "https://anar.whelby.com/api/v2016.1/Tasks";
            public const string Filters = "https://anar.whelby.com/api/v2016.1/filters";
            public const string Status = "Active";          

            public static string AccessToken = "";
            public static string TokenType = "";

            public const string NoData = "No Data";



        }
    }
}
