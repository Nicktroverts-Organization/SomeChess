using SomeChess.Code.GameEngine.ChessImplementation;
using Microsoft.AspNetCore.SignalR;
using SomeChess.Code.GameEngine;
using SomeChess.Code.Social;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SomeChess.Code.MatchMaking.ChessMatchImplementation
{
    public sealed class ChessStorage
    {
        List<Chess> _chessPlays = new();

        private static ChessStorage _instance;

        public static ChessStorage GetInstance()
        {
            if(_instance == null)
            {
                _instance = new ChessStorage();
            }

            return _instance;
        }

        public void RemoveChessGameByID(Guid id)
        {
            try
            {
                Chess? chess = _chessPlays.Where(c => c.Test == id).FirstOrDefault();
                if(chess != null)
                {
                    _chessPlays.Remove(chess);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot remove a player by ID: " + e);
            }
        }

        public Chess? FindChessGameById(Guid id)
        {
            Chess? chess = _chessPlays.Where(c => c.Test == id).FirstOrDefault();

            if( chess != null )
            {
                return chess;
            }
            else
            {
                return null;
            }
        }

        public Chess CreateChessGame()
        {
            Chess chess = new Chess();
            _chessPlays.Add(chess);
            return chess;
        }
    }




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
        public Chess Chess
        {
            get => Chess;
            private set
            {
                Chess = value;
                GameID = value.Test;
            }
        }

        private int MatchID { get; }

        private Guid GameID { get; set; }

        private bool isStarted;

        // todo - for Ilkin - from denys and nick
        /// <summary>
        /// <para>Ilkin it's personal for you</para>
        /// </summary>
        
        private GameMode gameMode {  get; set; }

        public bool HasTimer { get; set; }

        public int Duration { get; set; }

        public int ExtraTime { get; set; }

        public ChessPlayer Black { get; set; }

        public ChessPlayer White { get; set; }

        public Page MatchPage { get; set; }


        public ChessMatch(IGame<Chess> game, Player aPlayer, GameMode mode, int uniqueId)
        {
            isStarted = false;

            HasTimer = ModePropertiesChecker.HasTimer(mode);
            ExtraTime = ModePropertiesChecker.GetExtraTime(mode);
            Duration = ModePropertiesChecker.GetDuration(mode);

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


        public Guid GetGameID()
        {
            return GameID;
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
            return new ChessMatchSettings(gameMode);
        }


        public int GetMatchID()
        {
            return MatchID;
        }



    }
}
