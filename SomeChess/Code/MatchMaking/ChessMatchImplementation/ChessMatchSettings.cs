namespace SomeChess.Code.MatchMaking.ChessMatchImplementation
{
    public record ChessMatchSettings : MatchSettings
    {
        public bool HasTimer { get; }

        public TimerDuration Duration { get; }



        public ChessMatchSettings(bool hasTimer, TimerDuration duration)
        { 
            HasTimer = hasTimer;
            Duration = duration;
        }
    }
}
