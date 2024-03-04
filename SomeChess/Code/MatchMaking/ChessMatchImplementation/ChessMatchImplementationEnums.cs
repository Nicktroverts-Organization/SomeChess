namespace SomeChess.Code.MatchMaking.ChessMatchImplementation
{
    //public enum TimerDuration
    //{
    //    OneMinute = 60,
    //    TwoMinutes = 120,
    //    ThreeMinutes = 180,
    //    FiveMinutes = 300,
    //    TenMinutes = 600,
    //    FifteenMinutes = 900,
    //    ThirtyMinutes = 1800
    //}
    //
    //public enum ExtraTime
    //{
    //    NoExtraTime = 0,
    //    OneSecond = 1,
    //    TwoSecond = 2,
    //    TenSecond = 10,
    //}


    public enum GameMode
    {
        OfflinePlayers = 0,
        WithComputer = 1,

        OnlineOneMinute = 11,
        OnlineOneMinutePlusOne = 12,
        OnlineTwoMinutesPlusOne = 13,
        OnlineThreeMinutes = 14,
        OnlineThreeMinutesPlusTwo = 15,
        OnlineFiveMinutes = 16,
        OnlineTenMinutes = 17,
        OnlineFifteenMinutesPlusTen = 18,
        OnlineThirtyMinutes = 19,

    }


    static class ModePropertiesChecker
    {
        public static bool IsOnline(GameMode mode)
        {
            if(((int)mode) > 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HasTimer(GameMode mode)
        {
            if(IsOnline(mode))
            {
                return true;
            }
            else
            {
                return false;
            }

            //switch(mode)
            //{
            //    case GameMode.OfflinePlayers:
            //        return false;
            //    case GameMode.WithComputer:
            //        return false;
            //}
            //
            //return false;
        }

        public static int GetDuration(GameMode mode)
        {
            switch(mode)
            {
                case GameMode.OnlineOneMinute:
                    return 60;
                case GameMode.OnlineOneMinutePlusOne:
                    return 60;
                case GameMode.OnlineTwoMinutesPlusOne:
                    return 120;
                case GameMode.OnlineThreeMinutes:
                    return 180;
                case GameMode.OnlineThreeMinutesPlusTwo:
                    return 180;
                case GameMode.OnlineFiveMinutes:
                    return 300;
                case GameMode.OnlineTenMinutes:
                    return 600;
                case GameMode.OnlineFifteenMinutesPlusTen:
                    return 900;
                case GameMode.OnlineThirtyMinutes:
                    return 1800;
            }

            return 0;
        }

        public static int GetExtraTime(GameMode mode)
        {
            switch(mode )
            {
                case GameMode.OnlineOneMinutePlusOne:
                    return 1;
                case GameMode.OnlineTwoMinutesPlusOne:
                    return 1;
                case GameMode.OnlineThreeMinutesPlusTwo:
                    return 2;
                case GameMode.OnlineFifteenMinutesPlusTen:
                    return 10;
            }
            
            return 0;
        }



    }


}
