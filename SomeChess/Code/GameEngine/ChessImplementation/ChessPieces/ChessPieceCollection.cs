using System.Data;
using System.Numerics;
using System.Security.Cryptography;
using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class KnightPiece : ChessPiece
    {
        public ChessPieceType PieceType = ChessPieceType.Knight;
        public Team Team = Team.White;
        public override bool CanMove(string from, string to)
        {
            //get x and y distances the piece is moving regardless of positive or negative numbers
            int rowDiff = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));
            int colDiff = Convert.ToInt16(MathF.Abs((int)to[1] - (int)from[1]));

            if (!((rowDiff == 1 && colDiff == 2) || (rowDiff == 2 && colDiff == 1))) return false; // Checks for correct move pattern (L)

            if (Chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true; //Checks if moving to empty field

            return Chess.Board.GetPiece(to).Team != Team; //Tries to move on other piece if opponent return true else return false;
        }
    }

    public class BishopPiece : ChessPiece
    {
        public ChessPieceType PieceType = ChessPieceType.Bishop;
        public Team Team = Team.White;

        public override bool CanMove(string from, string to)
        {
            if (from == "" || to == "") //given arguments are invalid.
                throw new ArgumentException("\"from\" or \"to\" was empty!");

            //make sure fields are actual chess fields
            Chess.Board.ValidateFields(new[]{ from, to });

            //get the distance piece is going to move and make sure it's diagonal
            int pathLength = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));
            if (pathLength != MathF.Abs(Convert.ToInt16(to[1]) - Convert.ToInt16(from[1]))) return false;

            //change multiplier depending on whether or not piece is moving in a positive or negative direction
            int rowChange = to[0] > from[0] ? 1 : -1;
            int colChange = to[1] > from[1] ? 1 : -1;

            //check if something is in the way
            for (int i = 1; i < pathLength; i++)
            {
                char file = (char)(from[0] + i * rowChange);
                char rank = (char)(from[1] + i * colChange);
                if (Chess.Board.GetPiece($"{file}{rank}").PieceType != ChessPieceType.None)
                    return false;
            }


            if (Chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true; //Moves to empty field

            return Chess.Board.GetPiece(to).Team != Team; //Tries to move on other piece if opponent return true else return false;
        }
    }

    public class RookPiece : ChessPiece
    {
        public ChessPieceType PieceType = ChessPieceType.Rook;
        public Team Team = Team.White;

        public override bool CanMove(string from, string to)
        {
            if (from == "" || to == "")
                throw new ArgumentException("\"from\" or \"to\" was empty!");

            //checks if given fields are actual chess fields
            Chess.Board.ValidateFields(new[] { from, to });

            //gets the distance piece is traveling
            int pathLength = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));
            
            if (pathLength == MathF.Abs(Convert.ToInt16(to[1]) - Convert.ToInt16(from[1]))) return false; //If Diagonal return false;

            //check if other piece is in the way
            for (int i = 1; i < pathLength; i++)
            {
                if (from[0] == to[0]) // Moving vertically
                {
                    char row = from[0];
                    char col = (char)(from[1] + (to[1] > from[1] ? i : -i));
                    if (Chess.Board.GetPiece($"{row}{col}").PieceType != ChessPieceType.None)
                        return false;
                }
                else if (from[1] == to[1]) // Moving horizontally
                {
                    char row = (char)(from[0] + (to[0] > from[0] ? i : -i));
                    char col = from[1];
                    if (Chess.Board.GetPiece($"{row}{col}").PieceType != ChessPieceType.None)
                        return false;
                }
            }

            if (Chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true; //If field is empty move there

            return Chess.Board.GetPiece(to).Team != Team; //If opponent on field move there else don't move there
        }
    }
    public class PawnPiece : ChessPiece
    {
        public ChessPieceType PieceType = ChessPieceType.Pawn;
        public Team Team = Team.White;

        public override bool CanMove(string from, string to)
        {
            //todo - Remember to do this it is not implemented as of now IMPORTANT!!!
            return false;
        }
    }

    public class QueenPiece : ChessPiece
    {
        private readonly RookPiece _rook = new();
        private readonly BishopPiece _bishop = new();

        public ChessPieceType PieceType = ChessPieceType.Queen;
        public Team Team = Team.White;

        public override bool CanMove(string from, string to)
        {
            //Check if the given fields are actual chess fields
            Chess.Board.ValidateFields(new[] { from, to });

            //Check if one of both pieces can move if yes return true else return false
            return _rook.CanMove(from, to) || _bishop.CanMove(from, to);
        }
    }

    public class KingPiece : ChessPiece
    {
        private readonly QueenPiece _queen = new();

        public ChessPieceType PieceType = ChessPieceType.King;
        public Team Team = Team.White;

        public override bool CanMove(string from, string to)
        {
            // Check if the given fields are actual chess fields
            Chess.Board.ValidateFields(new[] { from, to });

            // Check if the move is within one square in any direction
            int rowDiff = Math.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0]));
            int colDiff = Math.Abs((int)to[1] - (int)from[1]);

            if (rowDiff > 1 || colDiff > 1)
                return false;

            // logic to check if the move would result in putting the king in check.
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    string tempField = $"{(char)Chess.AlphConversionChars[i]}{(char)j}";

                    if (Chess.Board.GetPiece(tempField).Team == Team) continue;

                    if (Chess.Board.GetPiece(tempField).CanMove(tempField, to)) return false;
                }
            }

            if (Chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true;

            return Chess.Board.GetPiece(to).Team != Team;
        }
    }

    public class EmptyPiece : ChessPiece
    {
        public ChessPieceType PieceType = ChessPieceType.None;
        public Team Team = Team.White;

        public override bool CanMove(string from, string to) => false;
    }
}
