﻿namespace SomeChess.Code.GameEngine.ChessImplementation.ChessPieceCollection
{

    public class ChessPiece : IChessPiece
    {
        private ChessMovePattern? _MovePattern = null;
        public ChessMovePattern MovePattern { get => _MovePattern ?? ChessMovePattern.None; set => _MovePattern = value; }

        private Team? _Team = null;
        public Team Team
        {
            get => _Team ?? throw new Exception("Piece should have a team");
            set => _Team = value;
        }
    }
}
