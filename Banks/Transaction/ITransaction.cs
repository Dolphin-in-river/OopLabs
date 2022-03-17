namespace Banks
{
    public interface ITransaction
    {
        void Cancel();
        int GetId();

        TypeTransactions GetTypeTransaction();
        string GetInformation();
        void DoTransact();
    }
}