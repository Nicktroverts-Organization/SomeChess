using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class ChessBoard
    {
        public ChessPiece[,]? Board = Boards.Default;


        public void SetBoardToDefault()
        {
            Board = Boards.Default;

            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (i >= 5)
                    {
                        Board[i, j].Team = Team.Black;
                    }
                    else
                    {
                        Board[i, j].Team = Team.White;
                    }
                }
            }
        }


        public ChessPiece GetPiece(string Field)
        {
            if (ValidateField(Field))
                return Board[Chess.AlphConversionChars.IndexOf(Field.ToLower()[0]), (int)Char.GetNumericValue(Field.ToLower()[1]) - 1];

            return new EmptyPiece();
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
            Console.WriteLine(Chess.AlphConversionChars.IndexOf(Field.ToLower()[0]));
            Console.WriteLine((int)Char.GetNumericValue(Field.ToLower()[1]) - 1);
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
