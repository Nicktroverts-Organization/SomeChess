using SomeChess.Code.GameEngine;

namespace SomeChess.Code.MatchMaking
{
    public class ChessMatch : Match
    {
        private bool isStarted;

        // todo - for Ilkin - from denys and nick
        /// <summary>
        /// <para>Ilkin it's personal for you</para>
        /// </summary>
        public bool HasTimer;

        public string Black { get; }

        public string White { get; }


        public ChessMatch(IGame game, string blackPlayer, string whitePlayer, bool hasTimer = false) : base(game)
        {
            isStarted = false;

            HasTimer = hasTimer;
            Black = blackPlayer;
            White = whitePlayer;
        }

    }
}
