using System.ComponentModel.DataAnnotations;

namespace minimal_api.Dominio.Entidades
{
    public class Veiculo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Marca { get; set; }

        [Required]
        public int Ano { get; set; }
    }
}
