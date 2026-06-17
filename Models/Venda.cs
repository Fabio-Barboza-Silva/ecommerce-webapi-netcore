using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }

        public DateTime DataVenda { get; set; }

        public void EfetivarVenda(Produto produto, int quantidadeDesejada)
        {
            if (produto.Estoque < quantidadeDesejada)
            {
                throw new Exception($"Estoque insuficiente para realizar a venda, pois só temos {produto.Estoque} em estoque");             
            }

            produto.Estoque = produto.Estoque - quantidadeDesejada;

            ProdutoId = produto.Id;
            Quantidade = quantidadeDesejada;
            DataVenda = DateTime.Now;
        }
    }
}
