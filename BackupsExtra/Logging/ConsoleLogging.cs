using System;

namespace BackupsExtra.Logging
{
    public class ConsoleLogging : AbstractLogging
    {
        public ConsoleLogging()
        {
            TypeLogging = TypeLogging.ConsoleLogging;
        }

        public override void ExecuteLogging(string message)
        {
            Console.WriteLine(message);
        }
    }
}