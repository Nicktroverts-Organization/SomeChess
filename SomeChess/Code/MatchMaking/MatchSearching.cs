using SomeChess.Code.GameEngine;
using SomeChess.Code.Social;
using SomeChess.Code.GameEngine.ChessImplementation;
using System.Diagnostics;


namespace SomeChess.Code.MatchMaking
{

        //todo damn, using sealed, such professionalism. bababoye
    public sealed class MatchSearching  
    {   
        private List<Match> matches;


        private MatchSearching()    
        {   
                  
        }

        private static MatchSearching instance;

        public static MatchSearching GetInstance()
        {
            if(instance == null)
            {
                instance = new MatchSearching();
            }
            return instance;
        }

        public void AddMatch(Match match)
        {
            matches.Add(match);
        }
    }
}
