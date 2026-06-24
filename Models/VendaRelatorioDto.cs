namespace ConsoleApp1.Models
{
    public class VendaRelatorioDto
    {
        public int VendaId { get; set; }
        public string NomeCliente { get; set; }
        public string NomeProduto { get; set; }
        public decimal PrecoUnitario  { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime Data { get; set; }

    }
}
