namespace SomeChess.Code.MatchMaking.ChessMatchImplementation
{
    public record ChessMatchSettings : MatchSettings
    {
        public bool HasTimer { get; set; }

        public TimerDuration Duration { get; set; }



        public ChessMatchSettings(bool hasTimer, TimerDuration duration)
        { 
            HasTimer = hasTimer;
            Duration = duration;
        }
    }
}
