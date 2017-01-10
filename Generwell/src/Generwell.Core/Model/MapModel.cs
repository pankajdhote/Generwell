namespace Generwell.Core.Model
{
    public class MapModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public bool isFavorite { get; set; }
    }
}
