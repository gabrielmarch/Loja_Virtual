using Loja_Virtual.model;

namespace Loja_Virtual.data
{
    public interface ICupomRepository
    {
        Task<List<Cupom>> ListarAsync();
        Task<int> CadastrarAsync(Cupom cupom);
        Task<bool> AtualizarAsync(Cupom cupom);
        Task<bool> AlterarStatusAsync(int idCupom, bool ativo);
    }
}
