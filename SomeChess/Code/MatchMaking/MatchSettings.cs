namespace SomeChess.Code.MatchMaking
{
    public abstract record MatchSettings
    {
        public MatchSettings(Type type)
        {
            MatchType = type;
        }

        public Type MatchType { get; }

        public abstract object GetSettings();
    }
}
