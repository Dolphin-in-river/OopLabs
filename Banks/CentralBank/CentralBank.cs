using System;
using System.Collections.Generic;
using Banks.Tools;

namespace Banks
{
    public class CentralBank : ICentralBank
    {
        private List<Bank> _banks;
        private DateTime _nowDate = DateTime.Today;
        private List<ITransaction> _transactions = new List<ITransaction>();

        public CentralBank()
        {
            _banks = new List<Bank>();
        }

        public Bank AddBank(string bankName, double debitPercent, DepositInfo depositInfo, double creditLimit, double creditCommission, double criticalSum)
        {
            foreach (Bank bank in _banks)
            {
                if (bank.Name == bankName)
                {
                    throw new BanksException("Bank with same name is already registered");
                }
            }

            var newBank = new Bank(bankName, debitPercent, depositInfo, creditLimit, creditCommission, criticalSum);
            _banks.Add(newBank);
            return newBank;
        }

        public void AddBank(Bank bank)
        {
            foreach (Bank item in _banks)
            {
                if (bank.Name == item.Name)
                {
                    throw new BanksException("Bank with same name is already registered");
                }
            }

            _banks.Add(bank);
        }

        public void UpdateMoneyInformation(DateTime newDate)
        {
            if (newDate.Subtract(_nowDate).Days < 0)
            {
                throw new Exception("We can't go past");
            }

            _nowDate = newDate;
            foreach (Bank bank in _banks)
            {
                bank.NotifyCalculates(newDate);
            }
        }

        public List<ITransaction> GetTransactions()
        {
            return _transactions;
        }

        public List<string> GetNamesTransaction()
        {
            var transactionInformation = new List<string>();
            foreach (ITransaction transaction in _transactions)
            {
                transactionInformation.Add(transaction.GetInformation());
            }

            return transactionInformation;
        }

        public List<Bank> GetBanks()
        {
            return _banks;
        }

        public List<string> GetBanksNames()
        {
            var names = new List<string>();
            foreach (Bank bank in _banks)
            {
                names.Add(bank.Name);
            }

            return names;
        }

        public ITransaction MakeTransaction(ITransactionFactory creatorTransaction)
        {
            ITransaction transaction = creatorTransaction.Create();
            transaction.DoTransact();
            if (transaction.GetTypeTransaction().Equals(TypeTransactions.Transfer))
            {
                var transfer = (Transfer)transaction;
                CheckExistsAccount(transfer.GetSender(), transfer.GetGiver());
            }

            _transactions.Add(transaction);
            return transaction;
        }

        public void CancelTransaction(ITransaction transaction)
        {
            transaction.Cancel();
            for (int i = 0; i < _transactions.Count; i++)
            {
                if (_transactions[i].GetId() == transaction.GetId())
                {
                    _transactions.RemoveAt(i);
                    return;
                }
            }

            throw new BanksException("This account don't exists");
        }

        public AbstractAccount AddClientAndLinkAccount(Bank bank, Client client, AbstractAccountFactory createAccount)
        {
            return bank.AddClientAndLinkAccount(client, createAccount);
        }

        private void CheckExistsAccount(AbstractAccount sender, AbstractAccount giver)
        {
            foreach (Bank bank in _banks)
            {
                if (!bank.IsAccount(sender) || !bank.IsAccount(giver))
                {
                    throw new BanksException("These Accounts don't exist");
                }
            }
        }
    }
}