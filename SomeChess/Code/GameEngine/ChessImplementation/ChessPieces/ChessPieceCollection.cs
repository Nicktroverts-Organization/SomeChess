using System.Numerics;
using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class KnightPiece : ChessPiece
    {
        public ChessMovePattern MovePattern = ChessMovePattern.Knight;
        public Team Team = Team.White;
    }

    public class BishopPiece : ChessPiece
    {
        public ChessMovePattern MovePattern = ChessMovePattern.Bishop;
        public Team Team = Team.White;
    }

    public class RookPiece : ChessPiece
    {
        public ChessMovePattern MovePattern = ChessMovePattern.Rook;
        public Team Team = Team.White;
    }
    public class PawnPiece : ChessPiece
    {
        public ChessMovePattern MovePattern = ChessMovePattern.Pawn;
        public Team Team = Team.White;
    }

    public class QueenPiece : ChessPiece
    {
        public ChessMovePattern MovePattern = ChessMovePattern.Queen;
        public Team Team = Team.White;
    }

    public class KingPiece : ChessPiece
    {
        public ChessMovePattern MovePattern = ChessMovePattern.King;
        public Team Team = Team.White;
    }

    public class EmptyPiece : ChessPiece
    {
        public ChessMovePattern MovePattern = ChessMovePattern.None;
        public Team Team = Team.White;
    }
}
