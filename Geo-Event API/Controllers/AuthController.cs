using GeoEventApi.DTOs;
using GeoEventApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GeoEventApi.Controllers
{
    /// <summary>
    /// Controller for user authentication operations (register, login, and current user info).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="request">Registration data containing Username, Email, and Password</param>
        /// <returns>Newly created user with access token</returns>
        /// <remarks>
        /// Sample request:
        /// POST /api/auth/register
        /// {
        ///   "username": "john_doe",
        ///   "email": "john@example.com",
        ///   "password": "SecurePassword123!"
        /// }
        /// </remarks>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var response = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), new { id = response.Id }, response);
        }

        /// <summary>
        /// Authenticates a user and issues a JWT access token.
        /// </summary>
        /// <param name="request">Login credentials containing Username and Password</param>
        /// <returns>Access token and user information</returns>
        /// <remarks>
        /// Sample request:
        /// POST /api/auth/login
        /// {
        ///   "username": "john_doe",
        ///   "password": "SecurePassword123!"
        /// }
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves information about the currently authenticated user.
        /// </summary>
        /// <returns>Current user details (Id, Username, Email)</returns>
        /// <remarks>
        /// Requires a valid JWT token in the Authorization header.
        /// Sample request:
        /// GET /api/auth/me
        /// Authorization: Bearer {your_jwt_token}
        /// </remarks>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                id = userId,
                username = username,
                email = email,
                message = "Current authenticated user"
            });
        }
    }
}
