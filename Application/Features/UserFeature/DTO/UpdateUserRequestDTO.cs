using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.UserFeature.DTO
{
    public class UpdateUserRequestDTO
    {
        [Required(ErrorMessage = "Su nombre no puede estar vacio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El email no puede estar vacio"), EmailAddress(ErrorMessage = "El email es invalido")]
        public string Email { get; set; }
        public string? Password { get; set; }

    }
}
