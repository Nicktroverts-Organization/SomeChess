using SomeChess.Code.GameEngine;


namespace SomeChess.Code.MatchMaking
{
    public enum GameMode
    {
        OfflinePlayers,
        Multiplayer,
        WithComputer,
    }




    public abstract class Match
    {
        protected IGame game { get; set; }

        

        protected GameMode mode { get; set; }

        public Match(IGame game)
        {
            this.game = game;

            this.mode = GameMode.WithComputer;
        }

         
    }





    public class Timer
    {


        private static System.Timers.Timer aTimer;
        private int counter = 60;
        public void StartTimer()
        {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += CountDownTimer;
            aTimer.Enabled = true;
        }

        public void CountDownTimer(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (counter > 0)
            {
                counter -= 1;
            }
            else
            {
                aTimer.Enabled = false;
            }
        }
    }
}
