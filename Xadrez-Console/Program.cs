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
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.Terminada)
                {
                    try
                    {
                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.Tab);
                        Console.WriteLine();
                        Console.WriteLine("Turno: " + partida.Turno);
                        Console.WriteLine("Aguardando jogada da: " + partida.JogadorAtual);

                        Console.WriteLine();
                        Console.WriteLine("Origem: ");
                        Posicao origem = Tela.LerPosicaoXadrez().toPosicao();
                        partida.ValidarPosicaoDeOrigem(origem);

                        bool[,] posicoesPossiveis = partida.Tab.peca(origem).MovimentosPossiveis();

                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.Tab, posicoesPossiveis);

                        Console.WriteLine();
                        Console.WriteLine("Destino: ");
                        Posicao destino = Tela.LerPosicaoXadrez().toPosicao();

                        partida.ExecutarMovimento(origem, destino);
                    }
                    catch (TabuleiroException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                }

                Tela.ImprimirTabuleiro(partida.Tab);
            }

            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}