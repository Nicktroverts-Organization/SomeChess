using SomeChess.Code.GameEngine;
using SomeChess.Code.Social;
using SomeChess.Code.GameEngine.ChessImplementation;
using System.Diagnostics;
using System.Text.RegularExpressions;
using SomeChess.Code.MatchMaking.ChessMatchImplementation;


namespace SomeChess.Code.MatchMaking
{

    //todo damn, using sealed, such professionalism. bababoye
    public sealed class MatchSearching
    {
        private List<ChessMatch> chessMatches = new();

        private List<IMatch> offlineMatches = new();


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

        public int SearchMatchID(ChessMatchSettings settings)
        {
            foreach (ChessMatch match in chessMatches)
            {
                if(match.gameMode == settings.Mode)
                {
                    return match.MatchID;
                }
                //ChessMatchSettings test = (ChessMatchSettings)settings.GetSettings();
            }
            return 0;
        }

        public ChessMatch? GetChessMatchByID(int id)
        {
            var match = chessMatches.Where(m => m.GetMatchID() == id).FirstOrDefault();

            return match;
        }

        public void AddMatch(ChessMatch match)
        {
            chessMatches.Add(match);
        }

        public void RemoveMatch(ChessMatch match)
        {
            chessMatches.Remove(match);
        }

        public int GetUniqueID()
        {
            Random rng = new();

            if (chessMatches.Count >= 999999)
                throw new Exception("Too many games already exist, impossible to create an unique ID");

            int id = rng.Next(999999);
            bool alreadyExists = false;

            foreach (ChessMatch match in chessMatches)
            {
                if (match.GetMatchID() == id)
                    alreadyExists = true;
            }

            if (alreadyExists)
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
