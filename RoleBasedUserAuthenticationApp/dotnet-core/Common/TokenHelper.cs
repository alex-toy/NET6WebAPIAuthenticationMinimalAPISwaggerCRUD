using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace startup_kit_api.Common
{
    public class TokenHelper
    {
        public static string Encrypt(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool IsCredentialsOK(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static string GenerateJWTToken(long userId, bool rememberMe)
        {
            byte[] secret = Encoding.UTF8.GetBytes(Properties.Resources.JWT_Secret);
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(secret);
            SecurityTokenDescriptor tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]{
                        new Claim("userId", userId.ToString())
                    }
                ),
                Expires = rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(securityToken);
        }

        public static string GenerateJWTTokenByEmail(string email)
        {
            SecurityTokenDescriptor tokenDescription = GetTokenDescription(email);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(securityToken);
        }

        private static SecurityTokenDescriptor GetTokenDescription(string email)
        {
            Claim[] claims = GetClaims(email);
            byte[] secret = Encoding.UTF8.GetBytes(Properties.Resources.JWT_Secret);
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(secret);

            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
        }

        private static Claim[] GetClaims(string email)
        {
            return new Claim[] { new Claim("email", email) };
        }
    }
}