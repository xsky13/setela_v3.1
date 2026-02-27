using MediatR;
using SetelaServerV3._1.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.UserFeature.Commands.ChangePasswordCommand
{
    public class ChangePasswordCommand : IRequest<Result<object>>
    {
        [Required(ErrorMessage = "No puede haber campos vacios.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage ="No puede haber campos vacios."), MinLength(6, ErrorMessage ="La contraseña debe tener por lo menos 6 caracteres.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "No puede haber campos vacios.")]
        public string NewPasswordRepeat { get; set; }
        public int UserId { get; set; }
    }
}
