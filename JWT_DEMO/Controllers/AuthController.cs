using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_DEMO.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin model)
        {
            var user = GetUser(model);
            // Validate user credentials
            if (user != null)
            {
                var token = GenerateJwtToken(user.Username, user.Role);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(string username, string role)
        {
            var claims = new[]
            {
            new Claim("Role", role),
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User GetUser(UserLogin model)
        {
            using StreamReader reader = new("./user.json");
            var json = reader.ReadToEnd();
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);

            return users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
        }
    }

}
