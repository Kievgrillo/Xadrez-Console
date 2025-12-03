using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.branca;
            Terminada = false;
            ColocarPecas();
        }

        public void ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutarMovimento(origem, destino);
            Turno++;
            MudaJogador();
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (Tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (JogadorAtual != Tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!Tab.peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        #region Metodos Privados
        private void MudaJogador()
        {
            JogadorAtual = (JogadorAtual == Cor.branca) ? Cor.preto : Cor.branca;
        }

        private void ColocarPecas()
        {
            Tab.ColocarPeca(new Torre(Tab, Cor.branca), new PosicaoXadrez('c', 1).toPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.branca), new PosicaoXadrez('c', 2).toPosicao());
            Tab.ColocarPeca(new Rei(Tab, Cor.branca), new PosicaoXadrez('b', 3).toPosicao());

            Tab.ColocarPeca(new Torre(Tab, Cor.preto), new PosicaoXadrez('c', 3).toPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.preto), new PosicaoXadrez('c', 7).toPosicao());
            Tab.ColocarPeca(new Rei(Tab, Cor.preto), new PosicaoXadrez('b', 6).toPosicao());
        }
        #endregion
    }
}
