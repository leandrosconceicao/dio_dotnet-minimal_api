using minimal_api.Dominio.Dtos;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        Administradores? Login(LoginDto dto);

        bool Add(Administradores dto);

        Administradores? FindOneById(int id);

        List<Administradores> FindAll();
    }
}
