using System;
namespace Banks
{
    public class DepositAccountFactory : AbstractAccountFactory
    {
        private DateTime _finishDay;
        public DepositAccountFactory(double money, DateTime finishDay)
            : base(money)
        {
            _finishDay = finishDay;
        }

        public override AbstractAccount Create(Client client)
        {
            CheckCorrectData();
            return new DepositAccount(Data, Money, _finishDay, client);
        }
    }
}