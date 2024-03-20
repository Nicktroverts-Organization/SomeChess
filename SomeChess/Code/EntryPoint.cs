using SomeChess.Code.Social;
using SomeChess;
using SomeChess.Code.MatchMaking;
using SomeChess.Code.GameEngine.ChessImplementation;
using SomeChess.Code.MatchMaking.ChessMatchImplementation;

namespace SomeChess.Code
{
    public static class LoggingHandler
    {
        public static ILoggerFactory loggerFactory = LoggerFactory.Create(
            builder => builder
                .AddConsole()
                .AddDebug()
                .AddSimpleConsole()
                .SetMinimumLevel(LogLevel.Debug)
        );

        public static ILogger GetLogger<T>()
        {
            var logger = loggerFactory.CreateLogger<T>();
            return logger;
        }
    }

    public class EntryPoint
    {

        //todo - write all the stuff!
        //create your own world!
        //with our module system!

        public EntryPoint()
        {

        }


        

        



    }
}
