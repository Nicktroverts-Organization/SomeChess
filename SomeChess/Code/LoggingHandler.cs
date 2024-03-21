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

        public static Task DrawSeperatorLine(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            try
            {
                Console.WriteLine("\n" + String.Concat(Enumerable.Repeat("\u2500", Console.WindowWidth)) + "\n");
            }
            catch
            {
                Console.WriteLine("\n" + String.Concat(Enumerable.Repeat("\u2500", 25)) + "\n");
            }
            Console.ResetColor();
            return Task.CompletedTask;
        }
    }
}
