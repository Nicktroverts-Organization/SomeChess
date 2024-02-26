//C# is fucking trash get some good existing, why cant i define some random word to be using i hate tis ;-;
//ispolzovat KakietoSchachmaty.Kod.IgrovoiDvighok;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public record Chess : IGame<Chess>
    {
        public static ChessBoard Board = new();

        public static List<char> AlphConversionChars = new() 
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

        public static bool GameIsRunning = false;

        public Team TeamTurn = Team.White;

        public Chess() => ResetBoard();

        public void StartGame()
        {
            ResetBoard();
            GameIsRunning = true;
        }

        public void StopGame()
        {
            GameIsRunning = false;
        }

        public void ResetBoard()
        {
            Board.SetBoardToDefault();
            TeamTurn = Team.White;
        }

        public void UpdateGameState()
        {
            //todo
            throw new NotImplementedException();
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
            if (!GameIsRunning)
                throw new InvalidOperationException("Can't move piece while game is not running!");

            //Check if player is trying to move opponents piece.
            if (Board.GetPiece(From).Team != TeamTurn)
                return false;

            //Check if the given parameters are valid chess fields.
            Board.ValidateFields(new[] { From, To });

            if (!Board.GetPiece(From).CanMove(From, To)) return false; //If Piece can't move to field "To", return false.

            //Moves the piece to the new position
            Board.SetPiece(To, Board.GetPiece(From));
            Board.SetPiece(From, new EmptyPiece());
            
            //Successfully moved piece from field "From" to field "To"
            return true;
        }

        public Chess GetGame()
        {
            return this;
        }
    }
}
