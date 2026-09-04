using ProdutosApp.Models;

namespace ProdutosApp.Services;

public interface IProdutoService
{
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<Produto?> ObterPorIdAsync(int id);
    Task CriarAsync(Produto produto);
    Task<bool> AtualizarAsync(int id, Produto produto);
    Task<bool> RemoverAsync(int id);
}