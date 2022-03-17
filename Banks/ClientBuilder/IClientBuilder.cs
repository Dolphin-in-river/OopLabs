namespace Banks
{
    public interface IClientBuilder
    {
        IClientBuilder BuildName(string name);
        IClientBuilder BuildSurname(string surname);
        IClientBuilder BuildAddress(string address);
        IClientBuilder BuildPassportNumber(int passportNumber);
        Client GetResult();
    }
}