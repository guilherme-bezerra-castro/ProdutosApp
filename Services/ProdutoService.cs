using ProdutosApp.Models;
using ProdutosApp.Repositories;

namespace ProdutosApp.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;

    public ProdutoService(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
        => await _repository.ObterTodosAsync();

    public async Task<Produto?> ObterPorIdAsync(int id)
        => await _repository.ObterPorIdAsync(id);

    public async Task CriarAsync(Produto produto)
    {
        produto.DataCadastro = DateTime.Now; // garante que a data é sempre a do servidor
        await _repository.CriarAsync(produto);
        await _repository.SalvarAsync();
    }

    public async Task<bool> AtualizarAsync(int id, Produto produto)
    {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente is null) return false;

        existente.Descricao = produto.Descricao;
        existente.Quantidade = produto.Quantidade;
        existente.Valor = produto.Valor;
        existente.UsuarioCadastro = produto.UsuarioCadastro;
        // DataCadastro não é alterada na edição — reflete quando o produto foi criado

        await _repository.AtualizarAsync(existente);
        await _repository.SalvarAsync();
        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente is null) return false;

        await _repository.RemoverAsync(existente);
        await _repository.SalvarAsync();
        return true;
    }
}