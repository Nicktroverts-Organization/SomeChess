namespace SomeChess.Code.MatchMaking
{
    abstract class Match
    {
        public int n;

        public int[,] Board = new int[8,8]
        {
            {4, 3, 2, 6, 5, 2, 3, 4},
            {1, 1, 1, 1, 1, 1, 1, 1},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {1, 1, 1, 1, 1, 1, 1, 1},
            {4, 3, 2, 5, 6, 2, 3, 4},
        };



    }
}
