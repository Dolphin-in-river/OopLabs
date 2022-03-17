namespace Banks
{
    public class DebitAccountFactory : AbstractAccountFactory
    {
        public DebitAccountFactory(double money)
            : base(money)
        {
        }

        public override AbstractAccount Create(Client client)
        {
            CheckCorrectData();

            return new DebitAccount(Data, Money, client);
        }
    }
}