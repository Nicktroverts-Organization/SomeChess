//C# is fucking trash get some good existing, why cant i define some random word to be using i hate tis ;-;
//ispolzovat KakietoSchachmaty.Kod.IgrovoiDvighok;

using System.Reflection.Metadata;
using Newtonsoft.Json;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class Chess : IGame<Chess>, ICloneable
    {
        public ChessBoard Board = new();

        public static List<char> AlphConversionChars = new()
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

        public ILogger logger = LoggingHandler.GetLogger<Chess>();

        public ChessState GameState = ChessState.None;

        public Team TeamTurn = Team.White;

        public List<ChessPiece> BlackPieces = new();
        public List<ChessPiece> WhitePieces = new();

        public List<string> FieldsBlackCanMoveTo = new();
        public List<string> FieldsWhiteCanMoveTo = new();

        public string whiteKingField = "";
        public string blackKingField = "";

        public bool? WhiteKingCanMove;
        public bool? BlackKingCanMove;

        public bool Original = true;
        public Chess OriginalChess;
        public List<Chess>? Clones;

        public Guid Test = Guid.NewGuid();

        public int MadeMoves = 0;

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

        public Chess()
        {
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

        private void ClearVariables()
        {
            WhiteKingCanMove = false;
            BlackKingCanMove = false;
            WhitePieces = new();
            BlackPieces = new();
            FieldsWhiteCanMoveTo = new();
            FieldsBlackCanMoveTo = new();
            whiteKingField = "";
            blackKingField = "";
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
                logger.LogInformation($"{DateTime.Now.ToString("F")}" +
                                $"\n                                                                        Move-{MadeMoves}                                                                   " +
                            "\n      ________________________________________________________________________________________________________________________________________________");

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
                                        whiteKingField = FromPiece.Field;
                                        if (CanMoveTo)
                                            WhiteKingCanMove = true;
                                        break;
                                    case Team.Black:
                                        blackKingField = FromPiece.Field;
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
            if (FieldsWhiteCanMoveTo.Contains(blackKingField) && FieldsBlackCanMoveTo.Count == 0)
                GameState = ChessState.WhiteWin;
            if (FieldsBlackCanMoveTo.Contains(whiteKingField) && FieldsWhiteCanMoveTo.Count == 0)
                GameState = ChessState.BlackWin;
            if (BlackKingCanMove == false && !FieldsWhiteCanMoveTo.Contains(blackKingField) && BlackPieces.Count == 1)
                GameState = ChessState.Draw;
            if (WhiteKingCanMove == false && !FieldsBlackCanMoveTo.Contains(whiteKingField) && WhitePieces.Count == 1)
                GameState = ChessState.Draw;

            if (OriginalChess.Clones.Count == 0)
                logger.LogInformation("Current Chess Game State: " + GameState);
        }

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
                    if (ChessCopy.FieldsBlackCanMoveTo.Contains(ChessCopy.whiteKingField))
                    {

                        OriginalChess.Clones.RemoveAt(0);
                        ChessCopy = null;
                        return false;
                    }
                }
                else
                {
                    ChessCopy.MovePiece(From, To);
                    ChessCopy.EndTurn();
                    if (ChessCopy.FieldsWhiteCanMoveTo.Contains(ChessCopy.blackKingField))
                    {

                        OriginalChess.Clones.RemoveAt(0);
                        ChessCopy = null;
                        return false;
                    }
                }


                OriginalChess.Clones.RemoveAt(0);
                ChessCopy = null;
            }

            //Check if the given parameters are valid chess fields.
            Board.ValidateFields(new[] { From, To });

            if (!FromPiece.CanMove(From, To, this.GetGame()))
                return false; //If Piece can't move to field "To", return false.

            FromPiece.Field = To;

            //Moves the piece to the new position
            Board.SetPiece(To, FromPiece);
            Board.SetPiece(From, new EmptyPiece(FromPiece.Team, From));

            //Successfully moved piece from field "From" to field "To"
            return true;
        }

        public object Clone()
        {
            var chess = new Chess();
            chess.TeamTurn = TeamTurn;
            chess.Board = (ChessBoard)Board.Clone();
            chess.Original = !Original;
            chess.OriginalChess = OriginalChess;
            chess.Clones = null;
            return chess;
        }
    }
}
