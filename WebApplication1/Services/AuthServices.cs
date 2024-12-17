using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Data;

namespace WebApplication1.Services
{
    public class AuthServices: IAuthServices
    {
        private readonly AppDbContext _context;
        private const string SecretKey = "YourSuperSecretKey12345YourSuperSecretKey12345";

        public AuthServices(AppDbContext context)
        {
            _context = context;
        }
        //private readonly Dictionary<string, string> _user = new()
        //{
        //    {"admin" , "password" },
        //    {"user" , "1234" }
        //};
        public async Task<bool> ValidateUser(string username, string password)
        {
            //return _user.TryGetValue(username, out var stroedpassword) && stroedpassword == password;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Name == username);

            // Replace this with hashed password comparison in production
            if (user != null && user.Password == password)
                return true;

            return false;
        }

        public string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username), // Add the username
                    new Claim(ClaimTypes.Role, "User") // Add roles or other claims
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
