namespace GeoEventApi.DTOs
{
    public class CreateEventDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
