//C# is fucking trash get some good existing, why cant i define some random word to be using i hate tis ;-;
//ispolzovat KakietoSchachmaty.Kod.IgrovoiDvighok;

using SomeChess.Components;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class Chess : IGame<Chess>, ICloneable
    {
        public static List<char> AlphConversionChars = new()
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };


        public ChessBoard Board = new();

        public ILogger logger = LoggingHandler.GetLogger<Chess>();

        
        public ChessState GameState = ChessState.None;


        public Chessboard FrontendPageChessBoard;

        
        public Team TeamTurn = Team.White;


        public List<ChessPiece> BlackPieces = new();
        public List<ChessPiece> WhitePieces = new();

        public List<string> FieldsBlackCanMoveTo = new();
        public List<string> FieldsWhiteCanMoveTo = new();

        public ChessPiece WhiteKing = new KingPiece(Team.White, "");
        public ChessPiece BlackKing = new KingPiece(Team.White, "");

        public bool? WhiteKingCanMove;
        public bool? BlackKingCanMove;

        public bool WhiteKingHasMoved = false;
        public bool BlackKingHasMoved = false;


        public bool Original = true;
        public Chess OriginalChess;
        public List<Chess>? Clones;


        public Guid ChessID = Guid.NewGuid();


        public int MadeMoves = 0;
        public List<Tuple<ChessPiece, ChessPiece, bool>> ChessPieceMoveHistory = new();

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
        /// <para>Whether or not the game ended in a Draw</para>
        /// </summary>
        public bool IsDraw
        {
            get => GameState == ChessState.Draw;
            set => throw new InvalidOperationException(nameof(IsDraw) + "can't be set!");
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

        public Chess(Chessboard cb)
        {
            FrontendPageChessBoard = cb;
            OriginalChess = this;
            if (Clones == null)
                Clones = new();
            ResetBoard();
        }

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
            ChessPieceMoveHistory = new();
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
            MadeMoves++;
            UpdateGameState();
        }

        /// <summary>
        /// Quite self explanatory i think.
        /// </summary>
        private void ClearVariables()
        {
            WhiteKingCanMove = false;
            BlackKingCanMove = false;
            WhitePieces = new();
            BlackPieces = new();
            FieldsWhiteCanMoveTo = new();
            FieldsBlackCanMoveTo = new();
        }

        /// <summary>
        /// <para>Checks game state and changes some variables according to this game state</para>
        /// <para>Performance hell</para>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If for some reason the Team enum does not equal any possible values it throws an exception</exception>
        public void UpdateGameState()
        {
            ClearVariables();
            if (OriginalChess.Clones.Count == 0)
            {
                LoggingHandler.DrawSeperatorLine(ConsoleColor.Gray);
                Console.WriteLine(String.Concat(Enumerable.Repeat(" ", (Console.WindowWidth / 2) - 8)) + $"Move-{MadeMoves}" + String.Concat(Enumerable.Repeat(" ", (Console.WindowWidth / 2) - 8)));
                LoggingHandler.DrawSeperatorLine(ConsoleColor.Gray);
                logger.LogInformation($"{DateTime.Now.ToString("F")}\n      Current Move: {(TeamTurn == Team.White ? "White" : "Black")}\n      Next Move: {(TeamTurn == Team.White ? "Black" : "White")}");
            }

            //witness the power of Quadruple loop!
            bool LastWasTrue = true;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Get From Piece
                    var FromPiece = Board.GetPiece($"{AlphConversionChars[i]}{j + 1}");

                    if (FromPiece.PieceType != ChessPieceType.None)
                    {
                        if (FromPiece.Team == Team.White)
                            WhitePieces.Add(FromPiece);
                        else
                            BlackPieces.Add(FromPiece);
                    }
                    for (int x = 0; x < 8; x++)
                    {
                        LastWasTrue = false;
                        for (int y = 0; y < 8; y++)
                        {

                            bool CanMoveTo = FromPiece.CanMove($"{FromPiece.Field}",
                                $"{AlphConversionChars[x]}{y + 1}", GetGame());

                            //Check for Fields each Team can move to
                            if (FromPiece.Team == Team.White)
                            {
                                if (CanMoveTo)
                                    if (FromPiece.PieceType != ChessPieceType.Pawn)
                                        FieldsWhiteCanMoveTo.Add($"{AlphConversionChars[x]}{y + 1}");
                            }
                            else if (FromPiece.Team == Team.Black)
                            {
                                if (CanMoveTo)
                                    if (FromPiece.PieceType != ChessPieceType.Pawn)
                                        FieldsBlackCanMoveTo.Add($"{AlphConversionChars[x]}{y + 1}");
                            }

                            //Check if the kings can move anywhere without checkmate
                            if (FromPiece.PieceType == ChessPieceType.King)
                            {
                                switch (FromPiece.Team)
                                {
                                    case Team.White:
                                        WhiteKing = FromPiece;
                                        if (CanMoveTo)
                                            WhiteKingCanMove = true;
                                        break;
                                    case Team.Black:
                                        BlackKing = FromPiece;
                                        if (CanMoveTo)
                                            BlackKingCanMove = true;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException("FromPiece doesn't have a Team");
                                }
                            }
                        }
                    }
                }
            }

            if (OriginalChess.Clones.Count == 0)
                Console.Write("\n");

            //set default state to playing
            GameState = ChessState.Playing;


            //Beispiel//


            if (OriginalChess.Clones.Count == 0)
            {
                Task CleanUpWhite = new Task(CleanUpFieldsWhiteCanMoveTo);
                Task CleanUpBlack = new Task(() => CleanUpFieldsBlackCanMoveTo(CleanUpWhite));

                CleanUpWhite.Start();
                CleanUpBlack.Start();
                Task.WaitAll(CleanUpWhite, CleanUpBlack);
                if (OriginalChess.Clones.Count != 0)
                {
                    OriginalChess.Clones.RemoveRange(0, OriginalChess.Clones.Count);
                }
            }

            //Check for conditions to gather correct states
            if (FieldsWhiteCanMoveTo.Contains(BlackKing.Field) && FieldsBlackCanMoveTo.Count == 0)
                GameState = ChessState.WhiteWin;
            if (FieldsBlackCanMoveTo.Contains(WhiteKing.Field) && FieldsWhiteCanMoveTo.Count == 0)
                GameState = ChessState.BlackWin;
            if (WhitePieces.Count == 1 && BlackPieces.Count == 1)
                GameState = ChessState.Draw;
            else
            {
                if (WhitePieces.Count == 1)
                {
                    for (int i = 0; i < FieldsWhiteCanMoveTo.Count; i++)
                    {
                        if (FieldsBlackCanMoveTo.Contains(FieldsWhiteCanMoveTo[i]))
                        {
                            FieldsWhiteCanMoveTo.RemoveAt(i);
                            i--;
                        }
                    }

                    if (FieldsWhiteCanMoveTo.Count == 0)
                        WhiteKingCanMove = false;
                }
                else if (BlackPieces.Count == 1)
                {
                    for (int i = 0; i < FieldsBlackCanMoveTo.Count; i++)
                    {
                        if (FieldsWhiteCanMoveTo.Contains(FieldsBlackCanMoveTo[i]))
                        {
                            FieldsBlackCanMoveTo.RemoveAt(i);
                            i--;
                        }
                    }

                    if (FieldsBlackCanMoveTo.Count == 0)
                        BlackKingCanMove = false;
                }
            }
            if (BlackKingCanMove == false && !FieldsWhiteCanMoveTo.Contains(BlackKing.Field) && BlackPieces.Count == 1)
                GameState = ChessState.Draw;
            if (WhiteKingCanMove == false && !FieldsBlackCanMoveTo.Contains(WhiteKing.Field) && WhitePieces.Count == 1)
                GameState = ChessState.Draw;

            if (OriginalChess.Clones.Count == 0)
            {
                if (GameState == ChessState.Playing)
                {
                    Console.WriteLine("      Current Chess Game State: " + GameState + "\n");
                    LoggingHandler.DrawSeperatorLine(ConsoleColor.DarkGray).Wait();
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("      Current Chess Game State: " + GameState + "\n");
                    LoggingHandler.DrawSeperatorLine(ConsoleColor.Gray).Wait();
                    Console.ResetColor();

                    string winMessage;
                    if (GameState == ChessState.BlackWin)
                        winMessage = "Black Won";
                    else if (GameState == ChessState.WhiteWin)
                        winMessage = "White Won";
                    else
                        winMessage = "Draw";

                    Console.WriteLine(String.Concat(Enumerable.Repeat(" ", (Console.WindowWidth / 2) - winMessage.Length)) + winMessage + String.Concat(Enumerable.Repeat(" ", (Console.WindowWidth / 2) - winMessage.Length)));

                    LoggingHandler.DrawSeperatorLine(ConsoleColor.Gray).Wait();
                }
            }
        }


        //Beispiel//


        private void CleanUpFieldsWhiteCanMoveTo()
        {
            Chess? ChessCopy = (Chess)Clone();
            ChessCopy.TeamTurn = Team.White;
            OriginalChess.Clones.Add(ChessCopy);
            ChessCopy.UpdateGameState();
            ChessBoard _board = (ChessBoard)ChessCopy.Board.Clone();

            if (OriginalChess.Clones.Count < 4)
            {
                ChessCopy.Original = true;
            }

            for (int j = 0; j < FieldsWhiteCanMoveTo.Count; j++)
            {
                bool CanMoveToFieldJ = false;

                foreach (var i in WhitePieces)
                {
                    ChessCopy.Board = (ChessBoard)_board.Clone();
                    if (ChessCopy.MovePiece(i.Field, FieldsWhiteCanMoveTo[j]))
                    {
                        CanMoveToFieldJ = true;
                        break;
                    }
                }

                if (!CanMoveToFieldJ)
                {
                    FieldsWhiteCanMoveTo.RemoveAt(j);
                    j--;
                }
            }

            try
            {
                OriginalChess.Clones.RemoveAt(0);
            }
            catch
            {
                if (OriginalChess.Clones.Count != 0)
                    OriginalChess.Clones.RemoveAt(0);
            }
        }

        private void CleanUpFieldsBlackCanMoveTo(Task other)
        {
            Chess? ChessCopy = (Chess)Clone();
            ChessCopy.TeamTurn = Team.Black;
            OriginalChess.Clones.Add(ChessCopy);
            ChessCopy.UpdateGameState();
            ChessBoard _board = (ChessBoard)ChessCopy.Board.Clone();

            if (OriginalChess.Clones.Count < 4)
            {
                ChessCopy.Original = true;
            }

            for (int j = 0; j < FieldsBlackCanMoveTo.Count; j++)
            {
                bool CanMoveToFieldJ = false;

                foreach (var i in BlackPieces)
                {
                    ChessCopy.Board = (ChessBoard)_board.Clone();
                    if (ChessCopy.MovePiece(i.Field, FieldsBlackCanMoveTo[j]))
                    {
                        CanMoveToFieldJ = true;
                        break;
                    }
                }

                if (!CanMoveToFieldJ)
                {
                    FieldsBlackCanMoveTo.RemoveAt(j);
                    j--;
                }
            }

            try
            {
                OriginalChess.Clones.RemoveAt(0);
            }
            catch
            {
                if (OriginalChess.Clones.Count != 0)
                    OriginalChess.Clones.RemoveAt(0);
            }
        }

        private bool CheckCastling(string To, List<string> fieldsEnemyCanMoveTo, int row)
        {
            if (To == $"g{row}")
            {
                if (Board.GetPiece($"f{row}").PieceType == ChessPieceType.None && Board.GetPiece($"g{row}").PieceType == ChessPieceType.None && Board.GetPiece($"h{row}").PieceType == ChessPieceType.Rook)
                {
                    if (!fieldsEnemyCanMoveTo.Contains(To) && !fieldsEnemyCanMoveTo.Contains($"f{row}") &&
                        !fieldsEnemyCanMoveTo.Contains($"e{row}"))
                    {
                        Board.SetPiece($"f{row}", Board.GetPiece($"h{row}"));
                        Board.SetPiece($"h{row}", new EmptyPiece(Team.White, $"h{row}"));
                        return true;
                    }
                }
            }
            else if (To == $"c{row}")
            {
                if (Board.GetPiece($"d{row}").PieceType == ChessPieceType.None && Board.GetPiece($"c{row}").PieceType == ChessPieceType.None && Board.GetPiece($"a{row}").PieceType == ChessPieceType.Rook)
                {
                    if (!fieldsEnemyCanMoveTo.Contains(To) && !fieldsEnemyCanMoveTo.Contains($"d{row}") &&
                        !fieldsEnemyCanMoveTo.Contains($"e{row}"))
                    {
                        Board.SetPiece($"d{row}", Board.GetPiece($"a{row}"));
                        Board.SetPiece($"a{row}", new EmptyPiece(Team.White, $"a{row}"));
                        return true;
                    }
                }
            }

            return false;
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
            {
                logger.LogWarning("Trying to move piece even though game is not running!");
                return false;
            }

            var FromPiece = Board.GetPiece(From);

            //Check if player is trying to move opponents piece.
            if (OriginalChess.Clones.Count == 0)
                if (FromPiece.Team != TeamTurn)
                    return false;

            if (Original)
            {
                Chess? ChessCopy = (Chess)Clone();
                OriginalChess.Clones.Add(ChessCopy);
                ChessCopy.UpdateGameState();


                if (TeamTurn == Team.White)
                {
                    ChessCopy.MovePiece(From, To);
                    ChessCopy.EndTurn();
                    if (ChessCopy.FieldsBlackCanMoveTo.Contains(ChessCopy.WhiteKing.Field))
                    {
                        try
                        {
                            OriginalChess.Clones.RemoveAt(0);
                        }
                        catch
                        {
                            if (OriginalChess.Clones.Count != 0)
                                OriginalChess.Clones.RemoveAt(0);
                        }
                        ChessCopy = null;
                        return false;
                    }
                }
                else
                {
                    ChessCopy.MovePiece(From, To);
                    ChessCopy.EndTurn();
                    if (ChessCopy.FieldsWhiteCanMoveTo.Contains(ChessCopy.BlackKing.Field))
                    {

                        try
                        {
                            OriginalChess.Clones.RemoveAt(0);
                        }
                        catch
                        {
                            if (OriginalChess.Clones.Count != 0)
                                OriginalChess.Clones.RemoveAt(0);
                        }
                        ChessCopy = null;
                        return false;
                    }
                }

                try
                {
                    OriginalChess.Clones.RemoveAt(0);
                }
                catch
                {
                    if (OriginalChess.Clones.Count != 0)
                        OriginalChess.Clones.RemoveAt(0);
                }
                ChessCopy = null;
            }

            //Check if the given parameters are valid chess fields.
            Board.ValidateFields(new[] { From, To });


            if (FromPiece.PieceType == ChessPieceType.King)
            {
                if (FromPiece.Team == Team.White)
                {
                    if (!WhiteKingHasMoved)
                    {
                        if (CheckCastling(To, FieldsBlackCanMoveTo, 1))
                        {
                            if (OriginalChess.Clones.Count == 0)
                                ChessPieceMoveHistory.Add(new Tuple<ChessPiece, ChessPiece, bool>((ChessPiece)FromPiece.Clone(), (ChessPiece)Board.GetPiece(To).Clone(), true));

                            FromPiece.Field = To;

                            //Moves the piece to the new position
                            Board.SetPiece(To, FromPiece);
                            Board.SetPiece(From, new EmptyPiece(FromPiece.Team, From));

                            return true;
                        }
                    }
                }
                else
                {
                    if (!BlackKingHasMoved)
                    {
                        if (CheckCastling(To, FieldsWhiteCanMoveTo, 8))
                        {
                            if (OriginalChess.Clones.Count == 0)
                                ChessPieceMoveHistory.Add(new Tuple<ChessPiece, ChessPiece, bool>((ChessPiece)FromPiece.Clone(), (ChessPiece)Board.GetPiece(To).Clone(), true));

                            FromPiece.Field = To;

                            //Moves the piece to the new position
                            Board.SetPiece(To, FromPiece);
                            Board.SetPiece(From, new EmptyPiece(FromPiece.Team, From));

                            return true;
                        }
                    }
                }
            }

            if (!FromPiece.CanMove(From, To, GetGame()))
                return false; //If Piece can't move to field "To", return false.

            if (FromPiece.PieceType == ChessPieceType.King)
            {
                if (FromPiece.Team == Team.White)
                    WhiteKingHasMoved = true;
                else
                    BlackKingHasMoved = true;
            }

            if (OriginalChess.Clones.Count == 0)
            {
                if (FromPiece.PieceType == ChessPieceType.Pawn)
                {
                    if (To[1] == '8' || To[1] == '1')
                    {
                        //ChessPiece PawnToChessPiece = FrontendPageChessBoard.TransformPawn(FromPiece.Team, Board.GetPiece(To));
                        //Board.SetPiece(To, PawnToChessPiece);
                        //Board.SetPiece(From, new EmptyPiece(FromPiece.Team, From);
                        //return true;
                    }
                }

                ChessPieceMoveHistory.Add(new Tuple<ChessPiece, ChessPiece, bool>((ChessPiece)FromPiece.Clone(), (ChessPiece)Board.GetPiece(To).Clone(), false));
            }

            FromPiece.Field = To;

            //Moves the piece to the new position
            Board.SetPiece(To, FromPiece);
            Board.SetPiece(From, new EmptyPiece(FromPiece.Team, From));

            //Successfully moved piece from field "From" to field "To"
            return true;
        }

        public object Clone()
        {
            var chess = new Chess(FrontendPageChessBoard);
            chess.TeamTurn = TeamTurn;
            chess.Board = (ChessBoard)Board.Clone();
            chess.Original = !Original;
            chess.OriginalChess = OriginalChess;
            chess.Clones = null;
            return chess;
        }
    }
}
