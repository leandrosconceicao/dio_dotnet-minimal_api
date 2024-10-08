using System.ComponentModel.DataAnnotations;

namespace minimal_api.Dominio.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
