using SomeChess.Code.GameEngine;
using SomeChess.Code.Social;

namespace SomeChess.Code.MatchMaking.Chess
{
    public class ChessPlayer
    {
        public Player player { get; set; }

        Team team { get; set; }
    }


    public class ChessMatch : Match
    {
        private bool isStarted;

        // todo - for Ilkin - from denys and nick
        /// <summary>
        /// <para>Ilkin it's personal for you</para>
        /// </summary>
        public bool HasTimer;

        public ChessPlayer Black { get; set; }

        public ChessPlayer White { get; set; }


        public ChessMatch(IGame game, ChessPlayer? firstPlayer, ChessPlayer? secondPlayer, bool hasTimer = false) : base(game)
        {
            isStarted = false;

            HasTimer = hasTimer;


            Random rndm = new();

            if(rndm.Next(2) == 0)
            {
                White = firstPlayer;
                Black = secondPlayer;
            }
            else
            {
                Black = firstPlayer;
                White = secondPlayer;
            }
            
            
        }


        public void Join(ChessPlayer player)
        {
            if (Black == null)
            {
                Black = player;
            }
            else if (White == null)
            {
                White = player;
            }
            else
            {
                //this is temporary don't add this to master branch pls. i beg you. [Nick]
                // okay i hear your cry everything will be fine i delete that, nobody will know the truth. [Denys]
                // you mispelled hear! L  [Nick]
                throw new Exception("Match is full. Your mother wanted to make an abortion, but she was too late!");
            }
            //todo throw new NotImplementedException();
        }



    }
}
