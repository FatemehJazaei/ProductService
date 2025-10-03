using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductService.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductService.Api.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class AccountController : ControllerBase
        {
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly IConfiguration _config;

            public AccountController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IConfiguration config)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _config = config;
            }

            /// <summary>
            /// ثبت‌نام کاربر جدید
            /// </summary>
            /// <param name="model">اطلاعات ثبت‌نام (نام کاربری، ایمیل، رمز عبور)</param>
            /// <returns>نتیجه ثبت‌نام</returns>
            [HttpPost("register")]
            [SwaggerOperation(
                Summary = "ثبت‌نام کاربر",
                Description = "یک کاربر جدید با اطلاعات وارد شده ایجاد می‌کند."
            )]
            public async Task<IActionResult> Register([FromBody] RegisterDto model)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    return Ok("User registered successfully");

                return BadRequest(result.Errors);
            }

            /// <summary>
            /// ورود کاربر و دریافت توکن JWT
            /// </summary>
            /// <param name="model">اطلاعات ورود (نام کاربری و رمز عبور)</param>
            /// <returns>توکن JWT برای دسترسی به API</returns>
            [HttpPost("login")]
            [SwaggerOperation(
                Summary = "ورود کاربر",
                Description = "اطلاعات ورود را بررسی کرده و در صورت موفقیت یک JWT Token برمی‌گرداند."
            )]
            public async Task<IActionResult> Login([FromBody] LoginDto model)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                    return Unauthorized("Invalid credentials");

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded)
                    return Unauthorized("Invalid credentials");

                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            private string GenerateJwtToken(User user)
            {
                var jwtSettings = _config.GetSection("JwtSettings");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                if (!string.IsNullOrEmpty(user.Email))
                    claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

                var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }

        // DTOs
        public class RegisterDto
        {
            [Required]
            public string UserName { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, MinLength(6)]
            public string Password { get; set; }
        }

    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}