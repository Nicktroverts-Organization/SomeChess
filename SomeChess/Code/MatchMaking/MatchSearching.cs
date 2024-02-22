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
        public Player? Black { get; set; }
        public Player? White { get; set; }


        


        public MatchSearching(Player player)
        {
            Random rndm = new Random();
            
            if(rndm.Next(2) == 0)
            {
                Black = player;
            }
            else
            {
                White = player;
            }
        }

        public void Join(Player player)
        {

        }

    }
}
