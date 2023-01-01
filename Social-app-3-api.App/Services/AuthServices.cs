using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Social_app_3_api.Model.User;
using Social_app_3_api.Constants;
using Microsoft.AspNetCore.Identity;

namespace Social_app_3_api.Services
{
    public class AuthService
    {
        private readonly string _Secret = TokenConstants.Secret;

        private readonly PasswordHasher<User> _passwordHasher = new();

        public string GenerateToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_Secret)),
                    SecurityAlgorithms.HmacSha256Signature),
                Subject = AddClaims(user),
                Expires = DateTime.Now.AddHours(6),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity AddClaims(User user)
        {
            ClaimsIdentity claimsIdentity = new();

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
            claimsIdentity.AddClaim(new Claim("Id", user.Id!.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));
            //claimsIdentity.AddClaim(new Claim("Password", user.Password));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

            return claimsIdentity;
        }

        public string GenerateHash(User user, string password)
        {
            string hashedPassword = _passwordHasher.HashPassword(user, password);

            return hashedPassword;
        }
        public bool VerifyHash(User user, string password, string hashedPassword)
        {
            PasswordVerificationResult isValid = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
            // ajustar método para possibilidar a criação de novo hash quando necessário.
            if(isValid.ToString() == "Failed")
            {
                return false;
            }

            return true;
        }
    }
}