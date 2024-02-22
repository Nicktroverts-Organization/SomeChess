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

    public enum Team
    {
        White,
        Black
    }

    public interface IGame
    {
        public void StartGame();

        public void StopGame();
    }

}
