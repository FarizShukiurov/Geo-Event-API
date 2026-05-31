using GeoEventApi.Data;
using GeoEventApi.DTOs;
using GeoEventApi.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GeoEventApi.Services
{
    public class EventService
    {
        private readonly AppDbContext _context;
        private readonly GeometryFactory _geometryFactory;

        public EventService(AppDbContext context)
        {
            _context = context;
            _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        }

        public async Task<Guid> CreateEventAsync(CreateEventDto model)
        {
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                EventDate = model.EventDate,

                Location = _geometryFactory.CreatePoint(new Coordinate(model.Longitude, model.Latitude))
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return newEvent.Id;
        }

        public async Task<PagedResponse<EventResponseDto>> GetEventsNearbyAsync(
            double longitude, double latitude, double radiusInMeters, PaginationParameters pagination)
        {
            var userLocation = _geometryFactory.CreatePoint(new Coordinate(longitude, latitude));

            var query = _context.Events
                .Where(e => e.Location.Distance(userLocation) <= radiusInMeters);

            var totalRecords = await query.CountAsync();

            var eventsNearby = await query
                .OrderBy(e => e.Location.Distance(userLocation))
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(e => new EventResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    Longitude = e.Location.X,
                    Latitude = e.Location.Y,
                    DistanceInMeters = e.Location.Distance(userLocation)
                })
                .ToListAsync();

            return new PagedResponse<EventResponseDto>(eventsNearby, pagination.PageNumber, pagination.PageSize, totalRecords);
        }
    }
}