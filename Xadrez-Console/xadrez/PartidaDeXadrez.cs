using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public bool xeque { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.branca;
            Terminada = false;
            xeque = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutarMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(p, origem);
        }


        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutarMovimento(origem, destino);

            if (EstarEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }
            if (EstarEmXeque(Adversario(JogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (TesteXequeMate(Adversario(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }
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

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            Pecas.Add(peca);
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        public bool EstarEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Peca x in PecasEmJogo(Adversario(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EstarEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < Tab.linhas; i++)
                {
                    for (int j = 0; j < Tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutarMovimento(origem, destino);
                            bool testeXeque = EstarEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        #region Metodos Privados
        private void MudaJogador()
        {
            JogadorAtual = (JogadorAtual == Cor.branca) ? Cor.preto : Cor.branca;
        }

        private Cor Adversario(Cor cor)
        {
            return (cor == Cor.branca) ? Cor.preto : Cor.branca;
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        private void ColocarPecas()
        {
            colocarNovaPeca('a', 1, new Torre(Tab, Cor.branca));
            colocarNovaPeca('b', 1, new Cavalo(Tab, Cor.branca));
            colocarNovaPeca('c', 1, new Bispo(Tab, Cor.branca));
            colocarNovaPeca('d', 1, new Dama(Tab, Cor.branca));
            colocarNovaPeca('e', 1, new Rei(Tab, Cor.branca));
            colocarNovaPeca('f', 1, new Bispo(Tab, Cor.branca));
            colocarNovaPeca('g', 1, new Cavalo(Tab, Cor.branca));
            colocarNovaPeca('h', 1, new Torre(Tab, Cor.branca));
            colocarNovaPeca('a', 2, new Peao(Tab, Cor.branca));
            colocarNovaPeca('b', 2, new Peao(Tab, Cor.branca));
            colocarNovaPeca('c', 2, new Peao(Tab, Cor.branca));
            colocarNovaPeca('d', 2, new Peao(Tab, Cor.branca));
            colocarNovaPeca('e', 2, new Peao(Tab, Cor.branca));
            colocarNovaPeca('f', 2, new Peao(Tab, Cor.branca));
            colocarNovaPeca('g', 2, new Peao(Tab, Cor.branca));
            colocarNovaPeca('h', 2, new Peao(Tab, Cor.branca));


            colocarNovaPeca('a', 8, new Torre(Tab, Cor.preto));
            colocarNovaPeca('b', 8, new Cavalo(Tab, Cor.preto));
            colocarNovaPeca('c', 8, new Bispo(Tab, Cor.preto));
            colocarNovaPeca('d', 8, new Dama(Tab, Cor.preto));
            colocarNovaPeca('e', 8, new Rei(Tab, Cor.preto));
            colocarNovaPeca('f', 8, new Bispo(Tab, Cor.preto));
            colocarNovaPeca('g', 8, new Cavalo(Tab, Cor.preto));
            colocarNovaPeca('h', 8, new Torre(Tab, Cor.preto));
            colocarNovaPeca('a', 7, new Peao(Tab, Cor.preto));
            colocarNovaPeca('b', 7, new Peao(Tab, Cor.preto));
            colocarNovaPeca('c', 7, new Peao(Tab, Cor.preto));
            colocarNovaPeca('d', 7, new Peao(Tab, Cor.preto));
            colocarNovaPeca('e', 7, new Peao(Tab, Cor.preto));
            colocarNovaPeca('f', 7, new Peao(Tab, Cor.preto));
            colocarNovaPeca('g', 7, new Peao(Tab, Cor.preto));
            colocarNovaPeca('h', 7, new Peao(Tab, Cor.preto));
        }
        #endregion
    }
}
