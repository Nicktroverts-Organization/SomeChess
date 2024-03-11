//C# is fucking trash get some good existing, why cant i define some random word to be using i hate tis ;-;
//ispolzovat KakietoSchachmaty.Kod.IgrovoiDvighok;

using System.Security.Cryptography.X509Certificates;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public record Chess : IGame<Chess>
    {
        public ChessBoard Board = new();

        public static List<char> AlphConversionChars = new()
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };


        public ChessState State = ChessState.None;

        public Team TeamTurn = Team.White;

        public List<string> FieldsBlackCanMoveTo = new();
        public List<string> FieldsWhiteCanMoveTo = new();

        public bool? WhiteKingCanMove;
        public bool? BlackKingCanMove;

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
            WhiteKingCanMove = false;
            BlackKingCanMove = false;
            string whiteKingField = "";
            string blackKingField = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            var FromPiece = Board.GetPiece($"{AlphConversionChars[i]}{j + 1}");

                            if (FromPiece.Team == Team.White)
                            {
                                if (FromPiece.CanMove($"{AlphConversionChars[i]}{j + 1}", $"{AlphConversionChars[x]}{y + 1}", GetGame()))
                                    FieldsWhiteCanMoveTo.Add($"{AlphConversionChars[x]}{y + 1}");
                            }
                            else if (FromPiece.Team == Team.Black)
                            {
                                if (FromPiece.CanMove($"{AlphConversionChars[i]}{j + 1}", $"{AlphConversionChars[x]}{y + 1}", GetGame()))
                                    FieldsBlackCanMoveTo.Add($"{AlphConversionChars[x]}{y + 1}");
                            }

                            if (FromPiece.PieceType == ChessPieceType.King && FromPiece.CanMove($"{AlphConversionChars[i]}{j + 1}", $"{AlphConversionChars[x]}{y + 1}", GetGame()))
                            {
                                switch (FromPiece.Team)
                                {
                                    case Team.White:
                                        whiteKingField = $"{AlphConversionChars[i]}{j + 1}";
                                        WhiteKingCanMove = true;
                                        break;
                                    case Team.Black:
                                        blackKingField = $"{AlphConversionChars[i]}{j + 1}";
                                        BlackKingCanMove = true;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }

                            Console.WriteLine($"{AlphConversionChars[i]}{j + 1} -> {AlphConversionChars[x]}{y + 1} : {FromPiece
                                .CanMove($"{AlphConversionChars[i]}{j + 1}", $"{AlphConversionChars[x]}{y + 1}", GetGame())}");
                        }
                    }
                }
            }

            if (FieldsWhiteCanMoveTo.Contains(blackKingField) && BlackKingCanMove == false)
                State = ChessState.WhiteWin;
            if (FieldsBlackCanMoveTo.Contains(whiteKingField) && WhiteKingCanMove == false)
                State = ChessState.BlackWin;
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

            //if ((TeamTurn == Team.White ? FieldsBlackCanMoveTo.Contains(To) : FieldsWhiteCanMoveTo.Contains(To)) && Board.GetPiece(From).PieceType == ChessPieceType.King) return false;

            //Check if the given parameters are valid chess fields.
            Board.ValidateFields(new[] { From, To });

            if (!Board.GetPiece(From).CanMove(From, To, GetGame())) return false; //If Piece can't move to field "To", return false.

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
