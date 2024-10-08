using minimal_api.Dominio.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace minimal_api.Dominio.Entidades
{
    public class Administradores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default!;

        [StringLength(255)]
        [Required]
        public string Email { get; set; } = default!;

        [StringLength(50)]
        [Required]
        public string Senha { get; set; } = default!;

        [StringLength(10)]
        public string Perfil { get; set; } = default!;
    }
}
