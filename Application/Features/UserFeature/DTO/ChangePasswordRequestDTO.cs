using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.UserFeature.DTO
{
    public class ChangePasswordRequestDTO
    {
        [Required(ErrorMessage = "No puede haber campos vacios.")]
        public required string OldPassword { get; set; }
        [Required(ErrorMessage = "No puede haber campos vacios."), MinLength(6, ErrorMessage = "La contraseña debe tener por lo menos 6 caracteres.")]
        public required string NewPassword { get; set; }
        [Required(ErrorMessage = "No puede haber campos vacios.")]
        public required string NewPasswordRepeat { get; set; }
    }
}
