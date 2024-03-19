using SomeChess.Code.GameEngine.ChessImplementation;

namespace SomeChess.Code.GameEngine.ChessImplementation
{
    public class ChessBoard : ICloneable
    {
        public ChessPiece[,]? Board = Boards.Default;

        public ChessBoard()
        {
            return;
        }

        public Guid Test = Guid.NewGuid();

        public ChessBoard(ChessBoard origin)
        {
            Board = origin.Board;
        }

        public ChessBoard GetCopy()
        {
            return new ChessBoard((ChessBoard)this.MemberwiseClone());
        }

        /// <summary>
        /// <para>Sets the <see cref="Board"/> to <see cref="Boards.Default"/></para>
        /// </summary>
        public void SetBoardToDefault()
        {
            Board = Boards.Default;
        }

        /// <summary>
        /// <para>Gets <see cref="ChessPiece"/> on <paramref name="Field"/>.</para>
        /// </summary>
        /// <param name="Field">The field to get the <see cref="ChessPiece"/> from.</param>
        /// <returns>The <see cref="ChessPiece"/> from the <paramref name="Field"/></returns>
        public ChessPiece GetPiece(string Field)
        {
            if (ValidateField(Field))
                return Board[Chess.AlphConversionChars.IndexOf(Field.ToLower()[0]), (int)Char.GetNumericValue(Field.ToLower()[1]) - 1];

            return new EmptyPiece(Team.White, Field);
        }

        /// <summary>
        /// <para>Sets the <see cref="ChessPiece"/> on <paramref name="Field"/> to <paramref name="piece"/>.</para>
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="piece"></param>
        public void SetPiece(string Field, ChessPiece piece)
        {
            if (ValidateField(Field))
                Board[Chess.AlphConversionChars.IndexOf(Field.ToLower()[0]), (int)Char.GetNumericValue(Field.ToLower()[1]) - 1] = piece;
        }

        /// <summary>
        /// <para>Checks whether or not a field actually exists.</para>
        /// </summary>
        /// <param name="Field">Chess Field to check (example: "e2")</param>
        /// <returns><c>true</c> if it's valid and <c>false</c> if it's invalid</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool ValidateField(string Field)
        {
            if (!Chess.AlphConversionChars.Contains(Field.ToLower()[0]) || Field.Length > 2)
                throw new ArgumentException("Field does not exist!");
            if (Board[Chess.AlphConversionChars.IndexOf(Field.ToLower()[0]), (int)Char.GetNumericValue(Field.ToLower()[1]) - 1] is null)
                throw new ArgumentException("Field couldn't be found!");
            
            return true;
        }

        /// <summary>
        /// <para>Checks whether or not fields actually exists.</para>
        /// </summary>
        /// <param name="Field">Chess Fields to check (example: "e2", "e3")</param>
        /// <returns><c>true</c> if they're valid and <c>false</c> if they're invalid</returns>
        public bool ValidateFields(string[] Fields)
        {
            foreach (string field in Fields)
                if (!ValidateField(field)) 
                    return false;

            return true;
        }

        public object Clone()
        {
            var board = new ChessBoard();
            ChessPiece[,]? _boardClone = Boards.CloneDefault();
            ChessPiece[,]? boardClone = new ChessPiece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boardClone[i, j] = (ChessPiece)_boardClone[i, j].Clone();
                }
            }
            board.Board = (ChessPiece[,])boardClone.Clone();
            _boardClone = null;
            boardClone = null;
            return board;
        }
    }
}
