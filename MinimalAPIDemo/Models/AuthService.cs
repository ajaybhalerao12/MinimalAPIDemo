using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalAPIDemo.Models
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration) {
            _configuration = configuration;
        }

        // Method to generate a JWT token for a given username
        public string GenerateJwtToken(string username)
        {
            // Create a new SymmetricSecurityKey using the secret key from configuration.
            // The key is encoded in UTF8 and used for signing the JWT.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Create signing credentials using the security key and the HMAC-SHA256 algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define the claims to be included in the JWT.
            // Claims are name/value pairs that assert information about the subject,
            // such as the username and a unique identifier for the token.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username), // Subject claim with the username
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique identifier claim with a new GUID
            };

            // Create a new JWT token with the specified issuer, audience, claims, expiration time, and signing credentials
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // 'Issuer' - the party generating the token
                audience: _configuration["Jwt:Audience"], // 'Audience' - the intended recipient of the token
                claims: claims, // Claims contained within the JWT
                expires: DateTime.Now.AddHours(1), // Set the expiration time of the token (1 hour from the current time)
                signingCredentials: credentials); // Credentials used to sign the token, ensuring its validity
            
            // Serialize the token to a string and return it
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
