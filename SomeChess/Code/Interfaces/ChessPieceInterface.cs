using SomeChess.Code.Enums;
using SomeChess.Code;

namespace SomeChess.Code.Interfaces
{
    public interface IChessPiece
    {
        public ChessMovePattern MovePattern { get; set; }
    }
}
