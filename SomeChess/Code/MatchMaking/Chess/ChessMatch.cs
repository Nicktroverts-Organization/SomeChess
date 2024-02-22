using SomeChess.Code.GameEngine;

namespace SomeChess.Code.MatchMaking
{
    public class ChessMatch : Match
    {
        private bool isStarted;

        public string Black { get; }

        public string White { get; }


        public ChessMatch(IGame game, string blackPlayer, string whitePlayer) : base(game)
        {
            isStarted = false;

            Black = blackPlayer;
            White = whitePlayer;
        }

    }
}
