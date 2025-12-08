using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SetelaServerV3._1.Application.Features.Auth.Config;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SetelaServerV3._1.Application.Features.Auth.Commands.RegisterCommand
{
    public class RegisterHandler(AppDbContext _db, IOptions<AuthOptions> options) : IRequestHandler<RegisterCommand, string>
    {
        private readonly AuthOptions _options = options.Value;
        public async Task<string> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var userWithEmail = await _db.SysUsers.FirstOrDefaultAsync(user => user.Email == command.Email, cancellationToken);
            if (userWithEmail != null)
                throw new InvalidOperationException("Un usuario con ese email ya existe");

            var newUser = new SysUser
            {
                Name = command.Name,
                Email = command.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
                Roles = [UserRoles.Student]
            };

            _db.SysUsers.Add(newUser);

            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Sub, newUser.Id.ToString()),
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
