using Banks.Tools;

namespace Banks
{
    public class ClientBuilder : IClientBuilder
    {
        private readonly Client _client;

        public ClientBuilder()
        {
            _client = new Client();
        }

        public ClientBuilder(string name, string surname, string address = default, int passportNumber = default)
        {
            _client = new Client();
            BuildName(name).BuildSurname(surname).BuildAddress(address).BuildPassportNumber(passportNumber);
        }

        public IClientBuilder BuildName(string name)
        {
            _client.SetName(name);
            return this;
        }

        public IClientBuilder BuildSurname(string surname)
        {
            _client.SetSurname(surname);
            return this;
        }

        public IClientBuilder BuildAddress(string address)
        {
            _client.SetAddress(address);
            return this;
        }

        public IClientBuilder BuildPassportNumber(int passportNumber)
        {
            _client.SetPassportNumber(passportNumber);
            return this;
        }

        public Client GetResult()
        {
            CheckRequiredFields();
            _client.CheckNotRequireFields();

            return _client;
        }

        private void CheckRequiredFields()
        {
            if (_client.GetName() == " " || _client.GetSurname() == " ")
            {
                throw new BanksException("Client don't fill in the required fields");
            }
        }
    }
}