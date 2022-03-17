namespace Banks
{
    public class TransferFactory : ITransactionFactory
    {
        private AbstractAccount _sender;
        private AbstractAccount _giver;
        private double _money;

        public TransferFactory(AbstractAccount sender, AbstractAccount giver, double money)
        {
            _sender = sender;
            _giver = giver;
            _money = money;
        }

        public ITransaction Create()
        {
            return new Transfer(_sender, _giver, _money);
        }
    }
}