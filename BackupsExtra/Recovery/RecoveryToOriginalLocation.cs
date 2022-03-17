namespace BackupsExtra
{
    public class RecoveryToOriginalLocation : IRecovery
    {
        public RecoveryToOriginalLocation()
        {
        }

        public TypeRecovery GetTypeRecovery()
        {
            return TypeRecovery.OriginalLocation;
        }
    }
}