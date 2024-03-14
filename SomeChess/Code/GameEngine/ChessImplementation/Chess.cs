//C# is fucking trash get some good existing, why cant i define some random word to be using i hate tis ;-;
//ispolzovat KakietoSchachmaty.Kod.IgrovoiDvighok;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public record Chess : IGame<Chess>
    {
        public ChessBoard Board = new();

        public static List<char> AlphConversionChars = new()
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };


        public ChessState GameState = ChessState.None;

        public Team TeamTurn = Team.White;

        public List<ChessPiece> BlackPieces = new();
        public List<ChessPiece> WhitePieces = new();

        public List<string> FieldsBlackCanMoveTo = new();
        public List<string> FieldsWhiteCanMoveTo = new();

        public bool? WhiteKingCanMove;
        public bool? BlackKingCanMove;

        /// <summary>
        /// <para>Whether or not the game is currently running</para>
        /// </summary>
        public bool IsPlaying
        {
            get => GameState == ChessState.Playing;
            set => throw new InvalidOperationException(nameof(IsPlaying) + "can't be set!");
        }

        /// <summary>
        /// <para>Whether or not the game is currently running</para>
        /// </summary>
        public bool IsRunning
        {
            get => GameState == ChessState.Playing;
            set => throw new InvalidOperationException(nameof(IsRunning) + "can't be set!");
        }

        /// <summary>
        /// <para>Gets the team that won, if no one won it's null</para>
        /// </summary>
        public Team? WinnerTeam
        {
            get
            {
                return GameState switch
                {
                    ChessState.BlackWin => Team.Black,
                    ChessState.WhiteWin => Team.White,
                    _ => null
                };
            }
            set => throw new InvalidOperationException(nameof(WinnerTeam) + "can't be set!");
        }

        /// <summary>
        /// <para>Gets <see cref="Chess"/>.</para>
        /// </summary>
        public Chess Game
        {
            get => this;
            set => throw new InvalidOperationException(nameof(Game) + "can't be set!");
        }

        public Chess() => ResetBoard();

        /// <summary>
        /// <para>Returns <see cref="Chess"/> class for current chess game</para>
        /// </summary>
        /// <returns><see cref="Chess"/> class for current chess game</returns>
        public Chess GetGame()
        {
            return this;
        }

        public void StartGame()
        {
            ResetBoard();
            GameState = ChessState.Playing;
        }

        /// <summary>
        /// <para>Stops the game and makes players unable to continue moving pieces</para>
        /// </summary>
        public void StopGame()
        {
            GameState = ChessState.None;
        }

        /// <summary>
        /// <para>Resets the current board to default</para>
        /// </summary>
        public void ResetBoard()
        {
            Board.SetBoardToDefault();
            TeamTurn = Team.White;
        }

        /// <summary>
        /// <para>Gets the <see cref="ChessState"/> <see cref="GameState"/> of the current <see cref="Chess"/></para>
        /// </summary>
        /// <returns><see cref="ChessState"/> <see cref="GameState"/></returns>
        public ChessState GetGameState()
        {
            return GameState;
        }

        /// <summary>
        /// <para>End current players turn.</para>
        /// </summary>
        public void EndTurn()
        {
            TeamTurn = TeamTurn == Team.White ? Team.Black : Team.White;
            UpdateGameState();
        }

        private void ClearVariables()
        {
            WhiteKingCanMove = false;
            BlackKingCanMove = false;
            WhitePieces.Clear();
            BlackPieces.Clear();
            FieldsWhiteCanMoveTo.Clear();
            FieldsBlackCanMoveTo.Clear();
        }

        /// <summary>
        /// <para>Checks game state and changes some variables according to this game state</para>
        /// <para>Performance hell</para>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If for some reason the Team enum does not equal any possible values it throws an exception</exception>
        public void UpdateGameState()
        {
            ClearVariables();
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
                            //Get From Piece
                            var FromPiece = Board.GetPiece($"{AlphConversionChars[i]}{j + 1}");

                            //Check for Fields each Team can move to
                            if (FromPiece.Team == Team.White)
                            {
                                if (FromPiece.CanMove($"{AlphConversionChars[i]}{j + 1}", $"{AlphConversionChars[x]}{y + 1}", GetGame()))
                                    FieldsWhiteCanMoveTo.Add($"{AlphConversionChars[x]}{y + 1}");
                                if (FromPiece.PieceType != ChessPieceType.None)
                                    WhitePieces.Add(FromPiece);
                            }
                            else if (FromPiece.Team == Team.Black)
                            {
                                if (FromPiece.CanMove($"{AlphConversionChars[i]}{j + 1}", $"{AlphConversionChars[x]}{y + 1}", GetGame()))
                                    FieldsBlackCanMoveTo.Add($"{AlphConversionChars[x]}{y + 1}");
                                if (FromPiece.PieceType != ChessPieceType.None)
                                    BlackPieces.Add(FromPiece);
                            }

                            //Check if the kings can move anywhere without checkmate
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
                                        throw new ArgumentOutOfRangeException("FromPiece doesn't have a Team");
                                }
                            }

                            //Write colored debug message to console
                            Console.Write($"{AlphConversionChars[i]}{j + 1} -> {AlphConversionChars[x]}{y + 1} : ");
                            Console.ForegroundColor =
                                FromPiece.CanMove($"{AlphConversionChars[i]}{j + 1}", $"{AlphConversionChars[x]}{y + 1}", GetGame()) ? ConsoleColor.Green : ConsoleColor.Red;
                            Console.Write(FromPiece.CanMove($"{AlphConversionChars[i]}{j + 1}", $"{AlphConversionChars[x]}{y + 1}", GetGame()));
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"{(FromPiece.CanMove($"{AlphConversionChars[i]}{j + 1}", $"{ AlphConversionChars[x]}{y + 1}", GetGame()) ? " " : "")}" + " | ");
                        }
                        Console.Write("\n");
                    }
                }
            }

            //set default state to playing
            GameState = ChessState.Playing;

            //Check for conditions to gather correct states
            if (FieldsWhiteCanMoveTo.Contains(blackKingField) && BlackKingCanMove == false)
                GameState = ChessState.WhiteWin;
            if (FieldsBlackCanMoveTo.Contains(whiteKingField) && WhiteKingCanMove == false)
                GameState = ChessState.BlackWin;
            if (BlackKingCanMove == false && !FieldsWhiteCanMoveTo.Contains(blackKingField) && BlackPieces.Count == 1)
                GameState = ChessState.Draw;
            if (WhiteKingCanMove == false && !FieldsBlackCanMoveTo.Contains(whiteKingField) && WhitePieces.Count == 1)
                GameState = ChessState.Draw;
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
            if (GameState != ChessState.Playing)
                throw new InvalidOperationException("Can't move piece while not game is not running!");

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
    }
}
