using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{

    public abstract class ChessPiece : ICloneable
    {
        public ChessPieceType PieceType;

        public Team Team;

        public string Field;

        protected ChessPiece(Team team, string field)
        {
            Team = team;
            Field = field;
        }

        public abstract bool CanMove(string from, string to, Chess chess);
        public abstract object Clone();
    }
}
