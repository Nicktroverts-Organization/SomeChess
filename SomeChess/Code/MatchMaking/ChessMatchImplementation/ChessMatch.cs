using SomeChess.Code.GameEngine.ChessImplementation;
using SomeChess.Code.GameEngine;
using SomeChess.Code.Social;

namespace SomeChess.Code.MatchMaking.ChessMatchImplementation
{
    public class ChessPlayer
    {
        public Player Player { get; set; }

        public Team Team { get; set; }

        public ChessPlayer(Player player, Team team)
        {
            Player = player;
            Team = team;
        }
    }


    public class ChessMatch : IMatch
    {
        public Chess Chess { get; }

        private int MatchID { get; }

        private bool isStarted;

        // todo - for Ilkin - from denys and nick
        /// <summary>
        /// <para>Ilkin it's personal for you</para>
        /// </summary>
        public bool HasTimer;

        TimerDuration Duration { get; set; }

        public ChessPlayer Black { get; set; }

        public ChessPlayer White { get; set; }


        public ChessMatch(IGame<Chess> game, Player aPlayer, int uniqueId, bool hasTimer = false)
        {
            isStarted = false;

            HasTimer = hasTimer;

            Chess = game.GetGame();

            Random rndm = new();

            if(rndm.Next(2) == 0)
            {
                White = new ChessPlayer(aPlayer, Team.White);
                Black = null;
            }
            else
            {
                Black = new ChessPlayer(aPlayer, Team.Black);
                White = null;
            }
            
            
        }


        public void Join(Player player)
        {
            if (Black == null)
            {
                Black = new ChessPlayer(player, Team.Black);
            }
            else if (White == null)
            {
                White = new ChessPlayer(player, Team.White);
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


        public void Leave(Player player)
        {
            throw new NotImplementedException();
        }


        public MatchSettings GetSettings()
        {
            return new ChessMatchSettings(HasTimer, Duration);
        }


        public int GetMatchID()
        {
            return MatchID;
        }



    }
}
