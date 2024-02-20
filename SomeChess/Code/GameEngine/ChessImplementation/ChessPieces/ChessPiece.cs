using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{

    public abstract class ChessPiece
    {
        private ChessMovePattern? _MovePattern = null;
        public ChessMovePattern MovePattern { get => _MovePattern ?? ChessMovePattern.None; set => _MovePattern = value; }

        private Team? _Team = null;
        public Team Team
        {
            get => _Team ?? throw new Exception("Piece should have a team");
            set => _Team = value;
        }

        public void InheritFrom(ChessPiece piece)
        {
            MovePattern = piece.MovePattern;
            Team = piece.Team;
        }
    }
}
