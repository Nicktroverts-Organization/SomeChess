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

    public static class ChessPieceUtils
    {
        public static ChessPiece NewChessPieceByType(Team team, string field, ChessPieceType type)
        {
            switch (type)
            {
                case ChessPieceType.Pawn:
                    return new PawnPiece(team, field);
                case ChessPieceType.Knight:
                    return new KnightPiece(team, field);
                case ChessPieceType.Rook:
                    return new RookPiece(team, field);
                case ChessPieceType.Queen:
                    return new QueenPiece(team, field);
                case ChessPieceType.King:
                    return new KingPiece(team, field);
                case ChessPieceType.Bishop:
                    return new BishopPiece(team, field);
                case ChessPieceType.None:
                    return new EmptyPiece(team, field);
            }

            throw new ArgumentNullException();
        }
    }
    public class KnightPiece : ChessPiece
    {
        public KnightPiece(Team team, string field) : base(team, field)
        {
            PieceType = ChessPieceType.Knight;
            Team = team;
        }

        public override bool CanMove(string from, string to, Chess chess)
        {
            //get x and y distances the piece is moving regardless of positive or negative numbers
            int rowDiff = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));
            int colDiff = Convert.ToInt16(MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1])));

            if (!((rowDiff == 1 && colDiff == 2) || (rowDiff == 2 && colDiff == 1))) return false; // Checks for correct move pattern (L)

            if (chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true; //Checks if moving to empty field

            return chess.Board.GetPiece(to).Team != Team; //Tries to move on other piece if opponent return true else return false;
        }

        public override object Clone()
        {
            var clone = new KnightPiece(Team, Field);
            clone.PieceType = PieceType;
            return clone;
        }
    }

    public class BishopPiece : ChessPiece
    {
        public BishopPiece(Team team, string field) : base(team, field)
        {
            PieceType = ChessPieceType.Bishop;
            Team = team;
            Field = field;
        }

        public override bool CanMove(string from, string to, Chess chess)
        {
            if (from == "" || to == "") //given arguments are invalid.
                throw new ArgumentException("\"from\" or \"to\" was empty!");

            //make sure fields are actual chess fields
            chess.Board.ValidateFields(new[] { from, to });

            //get the distance piece is going to move and make sure it's diagonal
            int pathLength = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));
            if (pathLength != (int)MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1]))) return false;

            //change multiplier depending on whether or not piece is moving in a positive or negative direction
            int rowChange = (int)Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) > (int)Chess.AlphConversionChars.IndexOf(from.ToLower()[0]) ? 1 : -1;
            int colChange = (int)Char.GetNumericValue(to[1]) > (int)Char.GetNumericValue(from[1]) ? 1 : -1;

            //check if something is in the way
            for (int i = 1; i < pathLength; i++)
            {
                int file = (Chess.AlphConversionChars.IndexOf(from.ToLower()[0]) + i * rowChange);
                int rank = ((int)Char.GetNumericValue(from[1]) + i * colChange);
                if (chess.Board.GetPiece($"{Chess.AlphConversionChars[file]}{rank}").PieceType != ChessPieceType.None)
                    return false;
            }


            if (chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true; //Moves to empty field

            return chess.Board.GetPiece(to).Team != Team; //Tries to move on other piece if opponent return true else return false;
        }

        public override object Clone()
        {
            var clone = new BishopPiece(Team, Field);
            clone.PieceType = PieceType;
            return clone;
        }
    }

    public class RookPiece : ChessPiece
    {

        public RookPiece(Team team, string field) : base(team, field)
        {
            PieceType = ChessPieceType.Rook;
            Team = team;
            Field = field;
        }

        public override bool CanMove(string from, string to, Chess chess)
        {
            if (from == "" || to == "")
                throw new ArgumentException("\"from\" or \"to\" was empty!");

            //checks if given fields are actual chess fields
            chess.Board.ValidateFields(new[] { from, to });

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
                    if (chess.Board.GetPiece($"{row}{col}").PieceType != ChessPieceType.None)
                        return false;
                }
                else if (from[1] == to[1]) // Moving vertically
                {
                    char row = Chess.AlphConversionChars[(Chess.AlphConversionChars.IndexOf(from.ToLower()[0]) + (Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) > Chess.AlphConversionChars.IndexOf(from.ToLower()[0]) ? i : -i))];
                    int col = (int)Char.GetNumericValue(from[1]);
                    if (chess.Board.GetPiece($"{row}{col}").PieceType != ChessPieceType.None)
                        return false;
                }
            }

            if (chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true; //If field is empty move there

            return chess.Board.GetPiece(to).Team != Team; //If opponent on field move there else don't move there
        }

        public override object Clone()
        {
            var clone = new RookPiece(Team, Field);
            clone.PieceType = PieceType;
            return clone;
        }
    }

    public class PawnPiece : ChessPiece
    {
        public PawnPiece(Team team, string field) : base(team, field)
        {
            PieceType = ChessPieceType.Pawn;
            Team = team;
            Field = field;
        }

        // this is absolute pain i can't think and my brain is fried even though i just woke up pls kill me.
        public override bool CanMove(string from, string to, Chess chess)
        {
            return PawnCanMove(from, to, Team == Team.White ? 1 : -1, chess);
        }

        private bool PawnCanMove(string from, string to, int direction, Chess chess)
        {
            //No comments for you, Future me! get gud.

            if (direction != 1 && direction != -1) return false;
            //if ((int)MathF.Abs(direction) != 1) return false;

            chess.Board.ValidateFields(new[] { from, to });

            //gets the distance piece is traveling
            int colDiff = Convert.ToInt16(MathF.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0])));
            int pathLength = Convert.ToInt16(MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1])));

            //check for correct direction
            if ((int)Char.GetNumericValue(to[1]) > (int)Char.GetNumericValue(from[1])) {
                if (direction == -1) 
                    return false; }
            else
                if (direction == 1)
                    return false;


            if (pathLength > (((int)Char.GetNumericValue(from[1]) == 2 || (int)Char.GetNumericValue(from[1]) == 7) ? 2 : 1))
                return false;

            int numericDifference = (int)MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1]));

            if (numericDifference is not (1 or 2))
                return false;

            //if (!((int)MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1])) == direction || (int)MathF.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1])) == direction + direction)) return false;

            //check for other pieces in the way
            for (int i = 1; i < pathLength; i++)
            {
                char row = from[0];
                int col = (((int)Char.GetNumericValue(from[1])) + ((int)Char.GetNumericValue(to[1]) > (int)Char.GetNumericValue(from[1]) ? i : -i));
                if (chess.Board.GetPiece($"{row}{col}").PieceType != ChessPieceType.None)
                    return false;
            }


            if (colDiff > 1) return false;
            if (colDiff == 1 && pathLength != 1) return false;

            if (chess.Board.GetPiece(to).Team != Team && colDiff == 1 && chess.Board.GetPiece(to).PieceType != ChessPieceType.None)
            {
                if (Team == Team.White)
                    chess.FieldsWhiteCanMoveTo.Add(to);
                else
                    chess.FieldsBlackCanMoveTo.Add(to);

                return true;
            }
            else if (colDiff == 1 && chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return false;
            else if (colDiff == 1 && chess.Board.GetPiece(to).Team == Team) return false;
            if (chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true;

            return false;
        }

        public override object Clone()
        {
            var clone = new PawnPiece(Team, Field);
            clone.PieceType = PieceType;
            return clone;
        }
    }

    public class QueenPiece : ChessPiece
    {
        private readonly RookPiece _rook;
        private readonly BishopPiece _bishop;

        public QueenPiece(Team team, string field) : base(team, field)
        {
            PieceType = ChessPieceType.Queen;
            Team = team;
            Field = field;
            _rook = new RookPiece(team, Field);
            _bishop = new BishopPiece(team, Field);
        }

        public override bool CanMove(string from, string to, Chess chess)
        {
            //Check if the given fields are actual chess fields
            chess.Board.ValidateFields(new[] { from, to });

            //Check if one of both pieces can move if yes return true else return false
            if (_rook.CanMove(from, to, chess.GetGame()) || _bishop.CanMove(from, to, chess.GetGame())) return true;
            return false;
        }

        public override object Clone()
        {
            var clone = new QueenPiece(Team, Field);
            clone.PieceType = PieceType;
            return clone;
        }
    }

    public class KingPiece : ChessPiece
    {
        public KingPiece(Team team, string field) : base(team, field)
        {
            PieceType = ChessPieceType.King;
            Team = team;
            Field = field;
        }

        public override bool CanMove(string from, string to, Chess chess)
        {
            // Check if the given fields are actual chess fields
            chess.Board.ValidateFields(new[] { from, to });

            if (Team == Team.White)
            {
                if (chess.FieldsBlackCanMoveTo.Contains(to))
                    return false;
                if (chess.Original && chess.BlackKing.Field != "")
                {
                    Chess checkers = (Chess)chess.Clone();
                    checkers.Original = false;
                    chess.OriginalChess.Clones.Add(checkers);
                    checkers.UpdateGameState();
                    if (checkers.MovePiece(chess.BlackKing.Field, to))
                    {
                        chess.OriginalChess.Clones.Remove(checkers);
                        chess.FieldsBlackCanMoveTo.Add(to);
                        return false;
                    }
                    try
                    {
                        chess.OriginalChess.Clones.RemoveAt(0);
                    }
                    catch
                    {
                        if (chess.OriginalChess.Clones.Count != 0)
                            chess.OriginalChess.Clones.RemoveAt(0);
                    }
                }
            }
            else
            {
                if (chess.FieldsWhiteCanMoveTo.Contains(to))
                    return false;
                if (chess.Original && chess.WhiteKing.Field != "")
                {
                    Chess checkers = (Chess)chess.Clone();
                    checkers.Original = false;
                    chess.OriginalChess.Clones.Add(checkers);
                    checkers.UpdateGameState();
                    if (checkers.MovePiece(chess.WhiteKing.Field, to))
                    {
                        chess.OriginalChess.Clones.Remove(checkers);
                        chess.FieldsWhiteCanMoveTo.Add(to);
                        return false;
                    }
                    try
                    {
                        chess.OriginalChess.Clones.RemoveAt(0);
                    }
                    catch
                    {
                        if (chess.OriginalChess.Clones.Count != 0)
                            chess.OriginalChess.Clones.RemoveAt(0);
                    }
                }
            }

            // Check if the move is within one square in any direction
            int rowDiff = Math.Abs(Chess.AlphConversionChars.IndexOf(to.ToLower()[0]) - Chess.AlphConversionChars.IndexOf(from.ToLower()[0]));
            int colDiff = Math.Abs((int)Char.GetNumericValue(to[1]) - (int)Char.GetNumericValue(from[1]));

            if (rowDiff > 1 || colDiff > 1)
                return false;

            //// logic to check if the move would result in putting the king in check.
            //for (var i = 0; i < 8; i++)
            //{
            //    for (var j = 0; j < 8; j++)
            //    {
            //        string tempField = $"{(char)Chess.AlphConversionChars[i]}{(char)j + 1}";

            //        if (chess.Board.GetPiece(tempField).Team == Team) continue;

            //        if (chess.Board.GetPiece(tempField).CanMove(tempField, to, chess.GetGame())) return false;
            //    }
            //}

            if (chess.Board.GetPiece(to).PieceType == ChessPieceType.None) return true;

            return chess.Board.GetPiece(to).Team != Team;
        }

        public override object Clone()
        {
            var clone = new KingPiece(Team, Field);
            clone.PieceType = PieceType;
            return clone;
        }
    }

    public class EmptyPiece : ChessPiece
    {
        public EmptyPiece(Team team, string field) : base(team, field)
        {
            PieceType = ChessPieceType.None;
            Team = team;
            Field = field;
        }

        public override bool CanMove(string from, string to, Chess chess) => false;

        public override object Clone()
        {
            var clone = new EmptyPiece(Team, Field);
            clone.PieceType = PieceType;
            return clone;
        }
    }
}
