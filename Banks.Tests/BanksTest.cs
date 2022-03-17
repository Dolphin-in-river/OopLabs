using System;
using System.Collections.Generic;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        private string _bankName;
        private ICentralBank _centralBank;
        private double _debitPercent;
        private DepositInfo _depositInfo;
        private double _creditLimit;
        private double _creditCommission;
        private double _criticalSum;

        [SetUp]
        public void Setup()
        {
            _bankName = "Raspberry";
            _centralBank = new CentralBank();
            _debitPercent = 3.65;
            var sumInDeposits = new List<double>
            {
                50000,
                150000,
                500000
            };
            var percentsInDeposits = new List<double>
            {
                10.95,
                7.3,
                3.65
            };
            _depositInfo = new DepositInfo(sumInDeposits, percentsInDeposits);
            _creditLimit = 50000;
            _creditCommission = 36500;
            _criticalSum = 10000;
        }

        [Test]
        public void WithDrawMoneyOnUnsafeAccount()
        {
            Assert.Catch<BanksException>(() =>
            {
                var bankBuilder = new BankBuilder();
                Bank bank = bankBuilder.BuildName(_bankName).BuildDebitPercent(_debitPercent)
                    .BuildDepositInfo(_depositInfo)
                    .BuildCreditLimit(_creditLimit).BuildCreditCommission(_creditCommission)
                    .BuildCriticalSum(_criticalSum).GetResult();
                _centralBank.AddBank(bank);
                var clientBuilder = new ClientBuilder();
                Client client = clientBuilder.BuildName("Ivan").BuildSurname("Hryakov").BuildAddress("Orel")
                    .GetResult();
                AbstractAccount newDebitAccount =
                    _centralBank.AddClientAndLinkAccount(bank, client, new DebitAccountFactory(100000));
                
                _centralBank.MakeTransaction(new WithDrawOperationFactory(newDebitAccount, 12000));
            });
        }

        [Test]
        public void WithDrawMoneyOnSafeAccount()
        {
            var bankBuilder = new BankBuilder();
            Bank bank = bankBuilder.BuildName(_bankName).BuildDebitPercent(_debitPercent)
                .BuildDepositInfo(_depositInfo)
                .BuildCreditLimit(_creditLimit).BuildCreditCommission(_creditCommission)
                .BuildCriticalSum(_criticalSum).GetResult();
            _centralBank.AddBank(bank);
            var clientBuilder = new ClientBuilder();
            Client client = clientBuilder.BuildName("Ivan").BuildSurname("Hryakov").BuildAddress("Orel")
                .BuildPassportNumber(123123)
                .GetResult();
            AbstractAccount newDebitAccount =
                _centralBank.AddClientAndLinkAccount(bank, client, new DebitAccountFactory(100000));
            _centralBank.MakeTransaction(new WithDrawOperationFactory(newDebitAccount, 20000));
            Assert.AreEqual(80000, newDebitAccount.Money);
        }

        [Test]
        public void CheckCountMoneyOnDebitAccount()
        {
            var bankBuilder = new BankBuilder();
            Bank bank = bankBuilder.BuildName(_bankName).BuildDebitPercent(_debitPercent)
                .BuildDepositInfo(_depositInfo)
                .BuildCreditLimit(_creditLimit).BuildCreditCommission(_creditCommission)
                .BuildCriticalSum(_criticalSum).GetResult();
            _centralBank.AddBank(bank);
            var clientBuilder = new ClientBuilder();
            Client client = clientBuilder.BuildName("Ivan").BuildSurname("Hryakov").BuildAddress("Orel")
                .BuildPassportNumber(123123)
                .GetResult();
            AbstractAccount newDebitAccount =
                _centralBank.AddClientAndLinkAccount(bank, client, new DebitAccountFactory(100000));
            DateTime nowDate = DateTime.Today;
            nowDate = nowDate.AddDays(30);
            _centralBank.UpdateMoneyInformation(nowDate);
            Assert.AreEqual(100300, newDebitAccount.Money);
        }

        [Test]
        public void CheckCountMoneyOnDepositAccount()
        {
            var bankBuilder = new BankBuilder();
            Bank bank = bankBuilder.BuildName(_bankName).BuildDebitPercent(_debitPercent)
                .BuildDepositInfo(_depositInfo)
                .BuildCreditLimit(_creditLimit).BuildCreditCommission(_creditCommission)
                .BuildCriticalSum(_criticalSum).GetResult();
            _centralBank.AddBank(bank);
            var clientBuilder = new ClientBuilder();
            Client client = clientBuilder.BuildName("Ivan").BuildSurname("Hryakov").BuildAddress("Orel")
                .BuildPassportNumber(123123)
                .GetResult();
            DateTime nowDate = DateTime.Today;
            DateTime finishDay = nowDate.AddDays(365);
            AbstractAccount newDepositAccount =
                _centralBank.AddClientAndLinkAccount(bank, client, new DepositAccountFactory(100000, finishDay));

            _centralBank.UpdateMoneyInformation(nowDate.AddDays(30));
            Assert.AreEqual(100600, newDepositAccount.Money);
        }

        [Test]
        public void CheckCountMoneyOnCreditAccount()
        {
            var bankBuilder = new BankBuilder();
            Bank bank = bankBuilder.BuildName(_bankName).BuildDebitPercent(_debitPercent)
                .BuildDepositInfo(_depositInfo)
                .BuildCreditLimit(_creditLimit).BuildCreditCommission(_creditCommission)
                .BuildCriticalSum(_criticalSum).GetResult();
            _centralBank.AddBank(bank);
            var clientBuilder = new ClientBuilder();
            Client client = clientBuilder.BuildName("Ivan").BuildSurname("Hryakov").BuildAddress("Orel")
                .BuildPassportNumber(123123)
                .GetResult();
            DateTime nowDate = DateTime.Today;
            AbstractAccount newCreditAccount =
                _centralBank.AddClientAndLinkAccount(bank, client, new CreditAccountFactory(100000));
            newCreditAccount.WithDraw(110000);
            Assert.AreEqual(40000, newCreditAccount.Money);
            _centralBank.UpdateMoneyInformation(nowDate.AddDays(30)); 
            Assert.AreEqual(37000, newCreditAccount.Money);
        }

        [Test]
        public void WithDrawMoneyOnDepositAccountBeforeFinishDay()
        {
            Assert.Catch<BanksException>(() =>
            {
                var bankBuilder = new BankBuilder();
                Bank bank = bankBuilder.BuildName(_bankName).BuildDebitPercent(_debitPercent)
                    .BuildDepositInfo(_depositInfo)
                    .BuildCreditLimit(_creditLimit).BuildCreditCommission(_creditCommission)
                    .BuildCriticalSum(_criticalSum).GetResult();
                _centralBank.AddBank(bank);
                var clientBuilder = new ClientBuilder();
                Client client = clientBuilder.BuildName("Ivan").BuildSurname("Hryakov").BuildAddress("Orel")
                    .BuildPassportNumber(123123)
                    .GetResult();
                DateTime nowDate = DateTime.Today;
                DateTime finishDay = nowDate.AddDays(365);
                AbstractAccount newDepositAccount =
                    _centralBank.AddClientAndLinkAccount(bank, client, new DepositAccountFactory(100000, finishDay));
                _centralBank.UpdateMoneyInformation(nowDate.AddDays(30));
                _centralBank.MakeTransaction(new WithDrawOperationFactory(newDepositAccount, 20000));
            });
        }

        [Test]
        public void WithDrawMoneyOnDepositAccountAfterFinishDay()
        {
            var bankBuilder = new BankBuilder();
            Bank bank = bankBuilder.BuildName(_bankName).BuildDebitPercent(_debitPercent)
                .BuildDepositInfo(_depositInfo)
                .BuildCreditLimit(_creditLimit).BuildCreditCommission(_creditCommission)
                .BuildCriticalSum(_criticalSum).GetResult();
            _centralBank.AddBank(bank);
            var clientBuilder = new ClientBuilder();
            Client client = clientBuilder.BuildName("Ivan").BuildSurname("Hryakov").BuildAddress("Orel")
                .BuildPassportNumber(123123)
                .GetResult();
            DateTime nowDate = DateTime.Today;
            DateTime finishDay = nowDate.AddDays(365);
            AbstractAccount newDepositAccount =
                _centralBank.AddClientAndLinkAccount(bank, client, new DepositAccountFactory(100000, finishDay));
            _centralBank.UpdateMoneyInformation(nowDate.AddDays(365));
            Assert.AreEqual(107200, newDepositAccount.Money);
            _centralBank.MakeTransaction(new WithDrawOperationFactory(newDepositAccount, 20000));
            Assert.AreEqual(87200, newDepositAccount.Money);
        }

        [Test]
        public void CancelTransactionBetweenAccounts()
        {
            var bankBuilder = new BankBuilder();
            Bank bank = bankBuilder.BuildName(_bankName).BuildDebitPercent(_debitPercent)
                .BuildDepositInfo(_depositInfo)
                .BuildCreditLimit(_creditLimit).BuildCreditCommission(_creditCommission)
                .BuildCriticalSum(_criticalSum).GetResult();
            _centralBank.AddBank(bank);
            var clientBuilder = new ClientBuilder();
            Client client = clientBuilder.BuildName("Ivan").BuildSurname("Hryakov").BuildAddress("Orel")
                .BuildPassportNumber(123123)
                .GetResult();
            AbstractAccount firstDebitAccount =
                _centralBank.AddClientAndLinkAccount(bank, client, new DebitAccountFactory(100000));
            AbstractAccount secondDebitAccount =
                _centralBank.AddClientAndLinkAccount(bank, client, new DebitAccountFactory(200000));
            ITransaction transaction =
                _centralBank.MakeTransaction(new TransferFactory(firstDebitAccount, secondDebitAccount, 50000));
            Assert.AreEqual(50000, firstDebitAccount.Money);
            Assert.AreEqual(250000, secondDebitAccount.Money);
            _centralBank.CancelTransaction(transaction);
            Assert.AreEqual(100000, firstDebitAccount.Money);
            Assert.AreEqual(200000, secondDebitAccount.Money);
        }
        [Test]
        public void MakeChangeInConditions()
        {
            var bankBuilder = new BankBuilder();
            Bank bank = bankBuilder.BuildName(_bankName).BuildDebitPercent(_debitPercent)
                .BuildDepositInfo(_depositInfo)
                .BuildCreditLimit(_creditLimit).BuildCreditCommission(_creditCommission)
                .BuildCriticalSum(_criticalSum).GetResult();
            _centralBank.AddBank(bank);
            var clientBuilder = new ClientBuilder();
            Client client = clientBuilder.BuildName("Ivan").BuildSurname("Hryakov").BuildAddress("Orel")
                .BuildPassportNumber(123123)
                .GetResult();
            AbstractAccount newCreditAccount =
                _centralBank.AddClientAndLinkAccount(bank, client, new CreditAccountFactory(100000));
            bank.ChangeCriticalSum(50000);
            Assert.AreEqual(true, client.GetFlagNotification());
            Assert.AreEqual(50000,newCreditAccount.CriticalSum);
        }
    }
}