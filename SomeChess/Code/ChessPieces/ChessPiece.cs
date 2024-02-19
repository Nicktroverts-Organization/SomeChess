using SomeChess.Code;
using SomeChess.Code.Enums;
using SomeChess.Code.Interfaces;
using SomeChess.Code.ChessPieces;

namespace SomeChess.Code.ChessPieces
{

    public class ChessPiece : IChessPiece
    {
        private ChessMovePattern? _MovePattern = null;
        public ChessMovePattern MovePattern { get => _MovePattern ?? ChessMovePattern.None; set => _MovePattern = value; }

        public ChessPiece()
        {

        }
    }
}
