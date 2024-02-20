using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public static class Boards
    {

        /* T P L K K L P T
         * B B B B B B B B
         * _ _ _ _ _ _ _ _
         * _ _ _ _ _ _ _ _
         * _ _ _ _ _ _ _ _
         * _ _ _ _ _ _ _ _
         * B B B B B B B B
         * T P L K K L P T
         */
        public static ChessPiece[,] DefaultBoard = new ChessPiece[8, 8]
        {
            {new RookPiece(), new KnightPiece(), new BishopPiece(), new QueenPiece(), new KingPiece(), new BishopPiece(), new KnightPiece(), new RookPiece()},
            {new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece()},
            {new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            {new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            {new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            {new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()},
            {new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece(), new PawnPiece()},
            {new RookPiece(), new KnightPiece(), new BishopPiece(), new KingPiece(), new QueenPiece(), new BishopPiece(), new KnightPiece(), new RookPiece()},
        };
    }
}
