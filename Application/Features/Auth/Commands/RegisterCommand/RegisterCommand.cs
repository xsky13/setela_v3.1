using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.Auth.Commands.RegisterCommand
{
    public class RegisterCommand : IRequest<string>
    {
        [Required(ErrorMessage = "Su nombre no puede estar vacio")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; }
        [MinLength(6, ErrorMessage = "Su contrasena debe tener por lo menos 6 caracteres")]
        public string Password { get; set; }
    }
}
