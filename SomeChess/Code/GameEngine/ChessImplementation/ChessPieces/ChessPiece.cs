using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{

    public abstract class ChessPiece
    {
        public ChessPieceType PieceType;

        public Team Team;

        public void InheritFrom(ChessPiece piece)
        {
            PieceType = piece.PieceType;
            Team = piece.Team;
        }

        public abstract bool CanMove(string from, string to);
    }
}
