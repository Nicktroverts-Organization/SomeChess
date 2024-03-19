namespace SomeChess.Code.MatchMaking.ChessMatchImplementation
{
    public record ChessMatchSettings : MatchSettings
    {
        //public bool HasTimer { get; set; }
        //
        //public int Duration { get; set; }
        //
        //public int ExtraTime { get; set; }

        public GameMode Mode { get; set; }


        public ChessMatchSettings(GameMode mode)//, bool hasTimer, int duration, int extraTime)
        { 
            Mode = mode;
            //HasTimer = hasTimer;
            //Duration = duration;
            //ExtraTime = extraTime;
        }
    }
}
