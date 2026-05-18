using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.DTO;
using UserService.Models;
using UserService.Repository;
using UserService.Services;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly JWTservice _jwtService;

        public UserController(IUserRepository repo, JWTservice jwtService)
        {
            _repo = repo;
            _jwtService = jwtService;
        }

        // Register User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _repo.GetUserByEmail(dto.Email);

            if (existingUser != null)
            {
                return BadRequest(new
                {
                    message = "User with this email already exists."
                });
            }

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                MobileNo = dto.MobileNo,
                CreatedDate = DateTime.UtcNow,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            await _repo.Register(user);

            return Ok(new
            {
                message = "User registered successfully"
            });
        }

        // Login User
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _repo.GetUserByEmail(dto.Email);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token,
                userId = user.UserId,
                email = user.Email,
                name = user.FullName,
                role = user.Role
            });
        }

        // Current Logged-in User
        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                userId,
                email,
                role
            });
        }

        // Admin - Get All Users
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repo.GetAllUsers();

            return Ok(users.Select(u => new
            {
                u.UserId,
                u.Email,
                u.FullName,
                u.MobileNo,
                u.Role,
                u.CreatedDate
            }));
        }

        // Admin - Get User By Id
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _repo.GetUserById(id);

            if (user == null)
                return NotFound(new
                {
                    message = "User not found"
                });

            return Ok(new
            {
                user.UserId,
                user.Email,
                user.FullName,
                user.MobileNo,
                user.Role,
                user.CreatedDate
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _repo.GetUserById(id);

            if (existingUser == null)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            existingUser.FullName = dto.FullName;
            existingUser.Email = dto.Email;
            existingUser.MobileNo = dto.MobileNo;

            // Optional Password Update
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                existingUser.Password =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            await _repo.UpdateUser(existingUser);

            return Ok(new
            {
                message = "User updated successfully"
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _repo.GetUserById(id);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            // Prevent Admin deletion
            if (user.Role == "Admin")
            {
                return BadRequest(new
                {
                    message = "Admin user cannot be deleted"
                });
            }

            var result = await _repo.DeleteUser(id);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Delete failed"
                });
            }

            return Ok(new
            {
                message = "User deleted successfully"
            });
        }
    }
}
