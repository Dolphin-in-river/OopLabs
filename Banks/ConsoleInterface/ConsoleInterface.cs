using System;
using System.Collections.Generic;
using Banks.Tools;
using Spectre.Console;

namespace Banks
{
    public class ConsoleInterface
    {
        private ICentralBank _centralBank;

        public ConsoleInterface()
        {
            Start();
        }

        public void Start()
        {
            _centralBank = new CentralBank();
            string operation = ChooseOperation();
            while (operation != "EXIT")
            {
                CommandHandler(operation);
                operation = ChooseOperation();
            }
        }

        private string ChooseOperation()
        {
            string operation = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select the operation you need in [blue]Bank[/]?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Add Bank", "Get Balance", "Update Money Information", "Make Transaction",
                        "Cancel Transaction", "Add Client and Link him with Account", "Change Information",
                        "Get Bank Information", "EXIT",
                    }));

            return operation;
        }

        private void CommandHandler(string operation)
        {
            switch (operation)
            {
                case "Add Bank":
                    DoAddBank();
                    break;
                case "Update Money Information":
                    UpdateMoneyInformation();
                    break;
                case "Add Client and Link him with Account":
                    AddClientAndLinkWithAccount();
                    break;
                case "Get Balance":
                    GetBalance();
                    break;
                case "Make Transaction":
                    MakeTransaction();
                    break;
                case "Cancel Transaction":
                    CancelTransaction();
                    break;
                case "Change Information":
                    ChangeInformation();
                    break;
                case "Get Bank Information":
                    GetBankInformation();
                    break;
                case "EXIT":
                    break;
            }
        }

        private void GetBankInformation()
        {
            if (_centralBank.GetBanksNames().Count == 0)
            {
                throw new BanksException("Any bank hasn't registered");
            }

            string bankName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(5)
                    .AddChoices(_centralBank.GetBanksNames()));
            AnsiConsole.Clear();
            var typesInformation = new List<string>
            {
                "Get Name Bank",
                "Get Debit Info",
                "Get Deposit Info",
                "Get Credit Info",
                "Get Critical Sum",
            };
            string typeAccountCreator = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(typesInformation.Count)
                    .AddChoices(typesInformation));
            AnsiConsole.Clear();
            foreach (Bank item in _centralBank.GetBanks())
            {
                if (item.Name == bankName)
                {
                    switch (typeAccountCreator)
                    {
                        case "Get Name Bank":
                            AnsiConsole.WriteLine(item.Name);
                            break;
                        case "Get Debit Info":
                            AnsiConsole.WriteLine("Debit Percent is = " + item.Data.DebitPercent);
                            break;
                        case "Get Deposit Info":
                            for (int i = 0; i < item.Data.InfoDeposit.SumsInDeposits.Count; i++)
                            {
                                AnsiConsole.WriteLine(item.Data.InfoDeposit.SumsInDeposits[i] + " " +
                                                      item.Data.InfoDeposit.PercentsInDeposits[i]);
                            }

                            break;
                        case "Get Credit Info":
                            AnsiConsole.WriteLine("Credit Limit is " + item.Data.CreditLimit);
                            AnsiConsole.WriteLine("Credit Commission " + item.Data.CreditCommission);
                            break;
                        case "Get Critical Sum":
                            AnsiConsole.WriteLine(item.Data.CriticalSum);
                            break;
                        default:
                            throw new BanksException("Error in input data");
                    }
                }
            }
        }

        private void CancelTransaction()
        {
            if (_centralBank.GetNamesTransaction().Count == 0)
            {
                AnsiConsole.WriteLine("Central Bank hasn't any transactions");
            }

            string closedTransaction = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(5)
                    .AddChoices(_centralBank.GetNamesTransaction()));
            AnsiConsole.Clear();
            for (int i = 0; i < _centralBank.GetTransactions().Count; i++)
            {
                if (_centralBank.GetTransactions()[i].GetInformation() == closedTransaction)
                {
                    _centralBank.CancelTransaction(_centralBank.GetTransactions()[i]);
                }
            }

            AnsiConsole.WriteLine("Transaction has deleted");
        }

        private void ChangeInformation()
        {
            if (_centralBank.GetBanksNames().Count == 0)
            {
                throw new BanksException("Any bank hasn't registered");
            }

            string bankName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(5)
                    .AddChoices(_centralBank.GetBanksNames()));
            AnsiConsole.Clear();
            var typesChanged = new List<string>
            {
                "Change Debit Info",
                "Change Deposit Info",
                "Change Credit Info",
                "Change Critical Sum",
            };
            string typeAccountCreator = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(typesChanged.Count)
                    .AddChoices(typesChanged));
            AnsiConsole.Clear();
            foreach (Bank item in _centralBank.GetBanks())
            {
                if (item.Name == bankName)
                {
                    switch (typeAccountCreator)
                    {
                        case "Change Debit Info":
                            double newDebitPercent = AnsiConsole.Ask<double>("New Debit Percent is = ");
                            item.ChangeDebitInfo(newDebitPercent);
                            break;
                        case "Change Deposit Info":
                            int amount =
                                AnsiConsole.Ask<int>("How many elements will be in New Deposit account information? ");
                            var sumInDeposits = new List<double>();
                            var percentsInDeposits = new List<double>();
                            for (int i = 0; i < amount; i++)
                            {
                                double sum = AnsiConsole.Ask<double>(i + "- new sum in deposit is = ");
                                double percent = AnsiConsole.Ask<double>(i + "- new percent in deposit is = ");
                                sumInDeposits.Add(sum);
                                percentsInDeposits.Add(percent);
                            }

                            var newDepositInfo = new DepositInfo(sumInDeposits, percentsInDeposits);
                            item.ChangeDepositInfo(newDepositInfo);
                            break;
                        case "Change Credit Info":
                            double newCreditLimit = AnsiConsole.Ask<double>("New Credit Limit is = ");
                            double newCreditCommission = AnsiConsole.Ask<double>("New Credit Commission is = ");
                            item.ChangeCreditInfo(newCreditLimit, newCreditCommission);
                            break;
                        case "Change Critical Sum":
                            double newCriticalSum = AnsiConsole.Ask<double>("New Critical Sum is = ");
                            item.ChangeCriticalSum(newCriticalSum);
                            break;
                        default:
                            throw new BanksException("Error in input data");
                    }
                }
            }
        }

        private void MakeTransaction()
        {
            AnsiConsole.WriteLine("Please choose account, that gives money");
            AbstractAccount giver = FindAccount();
            var typesTransactions = new List<string>
            {
                "Add Money",
                "Transfer Money between accounts",
                "With Draw",
            };
            string typeAccountCreator = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(typesTransactions.Count)
                    .AddChoices(typesTransactions));
            AnsiConsole.Clear();
            double money = AnsiConsole.Ask<double>("How many money do you want to provide for transaction? ");
            switch (typeAccountCreator)
            {
                case "Add Money":
                    _centralBank.MakeTransaction(new AddMoneyOperationFactory(giver, money));
                    break;
                case "With Draw":
                    _centralBank.MakeTransaction(new WithDrawOperationFactory(giver, money));
                    break;
                case "Transfer Money between accounts":
                    AbstractAccount sender = FindAccount();
                    if (sender.GetId() == giver.GetId())
                    {
                        throw new BanksException("You couldn't transfer money for your account");
                    }

                    _centralBank.MakeTransaction(new TransferFactory(sender, giver, money));
                    break;
                default:
                    throw new BanksException("Error in input data");
            }
        }

        private void DoAddBank()
        {
            string bankName = AnsiConsole.Ask<string>("Bank Name is = ");
            double debitPercent = AnsiConsole.Ask<double>("Bank Debit Percent is = ");
            int amount = AnsiConsole.Ask<int>("How many elements will be in Deposit account information? ");
            var sumInDeposits = new List<double>();
            var percentsInDeposits = new List<double>();
            for (int i = 0; i < amount; i++)
            {
                double sum = AnsiConsole.Ask<double>(i + "-sum in deposit is = ");
                double percent = AnsiConsole.Ask<double>(i + "-percent in deposit is = ");
                sumInDeposits.Add(sum);
                percentsInDeposits.Add(percent);
            }

            var depositInfo = new DepositInfo(sumInDeposits, percentsInDeposits);
            double creditLimit = AnsiConsole.Ask<double>("Credit Limit is = ");
            double creditCommission = AnsiConsole.Ask<double>("Credit Commission is = ");
            double criticalSum = AnsiConsole.Ask<double>("Critical Sum is = ");
            _centralBank.AddBank(bankName, debitPercent, depositInfo, creditLimit, creditCommission, criticalSum);
        }

        private void GetBalance()
        {
            AbstractAccount account = FindAccount();
            AnsiConsole.WriteLine(account.Money);
        }

        private AbstractAccount FindAccount()
        {
            if (_centralBank.GetBanksNames().Count == 0)
            {
                throw new BanksException("Any bank hasn't registered");
            }

            string bankName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(5)
                    .AddChoices(_centralBank.GetBanksNames()));
            AnsiConsole.Clear();
            foreach (Bank item in _centralBank.GetBanks())
            {
                if (item.GetClientsNames().Count == 0)
                {
                    throw new BanksException("Any client hasn't registered");
                }

                if (item.Name == bankName)
                {
                    string clientInformation = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .PageSize(5)
                            .AddChoices(item.GetClientsNames()));
                    AnsiConsole.Clear();
                    foreach (Client itemClient in item.GetClients())
                    {
                        if (itemClient.GetInformationAccounts().Count == 0)
                        {
                            throw new BanksException("Any account hasn't registered");
                        }

                        if (itemClient.GetPersonalInformation() == clientInformation)
                        {
                            string accountInformation = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .PageSize(5)
                                    .AddChoices(itemClient.GetInformationAccounts()));
                            AnsiConsole.Clear();
                            foreach (AbstractAccount itemAccount in itemClient.GetAccounts())
                            {
                                if (itemAccount.GetInformation() == accountInformation)
                                {
                                    return itemAccount;
                                }
                            }
                        }
                    }
                }
            }

            throw new BanksException("Account wasn't found");
        }

        private void UpdateMoneyInformation()
        {
            DateTime nowDate = DateTime.Today;
            int changeDay = AnsiConsole.Ask<int>("How many days you want to move to update money information? ");
            nowDate = nowDate.AddDays(changeDay);
            _centralBank.UpdateMoneyInformation(nowDate);
        }

        private void AddClientAndLinkWithAccount()
        {
            if (_centralBank.GetBanksNames().Count == 0)
            {
                throw new BanksException("Any bank hasn't registered");
            }

            string bankName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(5)
                    .AddChoices(_centralBank.GetBanksNames()));
            AnsiConsole.Clear();

            foreach (Bank item in _centralBank.GetBanks())
            {
                if (item.Name == bankName)
                {
                    var typesAccount = new List<string>
                    {
                        "Debit",
                        "Deposit",
                        "Credit",
                    };
                    string typeAccountCreator = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .PageSize(typesAccount.Count)
                            .AddChoices(typesAccount));
                    double money = AnsiConsole.Ask<double>("How many money do you want to put to account? ");
                    AbstractAccountFactory createAccount;
                    switch (typeAccountCreator)
                    {
                        case "Debit":
                            createAccount = new DebitAccountFactory(money);
                            break;
                        case "Deposit":
                            int finishDay =
                                AnsiConsole.Ask<int>("For how many days do you want to open a Deposit Account = ");
                            createAccount = new DepositAccountFactory(money, DateTime.Today.AddDays(finishDay));
                            break;
                        case "Credit":
                            createAccount = new CreditAccountFactory(money);
                            break;
                        default:
                            throw new BanksException("Error in input data");
                    }

                    var clientBuilder = new ClientBuilder();
                    Client client = clientBuilder
                        .BuildName(AnsiConsole.Ask<string>("Client Name (required field) is = "))
                        .BuildSurname(AnsiConsole.Ask<string>("Client Surname (required field) is = "))
                        .BuildAddress(AnsiConsole.Prompt(
                            new TextPrompt<string>("Client Address is (not required field) = ")
                                .AllowEmpty()))
                        .BuildPassportNumber(AnsiConsole.Prompt(
                            new TextPrompt<int>("Passport Number is (not required field) = ")))
                        .GetResult();
                    _centralBank.AddClientAndLinkAccount(item, client, createAccount);
                    break;
                }
            }
        }
    }
}