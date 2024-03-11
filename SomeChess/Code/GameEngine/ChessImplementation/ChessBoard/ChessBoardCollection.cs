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
            {new RookPiece(), new PawnPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new PawnPiece(), new RookPiece()},
            {new KnightPiece(), new PawnPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new PawnPiece(), new KnightPiece()},
            {new BishopPiece(), new PawnPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new PawnPiece(), new BishopPiece()},
            {new QueenPiece(), new PawnPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new PawnPiece(), new QueenPiece()},
            {new KingPiece(), new PawnPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new PawnPiece(), new KingPiece()},
            {new BishopPiece(), new PawnPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new PawnPiece(), new BishopPiece()},
            {new KnightPiece(), new PawnPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new PawnPiece(), new KnightPiece()},
            {new RookPiece(), new PawnPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new PawnPiece(), new RookPiece()},
        };
    }
}
