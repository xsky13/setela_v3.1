using BCrypt.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SetelaServerV3._1.Application.Features.Auth.Config;
using SetelaServerV3._1.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SetelaServerV3._1.Application.Features.Auth.Commands
{
    public class LoginHandler(AppDbContext _db, IOptions<AuthOptions> options) : IRequestHandler<LoginCommand, string>
    {
        private readonly AuthOptions _options = options.Value;

        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _db.SysUsers.FirstOrDefaultAsync(user => user.Email == command.Email) ?? throw new UnauthorizedAccessException("Email invalido");
            
            if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Contrasena invalida");


            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            ];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpMinutes),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
