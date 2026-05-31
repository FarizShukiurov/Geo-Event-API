using NetTopologySuite.Geometries;

namespace GeoEventApi.Models
{
    public class Event
    {
        public Guid Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }

        public Point Location { get; set; } = null!;
    }
}
