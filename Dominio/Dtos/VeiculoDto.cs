using System.ComponentModel.DataAnnotations;

namespace minimal_api.Dominio.Dtos
{
    public class VeiculoDto
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        public string Marca { get; set; }

        [Required]
        [Range(1950, int.MaxValue, ErrorMessage = "Veiculo muito antigo")]
        public int Ano { get; set; }
    }
}
