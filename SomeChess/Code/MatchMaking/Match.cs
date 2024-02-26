using SomeChess.Code.GameEngine;
using SomeChess.Code.GameEngine.ChessImplementation;
using SomeChess.Code.Social;


namespace SomeChess.Code.MatchMaking
{


    public interface IMatch
    {

        public MatchSettings GetSettings();

        public void Join(Player player);

        public void Leave(Player player);

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
