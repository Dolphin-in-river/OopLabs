namespace Banks
{
    public interface ITransactionFactory
    {
        ITransaction Create();
    }
}