using BCrypt.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SetelaServerV3._1.Application.Features.Auth.Config;
using SetelaServerV3._1.Application.Features.Auth.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SetelaServerV3._1.Application.Features.Auth.Commands.LoginCommand
{
    public class LoginHandler(AppDbContext _db, IOptions<AuthOptions> options) : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly AuthOptions _options = options.Value;

        public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _db.SysUsers.FirstOrDefaultAsync(user => user.Email == command.Email, cancellationToken);
            
            if (user == null)
                return Result<LoginResponse>.Fail("Email invalida");

            if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
                return Result<LoginResponse>.Fail("Contrasena invalida");


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
            return Result<LoginResponse>.Ok(new LoginResponse { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
