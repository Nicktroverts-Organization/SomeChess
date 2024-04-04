using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public static class Boards
    {

        /* T P L K Q L P T
         * B B B B B B B B
         * _ _ _ _ _ _ _ _
         * _ _ _ _ _ _ _ _
         * _ _ _ _ _ _ _ _
         * _ _ _ _ _ _ _ _
         * B B B B B B B B
         * T P L K Q L P T
         */
        private static readonly ChessPiece[,] _Default = new ChessPiece[8, 8]
        {
            {new RookPiece(Team.White, "a1"), new PawnPiece(Team.White, "a2"), new EmptyPiece(Team.White, "a3"), new EmptyPiece(Team.White, "a4"), new EmptyPiece(Team.Black, "a5"), new EmptyPiece(Team.Black, "a6"), new PawnPiece(Team.Black, "a7"), new RookPiece(Team.Black, "a8")},
            {new KnightPiece(Team.White, "b1"), new PawnPiece(Team.White, "b2"), new EmptyPiece(Team.White, "b3"), new EmptyPiece(Team.White, "b4"), new EmptyPiece(Team.Black, "b5"), new EmptyPiece(Team.Black, "b6"), new PawnPiece(Team.Black, "b7"), new KnightPiece(Team.Black, "b8")},
            {new BishopPiece(Team.White, "c1"), new PawnPiece(Team.White, "c2"), new EmptyPiece(Team.White, "c3"), new EmptyPiece(Team.White, "c4"), new EmptyPiece(Team.Black, "c5"), new EmptyPiece(Team.Black, "c6"), new PawnPiece(Team.Black, "c7"), new BishopPiece(Team.Black, "c8")},
            {new QueenPiece(Team.White, "d1"), new PawnPiece(Team.White, "d2"), new EmptyPiece(Team.White, "d3"), new EmptyPiece(Team.White, "d4"), new EmptyPiece(Team.Black, "d5"), new EmptyPiece(Team.Black, "d6"), new PawnPiece(Team.Black, "d7"), new QueenPiece(Team.Black, "d8")},
            {new KingPiece(Team.White, "e1"), new PawnPiece(Team.White, "e2"), new EmptyPiece(Team.White, "e3"), new EmptyPiece(Team.White, "e4"), new EmptyPiece(Team.Black, "e5"), new EmptyPiece(Team.Black, "e6"), new PawnPiece(Team.Black, "e7"), new KingPiece(Team.Black, "e8")},
            {new BishopPiece(Team.White, "f1"), new PawnPiece(Team.White, "f2"), new EmptyPiece(Team.White, "f3"), new EmptyPiece(Team.White, "f4"), new EmptyPiece(Team.Black, "f5"), new EmptyPiece(Team.Black, "f6"), new PawnPiece(Team.Black, "f7"), new BishopPiece(Team.Black, "f8")},
            {new KnightPiece(Team.White, "g1"), new PawnPiece(Team.White, "g2"), new EmptyPiece(Team.White, "g3"), new EmptyPiece(Team.White, "g4"), new EmptyPiece(Team.Black, "g5"), new EmptyPiece(Team.Black, "g6"), new PawnPiece(Team.Black, "g7"), new KnightPiece(Team.Black, "g8")},
            {new RookPiece(Team.White, "h1"), new PawnPiece(Team.White, "h2"), new EmptyPiece(Team.White, "h3"), new EmptyPiece(Team.White, "h4"), new EmptyPiece(Team.Black, "h5"), new EmptyPiece(Team.Black, "h6"), new PawnPiece(Team.Black, "h7"), new RookPiece(Team.Black, "h8")},
        };

        public static ChessPiece[,] Default
        {
            get => CloneDefault();
            set => throw new InvalidOperationException();
        }

        public static ChessPiece[,] CloneDefault()
        {
            ChessPiece[,] clone = new ChessPiece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    clone[i, j] = (ChessPiece)_Default[i, j].Clone();
                }
            }

            return clone;
        }
    }
}
