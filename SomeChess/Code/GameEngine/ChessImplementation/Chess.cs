﻿//C# is fucking trash get some good existing, why cant i define some random word to be using i hate tis ;-;
//ispolzovat KakietoSchachmaty.Kod.IgrovoiDvighok;

using System.Security.Cryptography.X509Certificates;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public record Chess : IGame<Chess>
    {
        public static ChessBoard Board = new();

        public static List<char> AlphConversionChars = new() 
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };


        public ChessState State = ChessState.None;

        public Team TeamTurn = Team.White;

        public List<ChessPiece> WhitePieces = new();
        public List<ChessPiece> BlackPieces = new();

        public Chess() => ResetBoard();

        public void StartGame()
        {
            ResetBoard();
            State = ChessState.Playing;
        }

        public void StopGame()
        {
            State = ChessState.None;
        }

        public void ResetBoard()
        {
            Board.SetBoardToDefault();
            TeamTurn = Team.White;
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    WhitePieces.Add(Board.GetPiece($"{AlphConversionChars[i]}{j+1}"));
                }
                for (var j = 7; j > 5; j--)
                {
                    BlackPieces.Add(Board.GetPiece($"{AlphConversionChars[i]}{j+1}"));
                }
            }
        }

        /// <summary>
        /// <para>End current players turn.</para>
        /// </summary>
        public void EndTurn()
        {
            TeamTurn = TeamTurn == Team.White ? Team.Black : Team.White;
            UpdateGameState();
        }

        public void UpdateGameState()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            Console.WriteLine($"{AlphConversionChars[i]}{j + 1} -> {AlphConversionChars[x]}{y + 1} : {Board.GetPiece($"{AlphConversionChars[i]}{j + 1}")
                                .CanMove($"{AlphConversionChars[i]}{j + 1}", $"{AlphConversionChars[x]}{y + 1}")}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// <para>Moves the piece from field <paramref name="From"/> to the field <paramref name="To"/>.</para>
        /// </summary>
        /// <param name="From">The field of the piece to move.</param>
        /// <param name="To">The field to move the piece to.</param>
        /// <returns>Returns whether or not it was successfully moved</returns>
        public bool MovePiece(string From, string To)
        {
            //Check that game is running
            if (State == ChessState.None)
                throw new InvalidOperationException("Can't move piece while game is not running!");

            //Check if player is trying to move opponents piece.
            if (Board.GetPiece(From).Team != TeamTurn)
                return false;

            //Check if the given parameters are valid chess fields.
            Board.ValidateFields(new[] { From, To });

            if (!Board.GetPiece(From).CanMove(From, To)) return false; //If Piece can't move to field "To", return false.

            //Moves the piece to the new position
            Board.SetPiece(To, Board.GetPiece(From));
            Board.SetPiece(From, new EmptyPiece(Board.GetPiece(From).Team));
            
            //Successfully moved piece from field "From" to field "To"
            return true;
        }

        public Chess GetGame()
        {
            return this;
        }
    }
}
