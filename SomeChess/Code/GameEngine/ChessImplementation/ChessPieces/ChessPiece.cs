using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{

    public abstract class ChessPiece
    {
        private ChessPieceType? _MovePattern = null;
        public ChessPieceType PieceType { get => _MovePattern ?? ChessPieceType.None; set => _MovePattern = value; }

        private Team? _Team = null;
        public Team Team
        {
            get => _Team ?? throw new Exception("Piece should have a team");
            set => _Team = value;
        }

        public void InheritFrom(ChessPiece piece)
        {
            PieceType = piece.PieceType;
            Team = piece.Team;
        }

        public abstract bool CanMove(string from, string to);
    }
}
