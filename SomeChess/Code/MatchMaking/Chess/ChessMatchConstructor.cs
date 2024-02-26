 namespace SomeChess.Code.MatchMaking.Chess
{
    public enum TimerDuration
    {
        FourSecondTimer = 4,
        FifteenSecondTimer = 15,
        OneMinuteTimer = 60,
        TwoMinuteTimer = 120,
        FiveMinuteTimer = 300
    }


    public class ChessMatchConstructor
    {
        public bool HasTimer { get; set; }

        public TimerDuration RoundDuration
        {
            get => RoundDuration;
            set { if (HasTimer) { RoundDuration = value; } }
        }


    }
}
