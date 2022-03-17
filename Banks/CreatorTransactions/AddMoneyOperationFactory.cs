namespace Banks
{
    public class AddMoneyOperationFactory : ITransactionFactory
    {
        private AbstractAccount _account;
        private double _money;

        public AddMoneyOperationFactory(AbstractAccount account, double money)
        {
            _account = account;
            _money = money;
        }

        public ITransaction Create()
        {
            return new AddMoney(_account, _money);
        }
    }
}