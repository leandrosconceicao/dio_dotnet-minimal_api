using System.ComponentModel.DataAnnotations;
using minimal_api.Dominio.Enums;

namespace minimal_api.Dominio.Dtos
{
    public class AdministradorDto
    {
        public class BodyRequest
        {
            public Perfil? _Perfil;
            [Required]
            public string Email { get; set; } = default!;

            [Required]
            public string Senha { get; set; } = default!;

            [Required]
            public Perfil? Perfil { get {
                    return _Perfil;            
            } set {
                    if (value.ToString() == "Adm")
                    {
                        _Perfil = Enums.Perfil.Adm;
                    }
                    else
                    {
                        _Perfil = Enums.Perfil.Editor;
                    }
            } }

        }

        public class BodyResponse
        {
            public string Email { get; set; } = default!;
            public Perfil? Perfil { get; set; }
        }
    }
}
