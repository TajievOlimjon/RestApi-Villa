namespace WebVilla.Logging
{
    public class Logging : ILogging
    {
        public void LogInfo(string message)
        {
            //Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Information:"+message);
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public void LogError(string message)
        {
            //Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Error:"+ message);
            Console.BackgroundColor = ConsoleColor.Red;
        }
    }
}
