using tabuleiro;
using xadrez;
using System;

namespace Xadrez_Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.colocarPeca(new Torre(tab, Cor.preto), new Posicao(0, 0));
                tab.colocarPeca(new Torre(tab, Cor.preto), new Posicao(1, 3));
                tab.colocarPeca(new Rei(tab, Cor.preto), new Posicao(2, 4));

                tab.colocarPeca(new Rei(tab, Cor.branca), new Posicao(3, 6));

                Tela.ImprimirTabuleiro(tab);
            }

            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}