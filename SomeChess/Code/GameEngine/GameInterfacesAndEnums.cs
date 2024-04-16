namespace SomeChess.Code.GameEngine
{
    public enum Timer
    {
        NoTimer,
        OneTimerForAll,
        TimerForEachPlayer
    }


    public enum TurnBasedGameType
    {
        Chess,
        Checkers,
        Go
    }

    public interface IGame<T>
    {
        public void StartGame();

        public void StopGame();

        public T    GetGame();
    }

}
