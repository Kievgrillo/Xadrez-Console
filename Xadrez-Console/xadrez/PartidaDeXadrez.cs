using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.branca;
            ColocarPecas();
        }

        public void ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);
        }

        private void ColocarPecas()
        {
            tab.ColocarPeca(new Torre(tab, Cor.branca), new PosicaoXadrez('c', 1).toPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.branca), new PosicaoXadrez('c', 2).toPosicao());
            tab.ColocarPeca(new Cavalo(tab, Cor.branca), new PosicaoXadrez('d', 4).toPosicao());
            tab.ColocarPeca(new Bispo(tab, Cor.branca), new PosicaoXadrez('e', 5).toPosicao());
            tab.ColocarPeca(new Rei(tab, Cor.branca), new PosicaoXadrez('b', 3).toPosicao());
            tab.ColocarPeca(new Dama(tab, Cor.branca), new PosicaoXadrez('h', 1).toPosicao());

            tab.ColocarPeca(new Torre(tab, Cor.preto), new PosicaoXadrez('c', 3).toPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.preto), new PosicaoXadrez('c', 7).toPosicao());
            tab.ColocarPeca(new Cavalo(tab, Cor.preto), new PosicaoXadrez('d', 2).toPosicao());
            tab.ColocarPeca(new Bispo(tab, Cor.preto), new PosicaoXadrez('e', 2).toPosicao());
            tab.ColocarPeca(new Rei(tab, Cor.preto), new PosicaoXadrez('b', 6).toPosicao());
            tab.ColocarPeca(new Dama(tab, Cor.preto), new PosicaoXadrez('h', 4).toPosicao());
        }
    }
}
