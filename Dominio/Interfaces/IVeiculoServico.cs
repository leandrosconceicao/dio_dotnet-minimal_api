using minimal_api.Dominio.Entidades;

namespace minimal_api.Dominio.Interfaces
{
    public interface IVeiculoServico
    {
        List<Veiculo> FindAll(int offset = 0, int limit = 100, string? nome = null, string? marca = null);

        Veiculo? FindOneById(int id);

        void Add(Veiculo veiculo);
        
        void Update(Veiculo veiculo);

        void Delete(Veiculo veiculo);
    }
}
