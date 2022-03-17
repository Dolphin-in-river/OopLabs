namespace BackupsExtra.Logging
{
    public abstract class AbstractLogging
    {
        public AbstractLogging()
        {
        }

        public TypeLogging TypeLogging
        {
            get;
            protected set;
        }

        public bool Configuration
        {
            get;
            set;
        }

        public abstract void ExecuteLogging(string message);
    }
}