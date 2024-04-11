//C# is fucking trash get some good existing, why cant i define some random word to be using i hate tis ;-;
//ispolzovat KakietoSchachmaty.Kod.IgrovoiDvighok;

using System.Text;
using Newtonsoft.Json;
using SomeChess.Components;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class Chess : IGame<Chess>, ICloneable
    {
        //public string ChessID;

        public static List<char> AlphConversionChars = new()
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };


        public ChessBoard Board = new();
        public ChessBoard? LatestBoard = null;

        public ILogger logger = LoggingHandler.GetLogger<Chess>();

        
        public ChessState GameState { get; private set; } = ChessState.None;


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


        public Guid Test = Guid.NewGuid();


        public int MadeMoves = 0;
        public List<Tuple<ChessPiece, ChessPiece, bool>> ChessPieceMoveHistory = new();
        public List<ChessBoard> ChessBoardHistory = new();

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
        /// <para>Whether or not the Current viewed board is the latest one.</para>
        /// </summary>
        public bool IsLatestPosition
        {
            get => Board.Test == LatestBoard.Test;
            set => throw new InvalidOperationException(nameof(IsLatestPosition) + "can't be set!");
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
        /// <para>Defines whether one of both teams gave up.</para>
        /// </summary>
        private Team? Surrender = null;

        /// <summary>
        /// <para>Defines whether or not a draw is being forced.</para>
        /// </summary>
        private bool forcedDraw = false;

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
        /// <para>Sets the <see cref="Board"/> variable to the Specified index of the <see cref="ChessBoardHistory"/>.</para>
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void ViewPosition(int index)
        {
            index++;
            LatestBoard = (ChessBoard)Board.Clone();
            if (index == ChessBoardHistory.Count)
            {
                ViewLatestPosition();
            }
            else if (index < ChessBoardHistory.Count)
            {
                Guid _guid = ChessBoardHistory[index].Test;
                Board = (ChessBoard)ChessBoardHistory[index].Clone();
                Board.Test = _guid;
            }
            else
                throw new IndexOutOfRangeException($"The given index was larger than the size of {nameof(ChessBoardHistory)}");
            
            UpdateGameState();
        }

        /// <summary>
        /// <para>Sets the <see cref="Board"/> variable to the Latest Position that existed.</para>
        /// </summary>
        public void ViewLatestPosition()
        {
            if (LatestBoard is not null)
            {
                Guid _guid = LatestBoard.Test;
                Board = (ChessBoard)LatestBoard.Clone();
                Board.Test = _guid;
            }

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

        //Check each piece on board
        private string CheckEachPieceOnBoardForUpdateStep()
        {
            string _IDString = "";
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
                    _IDString +=
                        $"{(FromPiece.PieceType != ChessPieceType.None ? (FromPiece.PieceType == ChessPieceType.Knight ? FromPiece.PieceType.ToString().ToUpper()[1] : FromPiece.PieceType.ToString().ToUpper()[0]).ToString() + FromPiece.Team.ToString()[0].ToString() : "")}{FromPiece.Field}";

                    if (i != 7)
                        _IDString += "-";
                    else if (j != 7)
                        _IDString += "-";

                    //Check Each Piece on Board for Each Piece on Board
                    CheckEachPieceOnBoardForUpdateStep2(FromPiece);
                }
            }

            return _IDString;
        }

        private void CheckEachPieceOnBoardForUpdateStep2(ChessPiece FromPiece)
        {
            for (int x = 0; x < 8; x++)
            {
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
            string _IDString = CheckEachPieceOnBoardForUpdateStep();

            if (ChessBoardHistory.Count > 0)
                ChessBoardHistory[^1].IDString = _IDString;

            if (OriginalChess.Clones.Count == 0)
                Console.Write("\n");

            //set default state to playing
            GameState = ChessState.Playing;


            //Clean up the Lists of Fields each team can move to potentially.
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
            else CleanUpListForLastPieceLeft();

            if (BlackKingCanMove == false && !FieldsWhiteCanMoveTo.Contains(BlackKing.Field) && BlackPieces.Count == 1)
                GameState = ChessState.Draw;
            if (WhiteKingCanMove == false && !FieldsBlackCanMoveTo.Contains(WhiteKing.Field) && WhitePieces.Count == 1)
                GameState = ChessState.Draw;


            //Group the Board histoy
            var g = ChessBoardHistory.GroupBy(i => i.IDString.ToString());

            foreach (var grp in g)
            {
                //If any board ID exists 3 times or more it's a draw.
                if (grp.Count() >= 3)
                    GameState = ChessState.Draw;
            }


            if (forcedDraw)
                GameState = ChessState.Draw;
            if (Surrender != null)
                GameState = Surrender == Team.White ? ChessState.BlackWin : ChessState.WhiteWin;


            if (OriginalChess.Clones.Count == 0)
            {
                if (LatestBoard is null || IsLatestPosition)
                {
                    Guid _guid = Board.Test;
                    LatestBoard = (ChessBoard)Board.Clone();
                    LatestBoard.Test = _guid;
                }
            }


            // Some logging and Console output
            WinStateConsoleLogging();
        }

        private void CleanUpListForLastPieceLeft()
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

        private void WinStateConsoleLogging()
        {
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

        //--------UpdateGameState private Methods-----------

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


        //--------MovePiece private Methods-----------

        private bool CheckCastling(string To, List<string> fieldsEnemyCanMoveTo, int row) // no idea how to explain this ;-;
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

        private bool CheckEnPassant(ChessPiece FromPiece, string To, int direction)
        {
            if (ChessPieceMoveHistory.Count == 0) return false;

            if (ChessPieceMoveHistory[^1].Item1.PieceType != ChessPieceType.Pawn) return false;

            //Big chungus check
            if (AlphConversionChars.IndexOf(ChessPieceMoveHistory[^1].Item1.Field[0]) ==
                AlphConversionChars.IndexOf(FromPiece.Field[0]) - 1 ||
                AlphConversionChars.IndexOf(ChessPieceMoveHistory[^1].Item1.Field[0]) ==
                AlphConversionChars.IndexOf(FromPiece.Field[0]) + 1)
                if (char.GetNumericValue(ChessPieceMoveHistory[^1].Item1.Field[1]) ==
                    char.GetNumericValue(FromPiece.Field[1]) + (direction * 2) &&
                    char.GetNumericValue(ChessPieceMoveHistory[^1].Item2.Field[1]) ==
                    char.GetNumericValue(FromPiece.Field[1]))
                    if (AlphConversionChars.IndexOf(To[0]) == AlphConversionChars.IndexOf(FromPiece.Field[0]) + 1 ||
                        AlphConversionChars.IndexOf(To[0]) == AlphConversionChars.IndexOf(FromPiece.Field[0]) - 1)
                    {
                        Board.SetPiece($"{To[0]}{(char.GetNumericValue(To[1]) + (direction*-1))}", new EmptyPiece(FromPiece.Team, $"{To[0]}{(char.GetNumericValue(To[1]) + (direction * -1))}"));

                        if (OriginalChess.Clones.Count == 0)
                        {
                            ChessPieceMoveHistory.Add(new Tuple<ChessPiece, ChessPiece, bool>((ChessPiece)FromPiece.Clone(), new PawnPiece(Team.Black, $"{To[0]}{(char.GetNumericValue(To[1]) + (direction * -1))}"), true));
                            ChessBoardHistory.Add((ChessBoard)Board.Clone());
                        }

                        string From = FromPiece.Field;
                        FromPiece.Field = To;

                        //Moves the piece to the new position
                        Board.SetPiece(To, FromPiece);
                        Board.SetPiece(From, new EmptyPiece(FromPiece.Team, From));

                        return true;
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
            if (LatestBoard != null)
                if (!IsLatestPosition)
                    return false;

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


            if (FromPiece.PieceType == ChessPieceType.Pawn)
                if (CheckEnPassant(FromPiece, To, FromPiece.Team == Team.White ? 1 : -1))
                    return true;

            if (FromPiece.PieceType == ChessPieceType.King)
            {
                if (FromPiece.Team == Team.White)
                {
                    if (!WhiteKingHasMoved)
                    {
                        if (CheckCastling(To, FieldsBlackCanMoveTo, 1))
                        {
                            if (OriginalChess.Clones.Count == 0)
                            {
                                AddHistory(From, To, true);
                            }

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
                            {
                                AddHistory(From, To, true);
                            }

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
                AddHistory(From, To, false);

            FromPiece.Field = To;

            //Moves the piece to the new position
            Board.SetPiece(To, FromPiece);
            Board.SetPiece(From, new EmptyPiece(FromPiece.Team, From));

            if (OriginalChess.Clones.Count == 0)
            {
                if (TeamTurn == Team.White)
                {
                    if (FromPiece.PieceType == ChessPieceType.Pawn && Char.GetNumericValue(To[1]) == 8)
                    {
                        FrontendPageChessBoard.DoPawnTransformStart(FromPiece);
                    }
                }
                else
                {
                    if (FromPiece.PieceType == ChessPieceType.Pawn && To[1] == '1')
                    {
                        FrontendPageChessBoard.DoPawnTransformStart(FromPiece);
                    }
                }
            }

            if (OriginalChess.Clones.Count == 0)
            {
                if (LatestBoard is null || IsLatestPosition)
                {
                    Guid _guid = Board.Test;
                    LatestBoard = (ChessBoard)Board.Clone();
                    LatestBoard.Test = _guid;
                }
            }

            //Successfully moved piece from field "From" to field "To"
            return true;
        }

        private void AddHistory(string from, string to, bool Castled)
        {
            ChessPieceMoveHistory.Add(new Tuple<ChessPiece, ChessPiece, bool>((ChessPiece)Board.GetPiece(from).Clone(), (ChessPiece)Board.GetPiece(to).Clone(), Castled));
            ChessBoardHistory.Add((ChessBoard)Board.Clone());
        }


        //-----------Smol methods------------

        public void TransformPawn(Team team, string field, ChessPieceType CePiTy) => Board.SetPiece(field, ChessPieceUtils.NewChessPieceByType(team, field, CePiTy));

        /// <summary>
        /// <para>Make the Team given in the argument <paramref name="team"/> give up and lose.</para>
        /// </summary>
        /// <param name="team"></param>
        public void GiveUp(Team team) => Surrender = team;

        /// <summary>
        /// <para>Force a draw.</para>
        /// </summary>
        public void ForceDraw() => forcedDraw = true;


        //-----Implementation Methods---------

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
