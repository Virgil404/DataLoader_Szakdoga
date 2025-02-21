using System.Security.Claims;
using System.Text;
using DataloaderApi.Data;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DataloaderApi.Auth
{
    public class TokenProvider
    {
        private readonly IConfiguration _configuration;
            
        public TokenProvider(IConfiguration configuration) 
        {
        
            _configuration = configuration;
        
        }

        public Token GenerateToken (User user)
        {

            var AccessToken = GenerateAccessToken (user);

            return new Token { AccesToken = AccessToken };


        }

        private string  GenerateAccessToken(User user)
        {

            string secretkey = _configuration["JWT:SecretKey"];
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
            var credentials = new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (
                    [
                    new Claim (ClaimTypes.Name, user.UserID),
                    new Claim (ClaimTypes.Role, user.Role)


                    ]
                 ),
                 Expires = DateTime.UtcNow.AddMinutes (1),
                 SigningCredentials = credentials, 
                 Issuer = _configuration["JWT:Issuer"],
                 Audience = _configuration["JWT:Audience"]
                  
            };

            return new JsonWebTokenHandler().CreateToken (tokenDescriptor);


        }
    }
}
