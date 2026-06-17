using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get;set; }
        public string Cpf { get; set; }

        public void AtualizarCpf(string novoCpf)
        {
            if (novoCpf.Length != 11)
            {
                throw new Exception("CPF invalido");
            }
            else
            {
                Cpf = novoCpf;
            }
        }

    }
}
