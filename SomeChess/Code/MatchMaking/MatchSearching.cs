using SomeChess.Code.GameEngine;
using SomeChess.Code.Social;
using SomeChess.Code.GameEngine.ChessImplementation;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace SomeChess.Code.MatchMaking
{

        //todo damn, using sealed, such professionalism. bababoye
    public sealed class MatchSearching  
    {   
        private List<IMatch> matches;

        private List<IMatch> offlineMatches;


        private MatchSearching()
        {

        }

        private static MatchSearching instance;

        public static MatchSearching GetInstance()
        {
            if (instance == null)
            {
                instance = new MatchSearching();
            }
            return instance;
        }

        public IMatch SearchMatch(MatchSettings settings)
        {
            foreach(IMatch match in matches)
            {
                if(match.GetType() == settings.MatchType && match.GetSettings() == settings)
                {
                    return match;
                }
            }
            return null;
        }

        public void AddMatch(IMatch match)
        {
            matches.Add(match);
        }

        public void RemoveMatch(IMatch match)
        {
            matches.Remove(match);
        }

        public int GetUniqueID()
        {
            Random rng = new();

            if (matches.Count >= 999999)
                throw new Exception("Too many games already exist, impossible to create an unique ID");

            int id = rng.Next(999999);
            bool alreadyExists = false;

            foreach(IMatch match in matches)
            {
                if(match.GetMatchID() == id) 
                    alreadyExists = true;
            }

            if( alreadyExists )
            {
                id = GetUniqueID();
                return id;
            }
            else
            {
                return id;
            }

        }






    }
}
