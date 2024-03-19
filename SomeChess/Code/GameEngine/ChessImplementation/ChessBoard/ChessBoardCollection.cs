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
        public static ChessPiece[,] Default = new ChessPiece[8, 8]
        {
            {new RookPiece(Team.White), new PawnPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.Black), new EmptyPiece(Team.Black), new PawnPiece(Team.Black), new RookPiece(Team.Black)},
            {new KnightPiece(Team.White), new PawnPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.Black), new EmptyPiece(Team.Black), new PawnPiece(Team.Black), new KnightPiece(Team.Black)},
            {new BishopPiece(Team.White), new PawnPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.Black), new EmptyPiece(Team.Black), new PawnPiece(Team.Black), new BishopPiece(Team.Black)},
            {new QueenPiece(Team.White), new PawnPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.Black), new EmptyPiece(Team.Black), new PawnPiece(Team.Black), new QueenPiece(Team.Black)},
            {new KingPiece(Team.White), new PawnPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.Black), new EmptyPiece(Team.Black), new PawnPiece(Team.Black), new KingPiece(Team.Black)},
            {new BishopPiece(Team.White), new PawnPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.Black), new EmptyPiece(Team.Black), new PawnPiece(Team.Black), new BishopPiece(Team.Black)},
            {new KnightPiece(Team.White), new PawnPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.Black), new EmptyPiece(Team.Black), new PawnPiece(Team.Black), new KnightPiece(Team.Black)},
            {new RookPiece(Team.White), new PawnPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.White), new EmptyPiece(Team.Black), new EmptyPiece(Team.Black), new PawnPiece(Team.Black), new RookPiece(Team.Black)},
        };

        public static ChessPiece[,] CloneDefault()
        {
            ChessPiece[,] clone = new ChessPiece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    clone[i, j] = (ChessPiece)Default[i, j].Clone();
                }
            }

            return clone;
        }
    }
}
