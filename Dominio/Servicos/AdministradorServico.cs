using minimal_api.Dominio.Dtos;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Servicos
{
    public class AdministradorServico(ApiContext context) : IAdministradorServico
    {
        public bool Add(Administradores dto)
        {
            context.Administradores.Add(dto);
            return context.SaveChanges() > 0;
        }

        public Administradores? Login(LoginDto dto)
        {
            return context.Administradores.FirstOrDefault(adm => adm.Email == dto.Email && adm.Senha == dto.Senha);

        }

        public Administradores? FindOneById(int id)
        {
            return context.Administradores.FirstOrDefault(adm => adm.Id == id);
        }

        public List<Administradores> FindAll()
        {
            return context.Administradores.ToList();
        }
    }
}
