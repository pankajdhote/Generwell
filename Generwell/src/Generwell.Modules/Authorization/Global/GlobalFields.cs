using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Modules.Authorization.Global
{
    public static class GlobalFields
    {
        public static string AccessToken = string.Empty;
        public static string TokenType = string.Empty;
        public static string WellId = string.Empty;
        public static string WellName = string.Empty;
        public static string IsFollow = string.Empty;        
        public static string TaskId = string.Empty;
        public static string TaskName = string.Empty;
        public static int FieldLevelId = 0;
        public static int KeyId = 0;
        public static string TimeZone = "EDT";
        public static string Active = "active";
        public static string TaskActive = string.Empty;
        public static string WellActive = "active";
        public static string FacilityActive = string.Empty;
        public static string PipelineActive = string.Empty;
        public static string ProjectActive = string.Empty;
        public static string MapActive = string.Empty;
        public static string LogoutActive = string.Empty;
        // Find the server name on the previous page
        public static string UserName = string.Empty;
        //find out objects to display on google map.
        public static string previousPage = string.Empty;
        public static string myWellCheck = string.Empty;
        public static string defaultFilter = string.Empty;

        /// <summary>
        /// Menu active and deactie part is controlled in this method
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string SetMenu(string param)
        {
            // Change active menu class
            switch (param)
            {
                case "Task":
                    WellActive = string.Empty;
                    TaskActive = Active;
                    MapActive = string.Empty;
                    break;
                case "Well":
                    WellActive = Active;
                    TaskActive = string.Empty;
                    MapActive = string.Empty;
                    break;
                case "Map":
                    MapActive = Active;
                    TaskActive = string.Empty;
                    WellActive = string.Empty;
                    break;
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
