namespace Banks
{
    public class WithDrawOperationFactory : ITransactionFactory
    {
        private AbstractAccount _account;
        private double _money;

        public WithDrawOperationFactory(AbstractAccount account, double money)
        {
            _account = account;
            _money = money;
        }

        public ITransaction Create()
        {
            return new WithDrawMoney(_account, _money);
        }
    }
}