using Microsoft.EntityFrameworkCore;
using ProdutosApp.Models;

namespace ProdutosApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; } = null!;
}
