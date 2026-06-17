namespace ConsoleApp1.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Estoque { get; set; }        


        public void AtualizarPreco(decimal novoPreco)
        {
            if(novoPreco <= 0 )
            {
                throw new Exception("Preço invalido");
            }
            else
            {
                Price = novoPreco;
            }
        }
    }
}
