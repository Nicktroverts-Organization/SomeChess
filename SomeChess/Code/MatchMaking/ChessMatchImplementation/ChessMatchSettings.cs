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


        public ChessMatchSettings(Type type, GameMode mode) : base(type)//, bool hasTimer, int duration, int extraTime)
        {
            Mode = mode;
            //HasTimer = hasTimer;
            //Duration = duration;
            //ExtraTime = extraTime;
        }

        public override object GetSettings()
        {
            return this;
        }
    }
}
