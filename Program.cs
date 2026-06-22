
using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EcommerceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app     = builder.Build();

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

app.MapPost("/produtos", (Produto novoProduto, EcommerceContext banco) =>
{    
        banco.Produtos.Add(novoProduto);

        banco.SaveChanges();

        return "Produto cadastrado com sucesso pela API!";   
});

app.MapPost("/vendas", (Venda novaVenda, EcommerceContext banco) =>
{   
        var produtoDoBanco = banco.Produtos.FirstOrDefault(p => p.Id == novaVenda.ProdutoId);

        if(produtoDoBanco == null)
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
        catch(Exception ex)
        {
            return Results.BadRequest($"Não foi possivel vender: {ex.Message}");
        }    
});

app.Run();