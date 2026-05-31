using GeoEventApi.Data;
using GeoEventApi.DTOs;
using GeoEventApi.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using GeoEventApi.Settings;
using Microsoft.Extensions.Options;

namespace GeoEventApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, JwtService jwtService, IOptions<JwtSettings> jwtSettings, ILogger<AuthService> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingEmail != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Username}", newUser.Username);

            var token = _jwtService.GenerateToken(newUser);

            return new AuthResponseDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                Message = "User registered successfully",
                Token = token,
                ExpiresIn = _jwtSettings.ExpirationMinutes * 60
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: user not found. Username: {Username}", request.Username);
                throw new InvalidOperationException("Invalid username or password");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Login attempt failed: invalid password. Username: {Username}", request.Username);
                throw new InvalidOperationException("Invalid username or password");
            }

            _logger.LogInformation("User logged in successfully: {Username}", user.Username);

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Message = "User logged in successfully",
                Token = token,
                ExpiresIn = _jwtSettings.ExpirationMinutes * 60
            };
        }
    }
}
