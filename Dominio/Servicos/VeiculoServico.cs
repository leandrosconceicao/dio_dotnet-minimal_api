using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Servicos
{
    public class VeiculoServico(ApiContext context) : IVeiculoServico
    {
        public void Add(Veiculo veiculo)
        {
            context.Veiculos.Add(veiculo);
            context.SaveChanges();
        }

        public void Delete(Veiculo veiculo)
        {
            context.Veiculos.Remove(veiculo);
            context.SaveChanges();
        }

        public List<Veiculo> FindAll(int offset = 0, int limit = 100, string? nome = null, string? marca = null)
        {
            var query = context.Veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome)) { 
                query = query.Where(veiculo => veiculo.Name.Contains(nome));
            }
            if (!string.IsNullOrEmpty(marca)) { 
                query = query.Where(veiculo => veiculo.Marca.Contains(marca));
            }
            query.Skip(offset).Take(limit);
            return query.ToList();
        }

        public Veiculo? FindOneById(int id)
        {
            return context.Veiculos.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Veiculo veiculo)
        {
            context.Veiculos.Update(veiculo);
            context.SaveChanges();
        }
    }
}
