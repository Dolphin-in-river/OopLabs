namespace BackupsExtra
{
    public class RecoveryToDifferentLocation : IRecovery
    {
        public RecoveryToDifferentLocation(string newLocation)
        {
            NewLocation = newLocation;
        }

        public string NewLocation
        {
            get;
        }

        public TypeRecovery GetTypeRecovery()
        {
            return TypeRecovery.DifferentLocation;
        }
    }
}