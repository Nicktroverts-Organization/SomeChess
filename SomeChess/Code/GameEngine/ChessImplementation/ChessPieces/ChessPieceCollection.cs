﻿using System.Data;
using System;
using System.Numerics;
using System.Security.Cryptography;
using SomeChess.Code.GameEngine.ChessImplementation;
using SomeChess.Code.MatchMaking;
//ᵇᵃᵇᵃᵇᵒʸᵉ
//ʙᴀʙᴀʙᴏʏᴇ


// todo - Someone check the performance of this code. I don't think i could make it any better. [Nick, 26.02.2024]
// todo - Someone check if this code even works. I don't think i can make it good. [Nick, 04.03.2024]

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class KnightPiece : ChessPiece
    {
        public KnightPiece()
        {
            PieceType = ChessPieceType.Knight;
            Team = Team.White;
        }

        public override bool CanMove(string from, string to)
        {
            //get x and y distances the piece is moving regardless of positive or negative numbers
            int rowDiff = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));
            int colDiff = Convert.ToInt16(MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1])));

            if (!((rowDiff == 1 && colDiff == 2) || (rowDiff == 2 && colDiff == 1))) return false; // Checks for correct move pattern (L)

            if (Chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true; //Checks if moving to empty field

            return Chess.Board.GetPiece(to).Team != Team; //Tries to move on other piece if opponent return true else return false;
        }
    }

    public class BishopPiece : ChessPiece
    {
        public BishopPiece()
        {
            PieceType = ChessPieceType.Bishop;
            Team = Team.White;
        }

        public override bool CanMove(string from, string to)
        {
            if (from == "" || to == "") //given arguments are invalid.
                throw new ArgumentException("\"from\" or \"to\" was empty!");

            //make sure fields are actual chess fields
            Chess.Board.ValidateFields(new[]{ from, to });

            //get the distance piece is going to move and make sure it's diagonal
            int pathLength = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));
            if (pathLength != (int)MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1]))) return false;

            //change multiplier depending on whether or not piece is moving in a positive or negative direction
            int rowChange = (int)Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) > (int)Chess.AlphConversionChars.IndexOf(from.ToLower()[0]) ? 1 : -1;
            int colChange = (int)Char.GetNumericValue(to[1]) > (int)Char.GetNumericValue(from[1]) ? 1 : -1;

            //check if something is in the way
            for (int i = 1; i <= pathLength; i++)
            {
                int file = (Chess.AlphConversionChars.IndexOf(from.ToLower()[0]) + i * rowChange);
                int rank = ((int)Char.GetNumericValue(from[1]) + i * colChange);
                if (Chess.Board.GetPiece($"{Chess.AlphConversionChars[file]}{rank}").PieceType != ChessPieceType.None)
                    return false;
            }


            if (Chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true; //Moves to empty field

            return Chess.Board.GetPiece(to).Team != Team; //Tries to move on other piece if opponent return true else return false;
        }
    }

    public class RookPiece : ChessPiece
    {
        
        public RookPiece()
        {
            PieceType = ChessPieceType.Rook;
            Team = Team.White;
        }

        public override bool CanMove(string from, string to)
        {
            if (from == "" || to == "")
                throw new ArgumentException("\"from\" or \"to\" was empty!");

            //checks if given fields are actual chess fields
            Chess.Board.ValidateFields(new[] { from, to });

            //gets the distance piece is traveling
            int pathLength = 0;

            int HpathLength =
                Convert.ToInt16(MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1])));

            int VpathLength = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) -
                                                    Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));

            if (from[0] == to[0]) // Moving horizontally
                pathLength = HpathLength;
            else if (from[1] == to[1]) // Moving vertically
                pathLength = VpathLength;

            if (HpathLength != 0 && VpathLength != 0) return false;

            //check if other piece is in the way
            for (int i = 1; i < pathLength; i++)
            {
                if (from[0] == to[0]) // Moving horizontally
                {
                    char row = from[0];
                    int col = (((int)Char.GetNumericValue(from[1])) + ((int)Char.GetNumericValue(to[1]) > (int)Char.GetNumericValue(from[1]) ? i : -i));
                    if (Chess.Board.GetPiece($"{row}{col}").PieceType != ChessPieceType.None)
                        return false;
                }
                else if (from[1] == to[1]) // Moving vertically
                {
                    char row = Chess.AlphConversionChars[(Chess.AlphConversionChars.IndexOf(from.ToLower()[0]) + (Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) > Chess.AlphConversionChars.IndexOf(from.ToLower()[0]) ? i : -i))];
                    int col = (int)Char.GetNumericValue(from[1]);
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
        public PawnPiece()
        {
            PieceType = ChessPieceType.Pawn;
            Team = Team.White;
        }

        // this is absolute pain i can't think and my brain is fried even though i just woke up pls kill me.
        public override bool CanMove(string from, string to)
        {
            return PawnCanMove(from, to, Team == Team.White ? 1 : -1);
        }

        private bool PawnCanMove(string from, string to, int direction)
        {
            //No comments for you, Future me! get gud.
            
            if (direction != 1 && direction != -1) return false;
            //if ((int)MathF.Abs(direction) != 1) return false;
            
            Chess.Board.ValidateFields(new[] { from, to });

            //gets the distance piece is traveling
            int colDiff = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));
            int pathLength = Convert.ToInt16(MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1])));

            if (pathLength > (((int)Char.GetNumericValue(from[1]) == 2 || (int)Char.GetNumericValue(from[1]) == 7) ? 2 : 1)) 
                return false;

            int numericDifference = (int)MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1]));

            if (numericDifference is not (1 or 2))
                return false;

            //if (!((int)MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1])) == direction || (int)MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1])) == direction + direction)) return false;


            if (colDiff > 1) return false;
            if (colDiff == 1 && pathLength != 1) return false;

            if (Chess.Board.GetPiece(to).Team != Team && colDiff == 1) return true;
            else if(colDiff == 1 && Chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return false;
            else if (colDiff == 1 && Chess.Board.GetPiece(to).Team == Team) return false;
            if (Chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true;

            return false;
        }
    }

    public class QueenPiece : ChessPiece
    {
        private readonly RookPiece _rook = new();
        private readonly BishopPiece _bishop = new();

        public QueenPiece()
        {
            PieceType = ChessPieceType.Queen;
            Team = Team.White;
        }

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

        public KingPiece()
        {
            PieceType = ChessPieceType.King;
            Team = Team.White;
        }

        public override bool CanMove(string from, string to)
        {
            // Check if the given fields are actual chess fields
            Chess.Board.ValidateFields(new[] { from, to });

            // Check if the move is within one square in any direction
            int rowDiff = Math.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0]));
            int colDiff = Math.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1]));

            if (rowDiff > 1 || colDiff > 1)
                return false;

            // logic to check if the move would result in putting the king in check.
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    string tempField = $"{(char)Chess.AlphConversionChars[i]}{(char)j+1}";

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
        public EmptyPiece()
        {
            PieceType = ChessPieceType.None;
            Team = Team.White;
        }

        public override bool CanMove(string from, string to) => false;
    }
}
