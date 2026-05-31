using GeoEventApi.DTOs;
using GeoEventApi.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GeoEventApi.Controllers
{
    /// <summary>
    /// Controller for managing geo-located events (create and retrieve nearby events).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Creates a new event at the specified geographic location.
        /// </summary>
        /// <param name="model">Event details including Title, Description, EventDate, and Location coordinates</param>
        /// <returns>ID of the newly created event</returns>
        /// <remarks>
        /// Requires a valid JWT token for authentication.
        /// Sample request:
        /// POST /api/events
        /// Authorization: Bearer {your_jwt_token}
        /// {
        ///   "title": "Concert in Downtown",
        ///   "description": "Live music event",
        ///   "eventDate": "2026-06-15T19:00:00",
        ///   "location": {
        ///     "type": "Point",
        ///     "coordinates": [-74.0060, 40.7128]
        ///   }
        /// }
        /// </remarks>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateEventDto model)
        {
            var eventId = await _eventService.CreateEventAsync(model);

            return CreatedAtAction(nameof(GetNearby), new { id = eventId }, new { Id = eventId });
        }

        /// <summary>
        /// Retrieves events near the specified geographic location with pagination support.
        /// </summary>
        /// <param name="lng">Longitude of the search center point</param>
        /// <param name="lat">Latitude of the search center point</param>
        /// <param name="radius">Search radius in meters (default: 5000m)</param>
        /// <param name="pagination">Pagination parameters (PageNumber, PageSize)</param>
        /// <returns>Paginated list of events within the specified radius</returns>
        /// <remarks>
        /// Sample request:
        /// GET /api/events/nearby?lng=-74.0060&lat=40.7128&radius=5000&pageNumber=1&pageSize=10
        /// </remarks>
        [HttpGet("nearby")]
        [ProducesResponseType(typeof(PagedResponse<EventResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<EventResponseDto>>> GetNearby(
            [FromQuery] double lng,
            [FromQuery] double lat,
            [FromQuery] double radius = 5000,
            [FromQuery] PaginationParameters pagination = null!)
        {
            pagination ??= new PaginationParameters();

            var pagedResult = await _eventService.GetEventsNearbyAsync(lng, lat, radius, pagination);

            return Ok(pagedResult);
        }
    }
}