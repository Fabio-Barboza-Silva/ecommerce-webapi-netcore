
using ConsoleApp1.Data;
using ConsoleApp1.Models;

var builder = WebApplication.CreateBuilder(args);
var app     = builder.Build();

app.MapGet("/produtos", () =>
{
    using (EcommerceContext banco = new EcommerceContext())
    {
        var listaDeProdutos = banco.Produtos.ToList();
        return listaDeProdutos;
    }
});

app.MapGet("/produtos/estoque-baixo", () =>
{
    using (EcommerceContext banco = new EcommerceContext())
    {
        var produtosCriticos = banco.Produtos
                                    .Where(p => p.Estoque < 5)           
                                    .OrderBy(p => p.Price)
                                    .ToList();
        return produtosCriticos;
    }
});

app.MapGet("/produtos/financeiro-critico", () =>
{
    using (EcommerceContext banco =new EcommerceContext())
    {
        decimal totalImobilizado = banco.Produtos
                                        .Where(p => p.Estoque < 5)
                                        .Sum(p => p.Price * p.Estoque);
        return $"Valor total imobilizado em estoque critico: R${totalImobilizado:F2}";
    }                                   
                                         
});

app.MapPost("/produtos", (Produto novoProduto) =>
{
    using (EcommerceContext banco = new EcommerceContext())
    {
        banco.Produtos.Add(novoProduto);

        banco.SaveChanges();

        return "Produto cadastrado com sucesso pela API!";
    }
});

app.MapPost("/vendas", (Venda novaVenda) =>
{
    using (EcommerceContext banco = new EcommerceContext())
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
    }
});

app.Run();


//Outro sistema
//using ConsoleApp1.Data;
//using ConsoleApp1.Models;

//Console.WriteLine("=== SISTEMA DE VENDAS COM BAIXA DE ESTOQUE ===");

//// 1. Abrimos a conexão com a nossa ponte
//using (EcommerceContext banco = new EcommerceContext())
//{
//    //Venda vendaDoBanco = banco.Vendas.FirstOrDefault();

//    Produto produtoDoBanco = banco.Produtos.FirstOrDefault();

//    if (produtoDoBanco != null)
//    {       
//        Console.WriteLine($"\n Produto localizado:{produtoDoBanco.Name}");
//        Console.WriteLine($"Estoque atual do banco:{produtoDoBanco.Estoque}");

//        Venda novaVenda = new Venda();
//        novaVenda.ClienteId = 1;

//        try
//        {
//            Console.WriteLine("\n Tentando vender 50 unidades...");

//            novaVenda.EfetivarVenda(produtoDoBanco, 50);

//            banco.Vendas.Add(novaVenda);

//            banco.SaveChanges();

//            Console.WriteLine("Venda realizada com sucesso!");
//            Console.WriteLine($"Novo estoque do produto apos a baixa:{produtoDoBanco.Estoque}");
//        }
//        catch(Exception ex)
//        {
//            Console.WriteLine($"Erro na venda:{ex.Message}");           
//        }
//    }    
//}

//Console.WriteLine("\n=== FIM DO PROCESSO.===");