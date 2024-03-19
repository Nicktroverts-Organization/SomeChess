using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{

    public abstract class ChessPiece : ICloneable
    {
        public ChessPieceType PieceType;

        public Team Team;

        protected ChessPiece(Team team)
        {
            Team = team;
        }

        public abstract bool CanMove(string from, string to, Chess chess);
        public abstract object Clone();
    }
}
