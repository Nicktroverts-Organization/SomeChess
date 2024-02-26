 namespace SomeChess.Code.MatchMaking.ChessMatchImplementation
{



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
