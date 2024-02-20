using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class ChessBoard
    {
        public ChessPiece[,]? Board = Boards.DefaultBoard;

        private List<char> AlphConversionChars = new() 
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };


        public void SetFieldToDefault()
        {
            Board = Boards.DefaultBoard;

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
                return Board[AlphConversionChars.IndexOf(Field.ToLower()[0]), Convert.ToInt32(Field.ToLower()[1]) - 1];

            return new EmptyPiece();
        }

        public void SetPiece(string Field, ChessPiece piece)
        {
            if (ValidateField(Field))
                Board[AlphConversionChars.IndexOf(Field.ToLower()[0]), Convert.ToInt32(Field.ToLower()[1]) - 1] = piece;
        }

        public bool ValidateField(string Field)
        {
            if (!AlphConversionChars.Contains(Field.ToLower()[0]))
                throw new ArgumentException("Field does not exist!");
            if (Board[AlphConversionChars.IndexOf(Field.ToLower()[0]), Convert.ToInt32(Field.ToLower()[1]) - 1] is null)
                throw new ArgumentException("Field couldn't be found!");
            
            return true;
        }
    }
}
