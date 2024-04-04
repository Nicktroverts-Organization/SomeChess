using SomeChess.Code.GameEngine.ChessImplementation;
using SomeChess.Code.MatchMaking.ChessMatchImplementation;
using SomeChess.Code.Social;
using SomeChess.Components;


namespace SomeChess.Code.MatchMaking.ChessMatchImplementation
{



    public class ChessMatchConstructor
    {
        //public GameMode Mode { get; set; }
        //
        //private bool hasTimer {  get; set; }
        //
        //private int duration { get; set; }
        //
        //private int extraTime { get; set; }



        //public ChessMatchConstructor() { }

        //public void ChangeMode(GameMode newMode)
        //{
            //Mode = newMode;

            //hasTimer = ModePropertiesChecker.HasTimer(newMode);
            //duration = ModePropertiesChecker.GetDuration(newMode);
            //extraTime = ModePropertiesChecker.GetExtraTime(newMode);
        //}


        public ChessMatch CreateMatch(Player player, GameMode mode)
        {
            ChessMatch match = new(player, mode, MatchSearching.GetInstance().GetUniqueID());
            return match;
        }

    }
}
