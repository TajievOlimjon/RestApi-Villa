namespace WebVilla.Logging
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            if (type.ToLower() == "error")
            {
                LogError(message);
            }
            else if(type.ToLower() == "information")
            {
                LogInformation(message);
            }
        }
        public void LogInformation(string message)
        {
            Console.WriteLine("Information:" + " "+message);
            Console.BackgroundColor = ConsoleColor.White;
        }
        public void LogError(string message)
        {
            Console.WriteLine("Error:" + " " + message);
            Console.BackgroundColor = ConsoleColor.Red;
        }
    }
}
