using MediatR;
using SetelaServerV3._1.Application.Features.Auth.DTO;
using SetelaServerV3._1.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.Auth.Commands.RegisterCommand
{
    public class RegisterCommand : IRequest<Result<LoginResponse>>
    {
        [Required(ErrorMessage = "Su nombre no puede estar vacio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Su email no puede estar vacio"), EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Su contrasena no puede estar vacio"), MinLength(6, ErrorMessage = "Su contrasena debe tener por lo menos 6 caracteres")]
        public string Password { get; set; }
    }
}