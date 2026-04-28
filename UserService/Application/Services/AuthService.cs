using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.Validators;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories;

namespace UserService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        private readonly IValidator<RegisterDto> _validator;

        public AuthService(IUserRepository repo, IConfiguration config, IValidator<RegisterDto> validator)
        {
            _repo = repo;
            _config = config;
            _validator = validator;
        }

        public async Task<string> Register(RegisterDto dto)
        {
            var result = await _validator.ValidateAsync(dto);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var existing = await _repo.GetByUsername(dto.Username);
            if (existing != null) throw new Exception("Username already exists");

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Role = Enum.Parse<UserRole>(dto.Role, true)
            };

            await _repo.AddAsync(user);
            return GenerateToken(user);
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _repo.GetByUsername(dto.Username);
            if (user == null || user.Password != dto.Password)
                throw new Exception("Invalid credentials");

            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("UserId", user.Id.ToString())
        };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
