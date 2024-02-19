using SomeChess.Code.Enums;
using SomeChess.Code.Interfaces;
using SomeChess.Code.ChessPieces;

namespace SomeChess.Code.ChessPieces
{

    public static class ChessPieces
    {
        public static ChessPiece KnightPiece = new ChessPiece()
        {
            MovePattern = ChessMovePattern.Knight,

        };

        public static ChessPiece BishopPiece = new ChessPiece()
        {
            MovePattern = ChessMovePattern.Bishop,

        };

        public static ChessPiece RookPiece = new ChessPiece()
        {
            MovePattern = ChessMovePattern.Rook,

        };

        public static ChessPiece PawnPiece = new ChessPiece()
        {
            MovePattern = ChessMovePattern.Pawn,

        };

        public static ChessPiece QueenPiece = new ChessPiece()
        {
            MovePattern = ChessMovePattern.Queen,

        };

        public static ChessPiece KingPiece = new ChessPiece()
        {
            MovePattern = ChessMovePattern.King,

        };

    }
}
