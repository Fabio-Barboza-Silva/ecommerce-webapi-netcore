
using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EcommerceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/produtos", (EcommerceContext banco) =>
{
    var listaDeProdutos = banco.Produtos.ToList();
    return listaDeProdutos;
});

app.MapGet("/produtos/estoque-baixo", (EcommerceContext banco) =>
{
    var produtosCriticos = banco.Produtos
                                .Where(p => p.Estoque < 5)
                                .OrderBy(p => p.Price)
                                .ToList();
    return produtosCriticos;
});

app.MapGet("/produtos/financeiro-critico", (EcommerceContext banco) =>
{
    decimal totalImobilizado = banco.Produtos
                                  .Where(p => p.Estoque < 5)
                                  .Sum(p => p.Price * p.Estoque);
    return $"Valor total imobilizado em estoque critico: R${totalImobilizado:F2}";
});

app.MapGet("/vendas/relatorio", (EcommerceContext banco) =>
{
    var relatorioVendas = banco.Vendas
                               .Include(v => v.Produto)
                               .Include(v => v.Cliente)
                               .ToList();
    return relatorioVendas;
});

app.MapGet("/vendas/relatorio-enxuto", (EcommerceContext banco) =>
{
    var relatorioEnxuto = banco.Vendas
        .Include(v => v.Produto)
        .Include(v => v.Cliente)
        .AsEnumerable() // Para trazer o valor total com dois numeros decimais junto com o Math.Round
        .Select(v => new VendaRelatorioDto
        {
            VendaId = v.Id,
            NomeCliente = v.Cliente.Nome,
            NomeProduto = v.Produto.Name,
            PrecoUnitario = v.Produto.Price,
            Quantidade = v.Quantidade,
            ValorTotal = Math.Round(v.Produto.Price * v.Quantidade, 2),
            Data = v.DataVenda
        })
        .ToList();
    return relatorioEnxuto;
});


app.MapGet("/vendas/relatorio-filtrado", (int? clienteId, int pagina, int tamanhoPagina, EcommerceContext banco) =>
{
    var query = banco.Vendas
                     .Include(v => v.Produto)
                     .Include(v => v.Cliente)
                     .AsQueryable();
    if (clienteId.HasValue)
    {
        query = query.Where(v => v.ClienteId == clienteId.Value);
    }

    int linhaParaPular = (pagina - 1) * tamanhoPagina;

    var resultado = query.Skip(linhaParaPular)
                         .Take(tamanhoPagina)
                         .AsEnumerable()
                         .Select(v => new VendaRelatorioDto
                         {
                             VendaId = v.Id,
                             NomeCliente = v.Cliente.Nome,
                             NomeProduto = v.Produto.Name,
                             PrecoUnitario = v.Produto.Price,
                             Quantidade = v.Quantidade,
                             ValorTotal = Math.Round(v.Produto.Price * v.Quantidade, 2),
                             Data = v.DataVenda
                         })
                         .ToList();
    return resultado;

});

app.MapGet("/vendas/relatorio-avancado", (
    int? clienteId,
    DateTime? dataInicio,
    DateTime? dataFim,
    string? termoBusca,
    int pagina,
    int tamanhoPagina,
    EcommerceContext banco) =>
{
    var query = banco.Vendas
                      .Include(v => v.Produto)
                      .Include(v => v.Cliente)
                      .AsQueryable();

    if (clienteId.HasValue)
    {
        query = query.Where(v => v.ClienteId == clienteId.Value);
    }

    if (dataInicio.HasValue)
    {
        query = query.Where(v => v.DataVenda >= dataInicio.Value);
    }

    if (dataFim.HasValue)
    {
        query = query.Where(v => v.DataVenda <= dataFim.Value);
    }

    if (!string.IsNullOrWhiteSpace(termoBusca))
    {
        query = query.Where(v => v.Produto.Name.ToLower().Contains(termoBusca.ToLower()));
    }

    int linhasParaPular = (pagina - 1) * tamanhoPagina;

    var resultado = query.Skip(linhasParaPular)
                         .Take(tamanhoPagina)
                         .AsEnumerable()
                         .Select(v => new VendaRelatorioDto
                         {
                             VendaId = v.Id,
                             NomeCliente = v.Cliente.Nome,
                             NomeProduto = v.Produto.Name,
                             PrecoUnitario = v.Produto.Price,
                             Quantidade = v.Quantidade,
                             ValorTotal = Math.Round(v.Produto.Price * v.Quantidade, 2),
                             Data = v.DataVenda
                         })
                         .ToList();
    return resultado;

});

app.MapPost("/produtos", (Produto novoProduto, EcommerceContext banco) =>
{
    banco.Produtos.Add(novoProduto);

    banco.SaveChanges();

    return "Produto cadastrado com sucesso pela API!";
});

app.MapPost("/vendas", (Venda novaVenda, EcommerceContext banco) =>
{
    var produtoDoBanco = banco.Produtos.FirstOrDefault(p => p.Id == novaVenda.ProdutoId);

    if (produtoDoBanco == null)
    {
        return Results.BadRequest("Produto não encontrado no sistema!");
    }

    try
    {
        novaVenda.EfetivarVenda(produtoDoBanco, novaVenda.Quantidade);
        novaVenda.ClienteId = 1;

        banco.Vendas.Add(novaVenda);
        banco.SaveChanges();

        return Results.Ok($"Venda realizada! Estoque atualizado do produto: {produtoDoBanco.Estoque}");
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Não foi possivel vender: {ex.Message}");
    }
});

app.MapPut("/produtos/atualizar-estoque", (int produtoId, int novoEstoque, EcommerceContext banco) =>
{
    var produto = banco.Produtos.FirstOrDefault(p => p.Id == produtoId);
    if (produto == null)
    {
        return Results.NotFound("Produto não encontrado!");
    }
    produto.Estoque = novoEstoque;
    banco.SaveChanges();
    return Results.Ok($"Estoque do produto {produto.Name} atualizado para {novoEstoque} com sucesso!");

});

app.Run();