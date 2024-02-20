using SomeChess.Code.GameEngine.ChessImplementation.ChessPieceCollection;

namespace SomeChess.Code.GameEngine.ChessImplementation.ChessBoard
{
    public static class ChessBoard
    {
        //todo - optimize this, It's probably a bad way to do this
        public static ChessField[,] Board = new ChessField[8, 8]
        {
            {new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField()},
            {new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField()},
            {new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField()},
            {new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField()},
            {new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField()},
            {new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField()},
            {new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField()},
            {new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField(), new ChessField()},
        };

        private static List<char> letterConversionChars = new()
        {
            'h', 'g', 'f', 'e', 'd', 'c', 'b', 'a'
        };


        public static void ResetFieldToDefault()
        {
            //todo - create this method
        }


        public static ChessField GetField(string Field)
        {
            if (!letterConversionChars.Contains(Field.ToLower()[0]))
                throw new ArgumentException("Field does not exist");

            return Board[letterConversionChars.IndexOf(Field.ToLower()[0]), Field.ToLower()[1]];
        }


        public static ChessPiece GetPiece(string Field) => GetField(Field).Piece;
    }


    //This thing is temporary so please remember to delete it at some point
    //todo - delete this at some point when it's not needed anymore
    public class Temp
    {
        public Temp()
        {
            ChessBoard.GetField("e5").Piece = ChessPieces.PawnPiece;
            ChessBoard.GetPiece("e5").Team = Team.White;

            ChessBoard.GetField("h8").Piece = ChessPieces.RookPiece;
            ChessBoard.GetPiece("h8").Team = Team.Black;
        }
    }

    public class ChessField
    {
        public ChessPiece Piece { get; set; } = ChessPieces.EmptyPiece;
    }
}
