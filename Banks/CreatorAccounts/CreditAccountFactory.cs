namespace Banks
{
    public class CreditAccountFactory : AbstractAccountFactory
    {
        public CreditAccountFactory(double money)
            : base(money)
        {
        }

        public override AbstractAccount Create(Client client)
        {
            CheckCorrectData();

            return new CreditAccount(Data, Money, client);
        }
    }
}