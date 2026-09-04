using Microsoft.EntityFrameworkCore;
using ProdutosApp.Data;
using ProdutosApp.Models;

namespace ProdutosApp.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
        => await _context.Produtos.AsNoTracking().ToListAsync();

    public async Task<Produto?> ObterPorIdAsync(int id)
        => await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task CriarAsync(Produto produto)
        => await _context.Produtos.AddAsync(produto);

    public Task AtualizarAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Produto produto)
    {
        _context.Produtos.Remove(produto);
        return Task.CompletedTask;
    }

    public async Task SalvarAsync() 
        => await _context.SaveChangesAsync();
}