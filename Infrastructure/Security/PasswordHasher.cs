using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security
{
    public class PasswordHasher
    {
        private readonly SymmetricSecurityKey _key;

        public PasswordHasher(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string HashPassword(AppUser user, string password)
        {
            // Implement password hashing according to the System.IdentityModel.Tokens.Jwt documentation
            throw new NotImplementedException();
        }

        public bool VerifyHashedPassword(AppUser user, string hashedPassword, string providedPassword)
        {
            // Implement password verification according to the System.IdentityModel.Tokens.Jwt documentation
            throw new NotImplementedException();
        }
    }
}