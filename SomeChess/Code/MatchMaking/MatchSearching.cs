using SomeChess.Code.GameEngine;
using SomeChess.Code.Social;


namespace SomeChess.Code.MatchMaking
{
    public interface Builder
    {
        public void SetGameMode(GameMode game);

        public void SetTeam(Team team);
    }


    public class MatchSearching
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }


        public Team? Player1Team 
        {
            get => Player1Team;
            set
            {
                if (Player2Team == null)
                {
                    Player1Team = value;
                }
            }
        }

        public Team? Player2Team
        {
            get => Player2Team;
            set
            {
                if (Player1Team == null)
                {
                    if(value == Team.White)
                    {
                        Player1Team = Team.Black;
                        Player2Team = Team.White;
                    }
                    else
                    {
                        Player1Team = Team.White;
                        Player2Team = Team.Black;
                    }
                }
            }
        }


        public MatchSearching()
        {

        }

    }
}
