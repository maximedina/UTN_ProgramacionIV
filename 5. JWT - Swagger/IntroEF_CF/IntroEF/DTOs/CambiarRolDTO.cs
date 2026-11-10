using System.ComponentModel.DataAnnotations;

namespace IntroEF.DTOs
{
    public class CambiarRolDTO
    {
        [Required]
        public string Rol { get; set; } = null!;
    }
}
