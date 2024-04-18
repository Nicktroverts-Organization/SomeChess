namespace SomeChess.Code.MatchMaking
{
    public abstract record MatchSettings
    {
        public Type MatchType { get; }
    }
}
