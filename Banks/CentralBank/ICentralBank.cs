using System;
using System.Collections.Generic;

namespace Banks
{
    public interface ICentralBank
    {
        void UpdateMoneyInformation(DateTime newDate);
        ITransaction MakeTransaction(ITransactionFactory creatorTransaction);
        void CancelTransaction(ITransaction transaction);
        void AddBank(Bank bank);
        List<Bank> GetBanks();
        List<string> GetBanksNames();
        List<string> GetNamesTransaction();
        List<ITransaction> GetTransactions();
        AbstractAccount AddClientAndLinkAccount(Bank bank, Client client, AbstractAccountFactory createAccount);
        Bank AddBank(string bankName, double debitPercent, DepositInfo depositInfo, double creditLimit, double creditCommission, double criticalSum);
    }
}