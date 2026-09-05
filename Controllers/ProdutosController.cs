using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProdutosApp.Models;
using ProdutosApp.Services;

namespace ProdutosApp.Controllers;

[Authorize]
public class ProdutosController : Controller
{
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    // GET /Produtos
    public async Task<IActionResult> Index(
        string? filtroDescricao,
        string? filtroUsuario,
        string sortColumn = "Id",
        string sortDirection = "asc")
    {
        var produtos = await _produtoService.ObterTodosAsync();

        if (!string.IsNullOrWhiteSpace(filtroDescricao))
        {
            produtos = produtos.Where(p =>
                p.Descricao.Contains(filtroDescricao, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filtroUsuario))
        {
            produtos = produtos.Where(p =>
                p.UsuarioCadastro.Contains(filtroUsuario, StringComparison.OrdinalIgnoreCase));
        }

        produtos = sortColumn switch
        {
            "DataCadastro" => sortDirection == "asc"
                ? produtos.OrderBy(p => p.DataCadastro)
                : produtos.OrderByDescending(p => p.DataCadastro),
            "UsuarioCadastro" => sortDirection == "asc"
                ? produtos.OrderBy(p => p.UsuarioCadastro)
                : produtos.OrderByDescending(p => p.UsuarioCadastro),
            "Valor" => sortDirection == "asc"
                ? produtos.OrderBy(p => p.Valor)
                : produtos.OrderByDescending(p => p.Valor),
            _ => sortDirection == "asc"
                ? produtos.OrderBy(p => p.Id)
                : produtos.OrderByDescending(p => p.Id)
        };

        ViewData["CurrentSort"] = sortColumn;
        ViewData["CurrentDirection"] = sortDirection;
        ViewData["FiltroDescricao"] = filtroDescricao;
        ViewData["FiltroUsuario"] = filtroUsuario;

        return View(produtos);
    }

    // GET /Produtos/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var produto = await _produtoService.ObterPorIdAsync(id);
        if (produto is null) 
            return NotFound();
        return View(produto);
    }

    // GET /Produtos/Create
    public IActionResult Create() => View();

    // POST /Produtos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Descricao,Quantidade,Valor")] Produto produto)
    {
        if (!ModelState.IsValid) return View(produto);

        produto.UsuarioCadastro = User.Identity!.Name!; // preenchido automaticamente pelo login
        produto.DataCadastro = DateTime.Now;

        await _produtoService.CriarAsync(produto);
        TempData["Sucesso"] = "Produto cadastrado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Produtos/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var produto = await _produtoService.ObterPorIdAsync(id);
        if (produto is null) 
            return NotFound();
        return View(produto);
    }

    // POST /Produtos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Quantidade,Valor,UsuarioCadastro,DataCadastro")] Produto produto)
    {
        if (id != produto.Id) 
            return NotFound();
        if (!ModelState.IsValid) 
            return View(produto);

        var atualizado = await _produtoService.AtualizarAsync(id, produto);
        if (!atualizado) 
            return NotFound();

        TempData["Sucesso"] = "Produto atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Produtos/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var produto = await _produtoService.ObterPorIdAsync(id);
        if (produto is null) 
            return NotFound();
        return View(produto);
    }

    // POST /Produtos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _produtoService.RemoverAsync(id);
        TempData["Sucesso"] = "Produto removido com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}