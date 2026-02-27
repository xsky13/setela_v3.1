using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.UserFeature.DTO
{
    public class ChangePasswordRequestDTO
    {
        [Required(ErrorMessage = "La contraseña inicial no puede estar vacía.")]
        public required string OldPassword { get; set; }
        [Required(ErrorMessage = "La contraseña nueva no puede estar vacía."), MinLength(6, ErrorMessage = "La contraseña nueva debe tener por lo menos 6 caracteres.")]
        public required string NewPassword { get; set; }
        [Required(ErrorMessage = "La repetición de la contraseña nueva no puede estar vacía.")]
        public required string NewPasswordRepeat { get; set; }
    }
}
