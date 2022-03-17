using System;
using Banks.Tools;

namespace Banks
{
    public class Transfer : ITransaction
    {
        private static int _nextId = 0;
        private AbstractAccount _sender;
        private AbstractAccount _giver;
        private double _money;
        private bool _isCanceled = false;
        private int _id;
        private TypeTransactions _type;

        public Transfer(AbstractAccount sender, AbstractAccount giver, double money)
        {
            _sender = sender;
            _giver = giver;
            _money = money;
            _id = _nextId++;
            _type = TypeTransactions.Transfer;
        }

        public int GetId()
        {
            return _id;
        }

        public TypeTransactions GetTypeTransaction()
        {
            return _type;
        }

        public string GetInformation()
        {
            return _giver.GetClient().GetName() + _sender.GetClient().GetName() + _id;
        }

        public AbstractAccount GetSender()
        {
            return _sender;
        }

        public AbstractAccount GetGiver()
        {
            return _giver;
        }

        public void Cancel()
        {
            if (_isCanceled)
            {
                throw new BanksException("This transaction has already been cancelled");
            }

            _isCanceled = true;
            _sender.AddMoney(_money);
            _giver.WithDraw(_money);
        }

        public void DoTransact()
        {
            if (_money < 0)
            {
                throw new BanksException("Input data are incorrect");
            }

            _sender.WithDraw(_money);
            _giver.AddMoney(_money);
        }
    }
}