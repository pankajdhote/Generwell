namespace Generwell.Modules.Global
{
    public static class GlobalFields
    {
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
                    FacilityActive = string.Empty;
                    break;
                case "Well":
                    WellActive = Active;
                    TaskActive = string.Empty;
                    MapActive = string.Empty;
                    FacilityActive = string.Empty;
                    break;
                case "Map":
                    MapActive = Active;
                    TaskActive = string.Empty;
                    WellActive = string.Empty;
                    FacilityActive = string.Empty;
                    break;
                case "Facility":
                    FacilityActive = Active;
                    TaskActive = string.Empty;
                    WellActive = string.Empty;
                    MapActive = string.Empty;
                    break;
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
