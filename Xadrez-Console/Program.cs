using System;
using System.Threading.Tasks;
using tabuleiro;

namespace Xadrez_Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Tabuleiro tab = new Tabuleiro(8, 8);

            Tela.ImprimirTabuleiro(tab);

            Console.ReadLine();
        }
    }
}