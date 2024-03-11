using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class ChessBoard
    {
        public ChessPiece[,]? Board = Boards.Default;


        public void SetBoardToDefault()
        {
            Board = Boards.Default;
        }


        public ChessPiece GetPiece(string Field)
        {
            if (ValidateField(Field))
                return Board[Chess.AlphConversionChars.IndexOf(Field.ToLower()[0]), (int)Char.GetNumericValue(Field.ToLower()[1]) - 1];

            return new EmptyPiece(Team.White);
        }

        public void SetPiece(string Field, ChessPiece piece)
        {
            if (ValidateField(Field))
                Board[Chess.AlphConversionChars.IndexOf(Field.ToLower()[0]), (int)Char.GetNumericValue(Field.ToLower()[1]) - 1] = piece;
        }

        public bool ValidateField(string Field)
        {
            if (!Chess.AlphConversionChars.Contains(Field.ToLower()[0]) || Field.Length > 2)
                throw new ArgumentException("Field does not exist!");
            if (Board[Chess.AlphConversionChars.IndexOf(Field.ToLower()[0]), (int)Char.GetNumericValue(Field.ToLower()[1]) - 1] is null)
                throw new ArgumentException("Field couldn't be found!");
            
            return true;
        }

        public bool ValidateFields(string[] Fields)
        {
            foreach (string field in Fields)
                if (!ValidateField(field)) 
                    return false;

            return true;
        }
    }
}
